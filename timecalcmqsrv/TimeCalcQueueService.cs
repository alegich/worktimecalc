using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Apache.NMS;
using timecalcmq;

namespace timecalcmqsrv
{
   public partial class TimeCalcQueueService : ServiceBase
   {
      private QueueServer server;

      public TimeCalcQueueService()
      {
         InitializeComponent();
      }

      protected override void OnStart(string[] args)
      {
         //while (!Debugger.IsAttached)
         {
            Thread.Sleep(100);
         }

         try
         {
            IConnection connection =
               new NMSConnectionFactory("activemq:failover:(tcp://localhost:5672)").CreateConnection();
            server = new QueueServer(connection, "queue://timecalc.write", @"D:\data\docs\time",
               new timecalcfile.FileBasedTimeCalcFactory());
         }
         catch (Exception e)
         {
            // do something like cleanup
            e.ToString();
         }
      }

      protected override void OnStop()
      {
         server.Dispose();
      }
   }
}
