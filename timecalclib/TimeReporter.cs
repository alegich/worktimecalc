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
         TimeSpan nonWorkingTime = NonWorkingDuration();
         TimeSpan workTime = wholeDayTime.Subtract(nonWorkingTime);
         return workTime;
      }

      public TimeSpan AwayDuration()
      {
         return SessionsSumInterval(AwaySessions());
      }

      protected TimeSpan NonWorkingDuration()
      {
         return
            SessionsSumInterval(
               AwaySessions().Where(s => Interval(s) != LunchDuration() && Interval(s) > new TimeSpan(0, 1, 0, 0)).ToList());
      }

      public long WorkMillis()
      {
         return (long)WorkDuration().TotalMilliseconds;
      }

      public long AwayMillis()
      {
         return (long)AwayDuration().TotalMilliseconds;
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
         return Lunch().Key;
      }

      public DateTime LunchEnded()
      {
         return Lunch().Value;
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

         TimeSpan retVal = lastAction.Subtract(firstAction);

         return retVal != TimeSpan.Zero ? retVal : TimeSpan.FromHours(8);
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

      protected static List<KeyValuePair<DateTime, string>> ConvertFromStrings(List<string> source)
      {
         var retVal = new List<KeyValuePair<DateTime, string>>();

         DataFormatter formatter = new DataFormatter();

         foreach (string line in source)
         {
            int separator = line.LastIndexOf(" ", StringComparison.Ordinal);
            string timePart = line.Substring(0, separator);
            string action = line.Substring(separator).Trim();
            DateTime time = formatter.ParseDateTime(timePart);
            retVal.Add(new KeyValuePair<DateTime, string>(time, action));
         }

         return retVal;
      }

      protected KeyValuePair<DateTime, DateTime> Lunch()
      {
         TimeSpan minSpan = TimeSpan.MaxValue;
         KeyValuePair<DateTime, DateTime> retVal = new KeyValuePair<DateTime, DateTime>(DateTime.MinValue, DateTime.MinValue);
         foreach (var session in AwaySessions())
         {
            TimeSpan interval = Interval(session);
            TimeSpan currentSpan = interval.Subtract(new TimeSpan(0, 0, 30, 0)).Duration();
            if (currentSpan < minSpan)
            {
               minSpan = currentSpan;
               retVal = session;
            }
         }

         if (Interval(retVal) < new TimeSpan(0, 0, 15, 0) || Interval(retVal) > new TimeSpan(0, 1, 0, 0))
         {
            retVal = new KeyValuePair<DateTime, DateTime>(DateTime.MinValue, DateTime.MinValue);
         }

         return retVal;
      }

      public TimeSpan LunchDuration()
      {
         return Interval(Lunch());
      }

      private readonly List<KeyValuePair<DateTime, string>> records = new List<KeyValuePair<DateTime, string>>();
   }
}
