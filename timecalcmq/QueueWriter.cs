using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;

namespace timecalcmq
{
   class QueueWriter
   {
      private IConnection connection;

      private readonly string queueName;

      public QueueWriter(IConnection connection, string queueName)
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
   }
}
