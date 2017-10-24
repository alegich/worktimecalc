using System;
using Apache.NMS;
using Apache.NMS.Util;
using log4net;

namespace MessageProcessing.NMSActiveMQ
{
   public class NmsTopicSubscriber : ITopicSubscriber
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(NmsTopicSubscriber));

      private readonly IProcessorFactory processorFactory;
      private readonly IConnection connection;
      private readonly ISession session;
      private readonly IMessageConsumer consumer;
      private bool isDisposed = false;
      private readonly NMSTopicPublisher publisher;

      public NmsTopicSubscriber(string topicName, string brokerUri, string consumerId, 
         string selector, string[] messageProperties,
         IProcessorFactory processorFactory)
      {
         this.processorFactory = processorFactory;
         Tracer.Trace = new ServiceTracer();

         try
         {
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);
            connection = factory.CreateConnection();
            connection.ExceptionListener += Connection_ExceptionListener;
            connection.ConnectionInterruptedListener += Connection_ConnectionInterruptedListener;
            connection.ConnectionResumedListener += Connection_ConnectionResumedListener;
            connection.ClientId = consumerId;
            session = connection.CreateSession(AcknowledgementMode.IndividualAcknowledge);
            session.TransactionCommittedListener += Session_TransactionCommittedListener;
            session.TransactionRolledBackListener += Session_TransactionRolledBackListener;
            session.TransactionStartedListener += Session_TransactionStartedListener;
            ITopic topic = SessionUtil.GetTopic(session, topicName);
            selector = string.IsNullOrEmpty(selector) ? null : selector;
            consumer = session.CreateDurableConsumer(topic, consumerId, selector, false);
            publisher = new NMSTopicPublisher(connection, topic, messageProperties);
            log.Debug(string.Format("Durable subscriber <{0}> created for topic <{1}>, selector <{2}> on connection <{3}> with client id <{4}>"
                  ,consumerId, topicName, selector, brokerUri, connection.ClientId));
         }
         catch (Exception e)
         {
            log.Error("NMSTopicSubscriber init is failed.", e);
            CleanUpResources();

            throw;
         }
      }

      public event MessageReceivedDelegate OnMessageReceived;

      public void StartAcceptingMessages()
      {
         consumer.Listener += OnMessage;
         connection.Start();
      }
      
      public void StopAcceptingMessages()
      {
         consumer.Listener -= OnMessage;
      }

      public void CommitSession()
      {
         session.Commit();
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

      private void OnMessage(IMessage message)
      {
         ITextMessage textMessage = message as ITextMessage;
         if (OnMessageReceived != null && textMessage != null)
         {
            MessageReceivedEventArgs e = new MessageReceivedEventArgs()
            {
               Worker = new NmsWorker(textMessage, processorFactory, publisher)
            };
            OnMessageReceived(this, e);
         }
      }

      private void Session_TransactionStartedListener(ISession s)
      {
         log.Info("Transaction started");
      }

      private void Session_TransactionRolledBackListener(ISession s)
      {
         log.Info("Transaction rolled back");
      }

      private void Session_TransactionCommittedListener(ISession s)
      {
         log.Info("Transaction committed");
      }

      private void Connection_ConnectionInterruptedListener()
      {
         log.Warn("Connection interrupted");
      }

      private void Connection_ConnectionResumedListener()
      {
         log.Warn("Connection resumed");
      }

      private void Connection_ExceptionListener(Exception exception)
      {
         log.Error("Connection exception: ", exception);
      }

      private void CleanUpResources()
      {
         if (publisher != null)
         {
            publisher.Dispose();
         }

         if (connection != null)
         {
            connection.Close();
         }

         if (consumer != null)
         {
            consumer.Dispose();
         }

         if (session != null)
         {
            session.Dispose();
         }

         if (connection != null)
         {
            connection.Dispose();
         }

         log.Info("NMSTopicSubscriber is disconnected");
      }
   }
}
