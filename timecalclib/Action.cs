using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public class Action
   {
      public static readonly string Start = @"Start";
      public static readonly string Stop = @"Stop";
      public static readonly string Lock = @"Lock";
      public static readonly string Unlock = @"Unlock";

      public static bool IsAwayAction(string action)
      {
         return Stop.Equals(action) || Lock.Equals(action);
      }
   }
}
