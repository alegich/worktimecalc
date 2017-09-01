using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public class DataFormatter
   {
      public string FormatDateTime(DateTime dt)
      {
         return dt.ToString(CultureInfo.InvariantCulture);
      }

      public DateTime ParseDateTime(string dt)
      {
         return DateTime.Parse(dt, CultureInfo.InvariantCulture);
      }

      public string FormatDate(DateTime date)
      {
         return date.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture);
      }

      public DateTime ParseDate(string date)
      {
         return DateTime.ParseExact(date, "yyyy.MM.dd", CultureInfo.InvariantCulture);
      }
}
}
