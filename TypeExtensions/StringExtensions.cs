using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrinitySharp
{
    public static partial class StringExtensions
    {
        public static bool IsBase64String(this string stringIn)
        {
            if (stringIn == null) { throw new ArgumentNullException(nameof(stringIn)); }

            stringIn = stringIn.Trim();
            return (stringIn.Length % 4 == 0) && Regex.IsMatch(stringIn, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
