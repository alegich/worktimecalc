using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageProcessing
{
   public delegate void MessageReceivedDelegate(object sender, MessageReceivedEventArgs e);

   public interface ITopicSubscriber : IDisposable
   {
      event MessageReceivedDelegate OnMessageReceived;

      void StartAcceptingMessages();

      void StopAcceptingMessages();

      void CommitSession();
   }
}
