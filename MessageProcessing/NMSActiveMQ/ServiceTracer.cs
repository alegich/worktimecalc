using Apache.NMS;
using log4net;

namespace MessageProcessing.NMSActiveMQ
{
   /** Wrapper for using in Apache.NMS.Trace class
    */
   internal class ServiceTracer : ITrace
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ServiceTracer));

      public bool IsDebugEnabled
      {
         get
         {
            return true;
         }
      }

      public bool IsErrorEnabled
      {
         get
         {
            return true;
         }
      }

      public bool IsFatalEnabled
      {
         get
         {
            return true;
         }
      }

      public bool IsInfoEnabled
      {
         get
         {
            return true;
         }
      }

      public bool IsWarnEnabled
      {
         get
         {
            return true;
         }
      }

      public void Debug(string message)
      {
         log.Debug(message);
      }

      public void Error(string message)
      {
         log.Error(message);
      }

      public void Fatal(string message)
      {
         log.Fatal(message);
      }

      public void Info(string message)
      {
         log.Info(message);
      }

      public void Warn(string message)
      {
         log.Warn(message);
      }
   }
}
