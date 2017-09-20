using System;

namespace timecalcmq
{
   class Writer : timecalclib.Writable
   {
      private readonly QueueWriter queueWriter;

      public Writer(QueueWriter queueWriter)
      {
         this.queueWriter = queueWriter;
      }

      public void WriteLock(DateTime time)
      {
         queueWriter.SendActionMessage(timecalclib.Action.Lock, time);
      }

      public void WriteStart(DateTime time)
      {
         queueWriter.SendActionMessage(timecalclib.Action.Start, time);
      }

      public void WriteStop(DateTime time)
      {
         queueWriter.SendActionMessage(timecalclib.Action.Stop, time);
      }

      public void WriteUnlock(DateTime time)
      {
         queueWriter.SendActionMessage(timecalclib.Action.Unlock, time);
      }
   }
}
