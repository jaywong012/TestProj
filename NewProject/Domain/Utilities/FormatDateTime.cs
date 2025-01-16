using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utilities
{
    public static class FormatDateTime
    {
        public static string ToViewAbleDateTime(DateTime time)
        {
            return time.ToString("HH:mm MMM dd");
        } 
    }
}
