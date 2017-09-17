using System;
using timecalcfile;
using timecalclib;

namespace timecalcout
{
   class Program
   {
      static void Main(string[] args)
      {
         DateTime date = args.Length > 0 ? (new DataFormatter()).ParseDate(args[0]) : DateTime.Now.Date;

         timecalcfile.TimeCalcFactory factory = new timecalcfile.TimeCalcFactory();
         Reportable reporter = factory.CreateReporter(date);
         Console.WriteLine("Work started: {0}", reporter.WorkStarted().ToLocalTime());
         Console.WriteLine("Work ended: {0}", reporter.WorkEnded().ToLocalTime());
         foreach (var timePair in reporter.AwaySessions())
         {
            Console.WriteLine("Away: {0} - {1}", timePair.Key.ToLocalTime(), timePair.Value.ToLocalTime());
         }
         Console.WriteLine("Whole time spent: {0}", reporter.WholeDayTime());
         Console.WriteLine("Work time: {0}", reporter.WorkDuration());
         Console.WriteLine("Away time: {0}", reporter.AwayDuration());

         WeekReportable week = factory.CreateWeekReporter(date);

         Console.WriteLine("Time left: {0}", week.TimeLeft());

         Console.WriteLine("To show information for other day: timecalcout yyyy.MM.dd");
      }
   }
}
