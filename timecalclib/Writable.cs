using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public interface Writable
   {
      void WriteStart(DateTime time);
      void WriteStop(DateTime time);
      void WriteLock(DateTime time);
      void WriteUnlock(DateTime time);
   }
}
