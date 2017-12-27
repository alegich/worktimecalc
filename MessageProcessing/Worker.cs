using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;

namespace MessageProcessing
{
   public abstract class Worker
   {
      private static readonly ILog logger = LogManager.GetLogger(typeof(Worker));

      private readonly IRequestProcessor processor;

      private readonly ITopicPublisher publisher;

      protected Worker(IProcessorFactory processorFactory, ITopicPublisher publisher) 
      {
         processor = processorFactory.GetProcessor();
         this.publisher = publisher;
      }

      public virtual bool DoJob()
      {
         ProcessorOutcome retVal = new ProcessorOutcome();
         try
         {
            logger.Info(string.Format("Processing request {0}:", GetMessageId()));
            logger.Debug(GetMessageText());

            Stopwatch sw = Stopwatch.StartNew();
            retVal = processor.Process(GetMessageText(), GetMessageProperties());
            sw.Stop();
            if (retVal.Result)
            {
               SendChildRequests(retVal.ChildRequests);
               AcknowledgeMessage();
               logger.Info(string.Format("Request {0} processed successfully. Elapsed {1}", GetMessageId(), sw.ElapsedMilliseconds));
            }
            else
            {
               logger.Error(string.Format("Request processing failed. Elapsed {0}", sw.ElapsedMilliseconds));
            }
         }
         catch (Exception e)
         {
            logger.Error(string.Format("Request processing failed with exception {0}", e));
         }

         return retVal.Result;
      }

      protected void SendChildRequests(List<KeyValuePair<string, Dictionary<string, object>>> childRequests)
      {
         if (childRequests.Count > 0)
         {
            logger.Info(string.Format("Publishing {0} child request(s)", childRequests.Count));
         }

         foreach (KeyValuePair<string, Dictionary<string, object>> request in childRequests)
         {
            publisher.Publish(request.Key, request.Value);
         }
      }

      protected abstract void AcknowledgeMessage();

      protected abstract string GetMessageText();

      protected abstract string GetMessageId();

      protected abstract Dictionary<string, object> GetMessageProperties();
   }
}
