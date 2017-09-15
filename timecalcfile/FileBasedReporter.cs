using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using timecalclib;

namespace timecalcfile
{
   public class FileBasedReporter: TimeReporter
   {
      public FileBasedReporter(string dataFolder, DateTime day)
      {
         string fileName = dataFolder + @"\" + formatter.FormatDate(day);
         List<string> content = File.Exists(fileName) ? File.ReadAllLines(fileName).ToList() : new List<string>();

         foreach (var line in content)
         {
            int separator = line.LastIndexOf(" ", StringComparison.Ordinal);
            string timePart = line.Substring(0, separator);
            string action = line.Substring(separator).Trim();
            DateTime time = formatter.ParseDateTime(timePart);
            GetRecords().Add(new KeyValuePair<DateTime, string>(time, action));
         }
      }

      private readonly DataFormatter formatter = new DataFormatter();
   }
}
