using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using DHCVCommon.Logging;

namespace MessageProcessing
{
   public partial class MessageConsumerService : ServiceBase
   {
      private static readonly Logger log = Logger.GetLogger(typeof(MessageConsumerService));

      private readonly Config config;

      private JobDispatcher jobDispatcher;
      private ITopicSubscriber subscriber;
      private Task connectionTask;

      public MessageConsumerService(IProcessorFactory processorFactory) 
      {
         config = new Config(processorFactory);
         InitializeComponent();
         ServiceName = config.GetServiceName();
      }

      public void Open(string[] args)
      {
         OnStart(args);
      }

      protected override void OnStart(string[] args)
      {
         log.Info(string.Format("{0} {1}", "Service started at", DateTime.Now.ToString("[hh:mm:ss.fff]")));
         log.Info(string.Format("Session capacity is: {0}", config.SessionCapacity));

         try
         {
            config.Host = args.Length > 0 ? args[0] : null;
            connectionTask = Task.Factory.StartNew(() =>
            {
               subscriber = config.GetSubscriber();
               jobDispatcher = new JobDispatcher(null /*subscriber.CommitSession*/, config.SessionCapacity);
               subscriber.OnMessageReceived += jobDispatcher.DispatchJob;
               subscriber.StartAcceptingMessages();
            });
            connectionTask.ContinueWith((Task parent) =>
            {
               if (parent.IsFaulted && parent.Exception != null)
               {
                  log.Error("Connection task is failed with exception: ", parent.Exception.InnerException);
               }
            });
         }
         catch (Exception exc)
         {
            log.Fatal(exc);
         }
      }

      protected override void OnStop()
      {
         int received = 0;
         int completed = 0;
         try
         {
            connectionTask.Wait(config.GetWaitConnectionStopTimeout());

            if (subscriber != null)
            {
               subscriber.StopAcceptingMessages();
            }

            if (jobDispatcher != null)
            {
               jobDispatcher.CompleteAndCommitJobs(config.GetWaitConnectionStopTimeout());
               received = jobDispatcher.ReceivedJobsCount;
               completed = jobDispatcher.CompletedJobsCount;
            }
         }
         catch (Exception exc)
         {
            log.Fatal(exc);
         }
         finally
         {
            if (subscriber != null)
            {
               subscriber.Dispose();
            }
         }

         log.Info(string.Format(
            "Service stopped at {0}. Jobs received: {1}, completed: {2}",
            DateTime.Now.ToString("[hh:mm:ss.fff]"), 
            received, 
            completed));
      }
   }
}
