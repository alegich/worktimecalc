using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using timecalclib;

namespace timecalclibtest
{
   class TimeReporterSpy : TimeReporter
   {
      public void AddRecord(DateTime time, string action)
      {
         GetRecords().Add(new KeyValuePair<DateTime, string>(time, action));
      }
   }

   [TestClass]
   public class TimeReporterTest
   {
      [TestMethod]
      public void TestLunchDuration()
      {
         TimeReporterSpy reporter = new TimeReporterSpy();
         reporter.AddRecord(new DateTime(2017, 10, 03, 08, 00, 00, DateTimeKind.Utc), timecalclib.Action.Unlock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 09, 15, 00, DateTimeKind.Utc), timecalclib.Action.Lock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 09, 30, 00, DateTimeKind.Utc), timecalclib.Action.Unlock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 11, 00, 00, DateTimeKind.Utc), timecalclib.Action.Lock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 11, 14, 00, DateTimeKind.Utc), timecalclib.Action.Unlock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 12, 50, 00, DateTimeKind.Utc), timecalclib.Action.Lock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 13, 22, 00, DateTimeKind.Utc), timecalclib.Action.Unlock);
         reporter.AddRecord(new DateTime(2017, 10, 03, 16, 00, 00, DateTimeKind.Utc), timecalclib.Action.Lock);

         Assert.AreEqual(new TimeSpan(0, 0, 32, 0), reporter.LunchDuration());
         Assert.AreEqual(new TimeSpan(0, 8, 0, 0), reporter.WorkDuration());
         Assert.AreEqual(new TimeSpan(0, 8, 0, 0), reporter.WholeDayTime());
         Assert.AreEqual(new TimeSpan(0, 1, 1, 0), reporter.AwayDuration());
      }

      [TestMethod]
      public void TestDayOff()
      {
         TimeReporterSpy reporter = new TimeReporterSpy();

         Assert.AreEqual(new TimeSpan(0, 8, 0, 0), reporter.WholeDayTime());
      }
   }
}
