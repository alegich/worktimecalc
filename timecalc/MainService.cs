using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using timecalclib;

namespace timecalc
{
   public partial class MainService : ServiceBase
   {
      public MainService()
      {
         InitializeComponent();

         measurer = new TimeMeasurer(new FileWriter(dataFolder));
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

      private readonly string dataFolder = @"D:\data\docs\time";

      private readonly Measurable measurer;

   }
}
