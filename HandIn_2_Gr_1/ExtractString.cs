using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandIn_2_Gr_1
{
    public class ExtractString
    {
        public static string extractXconst(string inputstring)
        {

            string[] parts = inputstring.Split( new string[] { "%2F"}, StringSplitOptions.None);

            return parts[parts.Length - 1];
        }
    }
}
