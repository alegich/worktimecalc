using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageProcessing
{
   public class ProcessorOutcome
   {
      public ProcessorOutcome(bool result, List<KeyValuePair<string, Dictionary<string, object>>> childRequests)
      {
         Result = result;
         ChildRequests = childRequests;
      }

      public ProcessorOutcome() : this(false, new List<KeyValuePair<string, Dictionary<string, object>>>())
      {
      }

      public bool Result { get; private set; }

      public List<KeyValuePair<string, Dictionary<string, object>>> ChildRequests { get; private set; }
   }
}
