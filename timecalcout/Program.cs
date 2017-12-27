using System;
using timecalcfile;
using timecalcmq;
using timecalclib;

namespace timecalcout
{
   public static class Util
   {
      public static string FormatTimespan(this TimeSpan value)
      {
         string format = value.Hours > 0 ? @"{0} h, {1} min" : @"{1} min, {2} sec";
         return string.Format(format, value.Hours, value.Minutes, value.Seconds);
      }

      public static string FormatTime(this DateTime value)
      {
         return value.ToString(@"HH\:mm\:ss");
      }
   }
   class Program
   {
      static void Main(string[] args)
      {
         DateTime date = args.Length > 0 ? (new DataFormatter()).ParseDate(args[0]) : DateTime.Now.Date;

         //FileBasedTimeCalcFactory factory = new FileBasedTimeCalcFactory();
         MessageBasedTimeCalcFactory factory = new MessageBasedTimeCalcFactory();
         Reportable reporter = factory.CreateReporter(date);
         Console.WriteLine("Work started: {0}", reporter.WorkStarted().ToLocalTime());
         Console.WriteLine("Work ended: {0}", reporter.WorkEnded().ToLocalTime());
         foreach (var timePair in reporter.AwaySessions())
         {
            Console.WriteLine("Away: {0} - {1} [{2}]", timePair.Key.ToLocalTime().FormatTime(), 
               timePair.Value.ToLocalTime().FormatTime(), timePair.Value.Subtract(timePair.Key).FormatTimespan());
         }
         Console.WriteLine("Whole time spent: {0}", reporter.WholeDayTime().FormatTimespan());
         Console.WriteLine("Work time: {0}", reporter.WorkDuration().FormatTimespan());
         Console.WriteLine("Away time: {0}", reporter.AwayDuration().FormatTimespan());
         /*
         WeekReportable week = factory.CreateWeekReporter(date);

         Console.WriteLine("Time left: {0}", week.TimeLeft().FormatTimespan());*/

         Console.WriteLine("Lunch: {0} - {1} [{2}]", 
            reporter.LunchStarted().ToLocalTime().FormatTime(), reporter.LunchEnded().ToLocalTime().FormatTime(),
            reporter.LunchDuration().FormatTimespan());

         Console.WriteLine("To show information for other day: timecalcout yyyy.MM.dd");
      }
   }
}
