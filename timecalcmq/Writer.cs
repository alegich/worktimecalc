using System;

namespace timecalcmq
{
   class Writer : timecalclib.Writable
   {
      private Apache.NMS.IConnection connection;

      private void SendActionMessage(string actionType, DateTime actionTime)
      {
         Apache.NMS.ISession session = connection.CreateSession(Apache.NMS.AcknowledgementMode.AutoAcknowledge);
         Apache.NMS.IMessageProducer producer = session.CreateProducer();

         Apache.NMS.ITextMessage message = producer.CreateTextMessage();
         message.Properties.SetLong("actionTime", actionTime.Ticks);
         message.Properties.SetString("actionType", actionType);

         producer.Send(message);

         session.Close();
      }

      public Writer()
      {
         Apache.NMS.IConnectionFactory factory = new Apache.NMS.NMSConnectionFactory("", null);
         connection = factory.CreateConnection();
      }

      public void WriteLock(DateTime time)
      {
         SendActionMessage(timecalclib.Action.Lock, time);
      }

      public void WriteStart(DateTime time)
      {
         SendActionMessage(timecalclib.Action.Start, time);
      }

      public void WriteStop(DateTime time)
      {
         SendActionMessage(timecalclib.Action.Stop, time);
      }

      public void WriteUnlock(DateTime time)
      {
         SendActionMessage(timecalclib.Action.Unlock, time);
      }
   }
}
