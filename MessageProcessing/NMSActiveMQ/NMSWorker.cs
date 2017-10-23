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
   }
}
