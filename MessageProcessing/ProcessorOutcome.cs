using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageProcessing
{
   public class ProcessorOutcome
   {
      public ProcessorOutcome(bool result, List<string> childRequests)
      {
         Result = result;
         ChildRequests = childRequests;
      }

      public ProcessorOutcome() : this(false, new List<string>())
      {
      }

      public bool Result { get; private set; }

      public List<string> ChildRequests { get; private set; }
   }
}
