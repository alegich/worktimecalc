using System;

namespace timecalcmq
{
   class Writer : timecalclib.Writable
   {
      private readonly QueueClient _queueClient;

      public Writer(QueueClient _queueClient)
      {
         this._queueClient = _queueClient;
      }

      public void WriteLock(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.TimeAction.Lock, time);
      }

      public void WriteStart(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.TimeAction.Start, time);
      }

      public void WriteStop(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.TimeAction.Stop, time);
      }

      public void WriteUnlock(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.TimeAction.Unlock, time);
      }
   }
}
