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

      protected override Dictionary<string, string> GetMessageProperties()
      {
         Dictionary <string, string> retVal = new Dictionary<string, string>();

         foreach (string key in message.Properties.Keys)
         {
            retVal.Add(key, message.Properties[key].ToString());
         }

         return retVal;
      }
   }
}
