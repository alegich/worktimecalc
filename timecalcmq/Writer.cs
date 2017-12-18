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
         _queueClient.SendActionMessage(timecalclib.Action.Lock, time);
      }

      public void WriteStart(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.Action.Start, time);
      }

      public void WriteStop(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.Action.Stop, time);
      }

      public void WriteUnlock(DateTime time)
      {
         _queueClient.SendActionMessage(timecalclib.Action.Unlock, time);
      }
   }
}
