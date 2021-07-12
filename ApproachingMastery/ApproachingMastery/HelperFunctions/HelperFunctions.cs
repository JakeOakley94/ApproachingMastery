using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApproachingMastery
{
    public static class HelperFunctions
    {
        public static string GetClientIPAddress(HttpRequestBase request)
        {
            string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }
    }
}