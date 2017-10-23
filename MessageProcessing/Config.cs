using System;
using MessageProcessing.NMSActiveMQ;

namespace MessageProcessing
{
   internal class Config
   {
      /* AMQP
      const string DefaultTopic = "jms.topic.AppRequestTopic";
      const string DefaultBrokerUri = "amqp://guest:guest@127.0.0.1:5672";
      const string DefaultConsumerId = "InfoRequestService";
      */

      private const string DefaultTopic = "topic://AppRequestTopic";
      private const string DefaultBrokerUri = "activemq:failover:(tcp://$HOST:5672)?transport.startupMaxReconnectAttempts=100&transport.maxReconnectAttempts=20";
      private const string DefaultConsumerId = "InfoRequestService";

      private const string DefaultHost = "localhost";

      private const int DefaultSessionCapacity = 100;

      private readonly SettingsReader settingsReader = new SettingsReader();

      private readonly string serviceName;

      private readonly IProcessorFactory processorFactory;

      private string host;

      private int sessionCapacity;

      public Config(IProcessorFactory processorFactory)
      {
         this.processorFactory = processorFactory;
         serviceName = settingsReader.ReadAppSetting("serviceName", string.Empty);
         if (string.IsNullOrWhiteSpace(serviceName))
         {
            throw new ArgumentException("serviceName property should not be empty");
         }
         sessionCapacity = settingsReader.ReadAppSetting("sessionCapacity", DefaultSessionCapacity);
      }

      public string Host
      {
         get { return host; }
         set { host = string.IsNullOrWhiteSpace(value) ? DefaultHost : value; }
      }

      public ITopicSubscriber GetSubscriber()
      {
         string topic = settingsReader.ReadAppSetting("topic", DefaultTopic);
         string consumerId = settingsReader.ReadAppSetting("consumerId", DefaultConsumerId);
         string brokerUri = settingsReader.ReadAppSetting("brokerUri", DefaultBrokerUri).Replace("$HOST", host);
         string selector = settingsReader.ReadAppSetting("selector", string.Empty);
         string[] messageProperties = settingsReader.ReadAppSetting("messageProperties", string.Empty).Split( ',',';',' ');

         // return new AMQPTopicSubscriber(topic, brokerUri, consumerId);
         return new NmsTopicSubscriber(topic, brokerUri, consumerId, selector, messageProperties, processorFactory);
      }

      public string GetServiceName()
      {
         return serviceName;
      }

      public int GetWaitConnectionStopTimeout()
      {
         return settingsReader.ReadAppSetting("waitConnectionStop", 10000);
      }

      public int SessionCapacity
      {
         get { return sessionCapacity; }
      }
   }
}
