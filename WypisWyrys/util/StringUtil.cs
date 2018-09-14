using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanowaniePrzestrzenneExtension.Util
{
    class StringUtil
    {

        public static string ToRtfString(string s)
        {
            try
            {
                StringBuilder returned = new StringBuilder();
                foreach (char c in s)
                {
                    returned.Append(GetRtfEncoding(c));
                }
                return returned.ToString();
            }
            catch
            {
                return null;
            }

            
        }

        public static string GetRtfEncoding(char c)
        {
            if (c == '\\') return "\\\\";
            if (c == '{') return "\\{";
            if (c == '}') return "\\}";
            if (c == '\n') return "\r\n\\line ";
            int intCode = Convert.ToInt32(c);
            if (char.IsLetter(c) && intCode < 0x80)
            {
                return c.ToString();
            }
            return "\\u" + intCode + "?";
        }
    }
}
