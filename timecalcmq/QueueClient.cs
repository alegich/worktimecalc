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
            message.Properties.SetLong("actionTime", actionTime.Ticks);
            message.Properties.SetString("actionType", actionType);

            producer.Send(message);

            session.Close();
         }
      }

      public IList GetDayLog(DateTime date)
      {
         IList retVal;

         using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
         {
            IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);
            IQueue responseQueue = session.CreateTemporaryQueue();
            IMessageProducer producer = session.CreateProducer(queue);
            IMessageConsumer consumer = session.CreateConsumer(responseQueue);

            ITextMessage message = producer.CreateTextMessage();
            message.Text = "GET";
            message.Properties.SetLong("actionTime", date.Ticks);
            producer.Send(message);
            IMessage response = consumer.Receive(TimeSpan.FromSeconds(10));
            retVal = response.Properties.GetList("DATA");

            session.Close();
         }

         return retVal ?? new ArrayList();
      }
   }
}
