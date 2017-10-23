using System.Collections.Generic;

namespace MessageProcessing
{
   public interface IRequestProcessor
   {
      ProcessorOutcome Process(string request);
   }
}
