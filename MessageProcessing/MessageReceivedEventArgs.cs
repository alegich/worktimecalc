using System;

namespace MessageProcessing
{
   public class MessageReceivedEventArgs : EventArgs
   {
      public Worker Worker { get; set; }
   }
}
