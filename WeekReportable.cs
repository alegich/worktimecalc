﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace timecalclib
{
   public interface WeekReportable
   {
      TimeSpan WholeWeekTime();
      TimeSpan TimeLeft();
   }
}
