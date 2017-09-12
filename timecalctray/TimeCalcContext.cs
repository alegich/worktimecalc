using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using timecalclib;

namespace timecalctray
{
   public class TimeCalcContext : ApplicationContext
   {
      NotifyIcon notifyIcon = new NotifyIcon();
      System.Timers.Timer timer = new System.Timers.Timer();
      int warningLevel = 0;
      MenuItem overtimeItem;

      public TimeCalcContext()
      {
         MenuItem hibernateMenuItem = new MenuItem("Hibernate", new EventHandler(HibernateSystem));
         MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));
         overtimeItem = new MenuItem("Work for 10 minutes", new EventHandler(Overtime));
         overtimeItem.Visible = false;

         notifyIcon.Icon = timecalctray.Properties.Resources.AppIcon;
         notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { exitMenuItem, hibernateMenuItem, overtimeItem });
         notifyIcon.MouseClick += new MouseEventHandler(ShowTime);
         notifyIcon.MouseMove += new MouseEventHandler(UpdateHint);
         notifyIcon.Visible = true;
         

         timer.Elapsed += new ElapsedEventHandler(OnTimerEvent);
         timer.Interval = 30000;
         timer.Enabled = true;
      }

      private void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
      {
         throw new NotImplementedException();
      }

      string FormatTimespan(TimeSpan value)
      {
         return value.ToString(@"hh\:mm\:ss");
      }

      private void OnTimerEvent(object sender, ElapsedEventArgs e)
      {
         TimeSpan timeLeft = TimeLeft();
         string timeLeftFormatted = FormatTimespan(timeLeft);

         if (timeLeft <= (new TimeSpan()))
         {
            notifyIcon.ShowBalloonTip(3000, "Are you still here?", timeLeftFormatted, ToolTipIcon.Error);
            overtimeItem.Visible = true;
         }
         else if (warningLevel == 1 && timeLeft <= (new TimeSpan(0, 5, 0)))
         {
            notifyIcon.ShowBalloonTip(10000, "Time Left!", timeLeftFormatted, ToolTipIcon.Warning);
            ++warningLevel;
         }
         else if (warningLevel == 0 && timeLeft <= (new TimeSpan(0, 30, 0)))
         {
            notifyIcon.ShowBalloonTip(30000, "Time Left", timeLeftFormatted, ToolTipIcon.Info);
            ++warningLevel;
         }
         else if (warningLevel == 2)
         {
            warningLevel = 0;
         }

         timer.Interval = 30000;
      }

      string folder = @"D:\data\docs\time";

      TimeSpan TimeLeft()
      {
         
         FileBasedWeekReporter week = new FileBasedWeekReporter(DateTime.Now.Date, folder);
         return week.TimeLeft();
      }

      TimeSpan TodaysAway()
      {
         FileBasedReporter day = new FileBasedReporter(folder, DateTime.Now.Date);
         return day.AwayDuration();
      }

      TimeSpan GetWeeksBalance()
      {
         FileBasedWeekReporter week = new FileBasedWeekReporter(DateTime.Now.Date, folder);
         return week.TodaysBalance();
      }

      string GetTimeOutput()
      {
         TimeSpan timeLeft = TimeLeft();
         return "Left " + timeLeft.ToString(@"hh\:mm\:ss") +
            "\nLeave " + (DateTime.Now + timeLeft).ToShortTimeString() +
            "\nAway " + TodaysAway().ToString(@"hh\:mm\:ss") +
            "\nBalance " + GetWeeksBalance();
      }

      void UpdateHint(object sender, MouseEventArgs e)
      {
         notifyIcon.Text = GetTimeOutput();
      }

      void ShowTime(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left && e.Clicks == 0)
         {
            notifyIcon.ShowBalloonTip(3000, "Time Left", GetTimeOutput(), ToolTipIcon.Info);
         }
      }

      void Exit(object sender, EventArgs e)
      {
         notifyIcon.Visible = false;

         Application.Exit();
      }

      void HibernateSystem(object sender, EventArgs e)
      {
         Process.Start(@"cmd.exe", @"/C shutdown /h /f");
      }

      void Overtime(object sender, EventArgs e)
      {
         overtimeItem.Visible = false;
         timer.Interval = 600000;
      }
   }
}
