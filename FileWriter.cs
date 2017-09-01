using System;
using System.IO;

namespace timecalclib
{
   public class FileWriter: Writable
   {
      public FileWriter(string directory)
      {
         this.directory = directory;
      }
      public void WriteStart(DateTime time)
      {
         File.AppendAllText(GetTodaysFile(), FormatLine(time, Action.Start));
      }

      public void WriteStop(DateTime time)
      {
         File.AppendAllText(GetTodaysFile(), FormatLine(time, Action.Stop));
      }

      public void WriteLock(DateTime time)
      {
         File.AppendAllText(GetTodaysFile(), FormatLine(time, Action.Lock));
      }

      public void WriteUnlock(DateTime time)
      {
         File.AppendAllText(GetTodaysFile(), FormatLine(time, Action.Unlock));
      }

      protected string GetTodaysFile()
      {
         return $"{directory}\\{formatter.FormatDate(DateTime.UtcNow)}";
      }

      protected string FormatLine(DateTime time, string text)
      {
         return $"{formatter.FormatDateTime(time)} {text}\n";
      }

      private readonly string directory;

      private readonly DataFormatter formatter = new DataFormatter();
   }
}
