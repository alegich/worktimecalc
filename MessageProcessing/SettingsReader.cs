using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MessageProcessing
{
   public class SettingsReader
   {
      public string ReadAppSetting(string name, string defaultValue)
      {
         string value = ConfigurationManager.AppSettings[name];
         if (string.IsNullOrWhiteSpace(value))
         {
            value = defaultValue;
         }

         return value;
      }

      public int ReadAppSetting(string name, int defaultValue)
      {
         int retVal = defaultValue;
         int.TryParse(ReadAppSetting(name, retVal.ToString()), out retVal);
         return retVal;
      }
   }
}
