using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyShare.Controllers
{
    public static class Utils
    {
        public static bool ContainValidPhoneNumber(string HomePhoneNumber, string WorkPhoneNumber, string MobilePhoneNumber)
        {
            if (string.IsNullOrEmpty(HomePhoneNumber) && string.IsNullOrEmpty(WorkPhoneNumber) && string.IsNullOrEmpty(MobilePhoneNumber))
                return false;
            else
                return true;
        }
    }
}
