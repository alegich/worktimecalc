using System.Collections.Generic;
using Apache.NMS;

namespace MessageProcessing.NMSActiveMQ
{
   internal class NmsWorker : Worker
   {
      private readonly ITextMessage message;

      public NmsWorker(ITextMessage message, IProcessorFactory processorFactory, ITopicPublisher publisher)
         : base(processorFactory, publisher)
      {
         this.message = message;
      }

      protected override void AcknowledgeMessage()
      {
         message.Acknowledge();
      }

      protected override string GetMessageText()
      {
         return message.Text;
      }

      protected override string GetMessageId()
      {
         return message.NMSMessageId;
      }

      protected override Dictionary<string, object> GetMessageProperties()
      {
         Dictionary <string, object> retVal = new Dictionary<string, object>();

         foreach (string key in message.Properties.Keys)
         {
            retVal.Add(key, message.Properties[key]);
         }

         return retVal;
      }
   }
}
