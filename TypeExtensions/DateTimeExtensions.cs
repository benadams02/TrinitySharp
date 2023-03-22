using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrinitySharp
{
    public static partial class DateTimeExtensions
    {
        public static string ToISO8601String(this DateTime dateTime)
        {
            if (dateTime == null) { throw new ArgumentNullException(nameof(dateTime)); }

            return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}
