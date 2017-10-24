using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace MessageProcessing
{
   public class JobDispatcher
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(JobDispatcher));

      private readonly Action commit;
      private List<Task<bool>> jobs = new List<Task<bool>>();
      private readonly int sessionCapacity = 0;
      private int receivedCount = 0;
      private int acceptedCount = 0;

      public JobDispatcher(Action commit, int sessionCapacity = 100)
      {
         this.commit = commit;
         this.sessionCapacity = sessionCapacity;
      }

      public int ReceivedJobsCount
      {
         get
         {
            return receivedCount;
         }
      }

      public int CompletedJobsCount
      {
         get
         {
            return Interlocked.CompareExchange(ref acceptedCount, 0, 0);
         }
      }

      public void DispatchJob(object sender, MessageReceivedEventArgs e)
      {
         Worker worker = e.Worker;
         jobs.Add(Task<bool>.Factory.StartNew(
            w =>
         {
            Worker wrk = w as Worker;
            bool isProcessed = wrk != null && wrk.DoJob();
            if (isProcessed)
            {
               Interlocked.Increment(ref acceptedCount);
            }
            return isProcessed;
         }, worker));

         ++receivedCount;

         log.Debug(string.Format("Jobs count: {0}", jobs.Count));

         while (jobs.Count >= sessionCapacity)
         {
            jobs = jobs.Where(job => !job.IsCompleted).ToList();
            Thread.Sleep(10);
         }
      }

      public void CompleteAndCommitJobs(int waitTime = -1)
      {
         bool completed = Task.WaitAll(jobs.ToArray<Task>(), waitTime);

         log.Debug("All jobs completed");

         if (completed && (commit != null))
         {
            commit.Invoke();
         }
      }
   }
}
