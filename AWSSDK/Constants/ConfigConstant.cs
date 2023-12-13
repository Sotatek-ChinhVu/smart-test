using Amazon;
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
        public static byte StatusNotiSuccess = 1;
        public static byte StatusNotifailure = 0;
        public static byte StatusNotiInfo = 2;
        public static List<string> LISTSYSTEMDB = new List<string>() { "rdsadmin, postgres" };
        public static string RdsSnapshotBackupTermiante = "Bak-Termiante";
        public static string RdsSnapshotUpgrade = "Upgrade";
        public static string RdsSnapshotBackupRestore = "Bak-Restore";
        public static string ManagedCachingOptimized = "658327ea-f89d-4fab-a63d-7e88639e58f6";
        public static int PgPostDefault = 5432;

        public static string DestinationBucketName = "phuc-test-s3";
        public static string RestoreBucketName = "phuc-test-s3";
        public static string SourceBucketName = "phuc-test-s3-replication";
        public static RegionEndpoint RegionDestination = RegionEndpoint.GetBySystemName("ap-northeast-1");
        public static RegionEndpoint RegionSource = RegionEndpoint.GetBySystemName("ap-southeast-1");
        public static Dictionary<string, byte> StatusTenantDictionary()
        {
            Dictionary<string, byte> rdsStatusDictionary = new Dictionary<string, byte>
        {
            {"available", 1},
            {"creating", 2},
            {"modifying", 3},
            {"deleting", 4},
            {"backing-up", 5},
            {"updating", 6},
            {"failed", 7},
            {"inaccessible-encryption-credentials",8},
            {"storage-full", 9},
            {"update-failed", 10},
            {"terminating", 11},
            {"terminated", 12},
            {"terminate-failed", 13},
            {"stoped", 14},
            {"restoring", 15},
            {"restore-failed", 16},
            {"stopping", 17},
            {"starting", 18}
        };

            return rdsStatusDictionary;
        }
       
    }
}
