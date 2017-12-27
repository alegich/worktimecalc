using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;

namespace timecalcmq
{
   class QueueClient
   {
      private IConnection connection;

      private readonly string queueName;

      public QueueClient(IConnection connection, string queueName)
      {
         this.connection = connection;
         this.queueName = queueName;
      }

      public void SendActionMessage(string actionType, DateTime actionTime)
      {
         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);

            IMessageProducer producer = session.CreateProducer(queue);

            ITextMessage message = producer.CreateTextMessage();
            message.Text = "PUT";
            message.Properties.SetLong("actionTime", actionTime.Ticks);
            message.Properties.SetString("actionType", actionType);

            producer.Send(message);

            session.Close();
         }
      }

      public List<string> GetDayLog(DateTime date)
      {
         List<string> retVal = null;

         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);
            IQueue responseQueue = session.CreateTemporaryQueue();
            IMessageProducer producer = session.CreateProducer(queue);
            IMessageConsumer consumer = session.CreateConsumer(responseQueue);

            ITextMessage message = producer.CreateTextMessage();
            message.Text = "GET";
            message.Properties.SetLong("actionTime", date.Ticks);
            message.NMSReplyTo = responseQueue;
            message.NMSCorrelationID = responseQueue.QueueName;
            producer.Send(message);
            IMessage response = consumer.Receive(TimeSpan.FromSeconds(10));
            if (response != null && response is IObjectMessage)
            {
               object responseRaw = (response as IObjectMessage).Body;
               retVal = (List<string>) responseRaw;
            }

            session.Close();
         }

         return retVal ?? new List<string>();
      }
   }
}
