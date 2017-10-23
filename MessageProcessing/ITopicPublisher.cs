using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageProcessing
{
   public interface ITopicPublisher : IDisposable
   {
      void Publish(string request);
   }
}
