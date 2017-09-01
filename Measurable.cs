using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public interface Measurable
   {
      void MeasureStarted();
      void MeasureStopped();
      void DeviceLocked();
      void DeviceUnlocked();
      void DeviceTurnedOff();
   }
}
