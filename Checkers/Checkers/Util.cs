using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Util
    {
        public static int SetUserName(String uName)
        {
            return 0; // TODO: Save to disk
        }

        public static String GetUserName()
        {
            return "Mike"; // TODO: Read from disk
        }

        public static bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
                return false;

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
                return false;

            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }
    }
}
