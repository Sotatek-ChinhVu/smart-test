using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSSDK.Constants
{
    public static class ConfigConstant
    {
        public static string HostedZoneId = "Z09462362PXK5JFYQ59B";
        public static string Domain = "smartkarte.org";
        public static string DistributionId = "E1Q6ZVLBFAFBDX";
        public static string DedicateInstance = "db.m6g.large";
        public static string SharingInstance = "db.m6g.xlarge";
        public static int TimeoutCheckingAvailable = 15;
        public static int TypeSharing = 0;
        public static int TypeDedicate = 1;
        public static int SizeTypeMB = 1;
        public static int SizeTypeGB = 2;
        public static List<string> LISTSYSTEMDB = new List<string>() { "rdsadmin, postgres" };

        public static Dictionary<string, byte> StatusTenantDictionary()
        {
            Dictionary<string, byte> rdsStatusDictionary = new Dictionary<string, byte>
        {
            {"available", 1},
            {"creating", 2},
            {"modifying", 3},
            {"deleting", 4},
            {"backing-up", 5},
            {"restoring", 6},
            {"failed", 7},
            {"inaccessible-encryption-credentials",8},
            {"storage-full", 9},
            {"restore-failed", 10},
            {"terminating", 11},
            {"terminated", 12},
            {"terminating-failed", 13}
        };

            return rdsStatusDictionary;
        }
    }
}
