using System.ServiceProcess;
using timecalcfile;
using timecalclib;

namespace timecalc
{
   public partial class MainService : ServiceBase
   {
      public MainService()
      {
         InitializeComponent();

            FileBasedTimeCalcFactory factory = new FileBasedTimeCalcFactory();

         measurer = new TimeMeasurer(factory.CreateWriter());
      }

      protected override void OnStart(string[] args)
      {
         measurer.MeasureStarted();
      }

      protected override void OnStop()
      {
         measurer.MeasureStopped();
      }

      protected override void OnSessionChange(SessionChangeDescription changeDescription)
      {
         base.OnSessionChange(changeDescription);
         if (changeDescription.Reason == SessionChangeReason.SessionLock)
         {
            measurer.DeviceLocked();
         }
         else if (changeDescription.Reason == SessionChangeReason.SessionUnlock)
         {
            measurer.DeviceUnlocked();
         }
      }

      protected override void OnShutdown()
      {
         base.OnShutdown();
         measurer.DeviceTurnedOff();
      }

      protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
      {
         if (powerStatus == PowerBroadcastStatus.Suspend)
         {
            measurer.DeviceTurnedOff();
         }
         return base.OnPowerEvent(powerStatus);
      }

      private readonly Measurable measurer;

   }
}
