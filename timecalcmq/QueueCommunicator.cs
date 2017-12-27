using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;


namespace timecalcmq
{
   class QueueCommunicator: IDisposable
   {
      private readonly IConnection connection;

      private readonly string queueName = "queue://timecalc.write";

      private readonly string brokerUri = "activemq:failover:(tcp://localhost:5672)";

      public QueueCommunicator()
      {
         try
         {
            IConnectionFactory factory = new NMSConnectionFactory(brokerUri);
            connection = factory.CreateConnection();
            connection.Start();
         }
         catch (Exception)
         {
            CleanUp();
            throw;
         }
      }

      public QueueClient GetQueueClient()
      {
         return new QueueClient(connection, queueName);
      }

      private void CleanUp()
      {
         connection?.Dispose();
      }

      public void Dispose()
      {
         CleanUp();
      }
   }
}
