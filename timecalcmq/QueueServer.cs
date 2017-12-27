using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Transport;
using timecalclib;

namespace timecalcmq
{
   public class QueueServer: IDisposable
   {
      private IConnection connection;

      private readonly ISession session;
      private readonly IMessageConsumer consumer;

      private readonly string folder;

      private Writable writer;

      private bool isDisposed;

      public QueueServer(IConnection connection, string queueName, string folder, timecalclib.TimeCalcFactory factory)
      {
         this.connection = connection;
         this.folder = folder;
         writer = factory.CreateWriter();

         session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

         IQueue queue = Apache.NMS.Util.SessionUtil.GetQueue(session, queueName);
         consumer = session.CreateConsumer(queue);

         consumer.Listener += OnMessage;

         connection.Start();
      }

      private void OnMessage(IMessage message)
      {
         try
         {
            ITextMessage textMessage = message as ITextMessage;

            DateTime day = new DateTime(message.Properties.GetLong("actionTime"));

            if (textMessage != null)
            {
               if ("GET".Equals(textMessage.Text))
               {
                  IMessage response = session.CreateObjectMessage(GetContent(day));
                  response.NMSCorrelationID = message.NMSCorrelationID;
                  IMessageProducer producer = session.CreateProducer();
                  producer.Send(message.NMSReplyTo, response);
               }
               else if ("PUT".Equals(textMessage.Text))
               {
                  string action = textMessage.Properties.GetString("actionType");

                  if (TimeAction.Start.Equals(action)) writer.WriteStart(day);
                  if (TimeAction.Stop.Equals(action)) writer.WriteStop(day);
                  if (TimeAction.Lock.Equals(action)) writer.WriteLock(day);
                  if (TimeAction.Unlock.Equals(action)) writer.WriteUnlock(day);
               }
            }
         }
         catch (Exception e)
         {
            // log exception
            e.ToString();
         }
      }

      private List<string> GetContent(DateTime day)
      {
         DataFormatter formatter = new DataFormatter();
         string fileName = folder + @"\" + formatter.FormatDate(day);
         return File.Exists(fileName) ? File.ReadAllLines(fileName).ToList() : new List<string>();
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
