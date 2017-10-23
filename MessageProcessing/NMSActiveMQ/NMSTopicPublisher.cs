using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;
using DHCVCommon.Logging;

namespace MessageProcessing.NMSActiveMQ
{
   class NMSTopicPublisher: ITopicPublisher
   {
      private static readonly Logger log = Logger.GetLogger(typeof(NMSTopicPublisher));

      private readonly ISession session;
      private readonly IMessageProducer producer;
      private readonly string[] messageProperties;
      private bool isDisposed = false;

      public NMSTopicPublisher(IConnection connection, ITopic topic, string[] messageProperties)
      {
         this.messageProperties = messageProperties;
         session = connection.CreateSession();
         producer = session.CreateProducer(topic);
         producer.DeliveryMode = MsgDeliveryMode.Persistent;
      }

      public void Publish(string request)
      {
         IMessage message = producer.CreateTextMessage(request);
         foreach (string prop in messageProperties)
         {
            message.Properties[prop] = true;
         }
         message.NMSDeliveryMode = MsgDeliveryMode.Persistent;
         producer.Send(message);

         log.Debug(string.Format("Message published: {0}", message));
      }
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      protected virtual void Dispose(bool disposing)
      {
         if (disposing && !isDisposed)
         {
            session.Dispose();
            producer.Dispose();
            isDisposed = true;
         }
      }
   }
}
