using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public class TimeReporter: Reportable
   {
      public TimeSpan WorkDuration()
      {
         TimeSpan wholeDayTime = WholeDayTime();
         TimeSpan sessionsSum = SessionsSumInterval(AwaySessions());
         TimeSpan workTime = wholeDayTime.Subtract(sessionsSum);
         return workTime;
      }

      public TimeSpan AwayDuration()
      {
         return SessionsSumInterval(AwaySessions());
      }

      public long WorkMillis()
      {
         return (long)WorkDuration().TotalMilliseconds;
      }

      public long AwayMillis()
      {
         return (long)AwayDuration().TotalMilliseconds;
      }

      public long LunchMillis()
      {
         throw new NotImplementedException();
      }

      public DateTime WorkEnded()
      {
         KeyValuePair<DateTime, string> lastRecord = records.Count > 0
            ? records[records.Count - 1]
            : new KeyValuePair<DateTime, string>(DateTime.MinValue, Action.Stop);

         if (!Action.IsAwayAction(lastRecord.Value))
         {
            lastRecord = new KeyValuePair<DateTime, string>(DateTime.UtcNow, Action.Stop);
         }

         return lastRecord.Key;
      }

      public DateTime WorkStarted()
      {
         return records.Count > 0
            ? records[0].Key
            : DateTime.MinValue;
      }

      public DateTime LunchStarted()
      {
         throw new NotImplementedException();
      }

      public DateTime LunchEnded()
      {
         throw new NotImplementedException();
      }

      public List<KeyValuePair<DateTime, DateTime>> AwaySessions()
      {
         List<KeyValuePair<DateTime, DateTime>> retVal = new List<KeyValuePair<DateTime, DateTime>>();
         Stack<KeyValuePair<DateTime, string>> events = new Stack<KeyValuePair<DateTime, string>>(records.Count);
         foreach (var record in records)
         {
            if (Action.Lock.Equals(record.Value) || Action.Stop.Equals(record.Value))
            {
               // away started
               events.Push(record);
            }
            else if (events.Any() && (Action.Start.Equals(record.Value) || Action.Unlock.Equals(record.Value)))
            {
               // away stopped
               KeyValuePair<DateTime, string> awayStart = events.Pop();
               retVal.Add(new KeyValuePair<DateTime, DateTime>(awayStart.Key, record.Key));
            }
         }

         return retVal;
      }

      public TimeSpan WholeDayTime()
      {
         DateTime firstAction = WorkStarted();

         DateTime lastAction = WorkEnded();

         return lastAction.Subtract(firstAction);
      }

      private TimeSpan Interval(KeyValuePair<DateTime, DateTime> dates)
      {
         TimeSpan retVal = dates.Value.Subtract(dates.Key);
         return retVal;
      }

      private TimeSpan SessionsSumInterval(List<KeyValuePair<DateTime, DateTime>> sessions)
      {
         TimeSpan timeInterval = new TimeSpan();
         foreach (var session in sessions)
         {
            TimeSpan sessionInterval = Interval(session);
            timeInterval = timeInterval.Add(sessionInterval);
         }
         return timeInterval;
      }

      protected List<KeyValuePair<DateTime, string>> GetRecords()
      {
         return records;
      }

      private readonly List<KeyValuePair<DateTime, string>> records = new List<KeyValuePair<DateTime, string>>();
   }
}
