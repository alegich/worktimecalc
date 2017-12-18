using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using timecalclib;

namespace timecalcmq
{
   public class QueueServer: IDisposable
   {
      private IConnection connection;

      private readonly ISession session;
      private readonly IMessageConsumer consumer;

      private readonly string folder;

      private bool isDisposed;

      public QueueServer(IConnection connection, string queueName, string folder)
      {
         this.connection = connection;
         this.folder = folder;

         session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

         IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);
         consumer = session.CreateConsumer(queue);

         consumer.Listener += OnMessage;
      }

      private void OnMessage(IMessage message)
      {
         ITextMessage textMessage = message as ITextMessage;
         if (textMessage != null && "GET".Equals(textMessage.Text))
         {
            DateTime day = new DateTime(message.Properties.GetLong("actionTime"));
            DataFormatter formatter = new DataFormatter();
            string fileName = folder + @"\" + formatter.FormatDate(day);
            List<string> content = File.Exists(fileName) ? File.ReadAllLines(fileName).ToList() : new List<string>();

            IMessage response = session.CreateMessage();
            response.NMSCorrelationID = message.NMSCorrelationID;
            response.Properties.SetList("DATA", content);

            IDestination dest = message.NMSReplyTo;
            IMessageProducer producer = session.CreateProducer(dest);
            producer.Send(response);
         }
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
            CleanUpResources();
            isDisposed = true;
         }
      }

      private void CleanUpResources()
      {
         connection?.Close();
         consumer?.Dispose();
         session?.Dispose();
      }
   }
}
