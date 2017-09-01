using System;

namespace timecalclib
{
   public class TimeMeasurer: Measurable
   {
      public TimeMeasurer(Writable writer)
      {
         this.writer = writer;
      }

      public void DeviceLocked()
      {
         writer.WriteLock(DateTime.UtcNow);
      }

      public void DeviceUnlocked()
      {
         writer.WriteUnlock(DateTime.UtcNow);
      }

      public void MeasureStarted()
      {
         writer.WriteStart(DateTime.UtcNow);
      }

      public void MeasureStopped()
      {
         writer.WriteStop(DateTime.UtcNow);
      }

      public void DeviceTurnedOff()
      {
         writer.WriteLock(DateTime.UtcNow);
      }

      private readonly Writable writer;
   }
}
