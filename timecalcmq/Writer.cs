using System;
using Apache.NMS;

namespace timecalcmq
{
   class Writer : timecalclib.Writable, IDisposable
   {
      private IConnection connection;

      private readonly string queueName = "queue://timecalc.write";

      private readonly string brokerUri = "activemq:failover:(tcp://localhost:5672)";

      private void SendActionMessage(string actionType, DateTime actionTime)
      {
         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);

            IMessageProducer producer = session.CreateProducer(queue);

            ITextMessage message = producer.CreateTextMessage();
            message.Properties.SetLong("actionTime", actionTime.Ticks);
            message.Properties.SetString("actionType", actionType);

            producer.Send(message);

            session.Close();
         }
      }

      public Writer()
      {
         try
         {
            IConnectionFactory factory = new NMSConnectionFactory(brokerUri);
            connection = factory.CreateConnection();
         }
         catch (Exception)
         {
            CleanUp();
            throw;
         }
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

      private void CleanUp()
      {
         connection.Dispose();
      }

      public void Dispose()
      {
         CleanUp();
      }
   }
}
