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

         GetRecords().AddRange(ConvertFromStrings(content));
      }

      private readonly DataFormatter formatter = new DataFormatter();
   }
}
