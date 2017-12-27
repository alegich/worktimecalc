using System.Collections.Generic;

namespace MessageProcessing
{
   public interface IRequestProcessor
   {
      ProcessorOutcome Process(string request, Dictionary<string, object> properties);
   }
}
