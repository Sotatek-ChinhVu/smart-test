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
        public static List<string> LISTSYSTEMDB = new List<string>() { "rdsadmin, postgres" };
        public static string RdsSnapshotBackupTermiante = "Bak-Termiante";
        public static string RdsSnapshotUpgrade = "Upgrade";
        public static string RdsSnapshotBackupRestore = "Bak-Restore";
        public static string ManagedCachingOptimized = "658327ea-f89d-4fab-a63d-7e88639e58f6";
        public static int PgPostDefault = 5432;
        public static Dictionary<string, byte> StatusTenantDictionary()
        {
            Dictionary<string, byte> rdsStatusDictionary = new Dictionary<string, byte>
        {
            {"available", 1},
            {"creating", 2},
            {"modifying", 3},
            {"deleting", 4},
            {"backing-up", 5},
            {"upgrading", 6},
            {"failed", 7},
            {"inaccessible-encryption-credentials",8},
            {"storage-full", 9},
            {"upgrade-failed", 10},
            {"terminating", 11},
            {"terminated", 12},
            {"terminate-failed", 13},
            {"stoped", 14},
            {"restoring", 15},
            {"restore-failed", 16},
            {"stopping", 17}
        };

            return rdsStatusDictionary;
        }

        public static string SqlUserPermission = @"
                    INSERT INTO public.""USER_PERMISSION""
                    (""HP_ID"", ""USER_ID"", ""FUNCTION_CD"", ""PERMISSION"", ""CREATE_DATE"", ""CREATE_ID"", ""CREATE_MACHINE"", ""UPDATE_DATE"", ""UPDATE_ID"", ""UPDATE_MACHINE"")
                    VALUES
                    (1, 1, '99201001', 0, '2021-05-30 01:23:14.067', 0, NULL, '2021-05-30 01:23:14.067', 0, NULL),
                    (1, 1, '99201002', 0, '2021-05-30 01:23:14.067', 0, NULL, '2021-05-30 01:23:14.067', 0, NULL),
                    (1, 1, '99201004', 0, '2021-05-30 01:23:14.068', 0, NULL, '2021-05-30 01:23:14.068', 0, NULL),
                    (1, 1, '99201005', 0, '2021-05-30 01:23:14.068', 0, NULL, '2021-05-30 01:23:14.068', 0, NULL),
                    (1, 1, '99201006', 0, '2021-05-30 01:23:14.068', 0, NULL, '2021-05-30 01:23:14.068', 0, NULL),
                    (1, 1, '99201007', 0, '2021-05-30 01:23:14.069', 0, NULL, '2021-05-30 01:23:14.069', 0, NULL),
                    (1, 1, '99201008', 0, '2021-05-30 01:23:14.069', 0, NULL, '2021-05-30 01:23:14.069', 0, NULL),
                    (1, 1, '99201009', 0, '2021-05-30 01:23:14.069', 0, NULL, '2021-05-30 01:23:14.069', 0, NULL),
                    (1, 1, '99201010', 0, '2021-05-30 01:23:14.070', 0, NULL, '2021-05-30 01:23:14.070', 0, NULL),
                    (1, 1, '99201011', 0, '2021-05-30 01:23:14.070', 0, NULL, '2021-05-30 01:23:14.070', 0, NULL),
                    (1, 1, '99201012', 0, '2021-05-30 01:23:14.071', 0, NULL, '2021-05-30 01:23:14.071', 0, NULL),
                    (1, 1, '99202000', 0, '2021-05-30 01:23:14.071', 0, NULL, '2021-05-30 01:23:14.071', 0, NULL),
                    (1, 1, '99203000', 0, '2021-05-30 01:23:14.072', 0, NULL, '2021-05-30 01:23:14.072', 0, NULL),
                    (1, 1, '99204000', 1, '2021-05-30 01:23:14.072', 0, NULL, '2021-05-30 01:23:14.072', 0, NULL),
                    (1, 1, '99205000', 0, '2021-05-30 01:23:14.072', 0, NULL, '2021-05-30 01:23:14.072', 0, NULL),
                    (1, 1, '99206000', 0, '2021-05-30 01:23:14.073', 0, NULL, '2021-05-30 01:23:14.073', 0, NULL),
                    (1, 1, '97101000', 0, '2021-05-30 01:23:14.075', 0, NULL, '2021-05-30 01:23:14.075', 0, NULL),
                    (1, 1, '97102000', 0, '2021-05-30 01:23:14.076', 0, NULL, '2021-05-30 01:23:14.076', 0, NULL),
                    (1, 1, '97110000', 0, '2021-05-30 01:23:14.076', 0, NULL, '2021-05-30 01:23:14.076', 0, NULL),
                    (1, 1, '97201000', 0, '2021-05-30 01:23:14.077', 0, NULL, '2021-05-30 01:23:14.077', 0, NULL),
                    (1, 1, '97202000', 0, '2021-05-30 01:23:14.078', 0, NULL, '2021-05-30 01:23:14.078', 0, NULL),
                    (1, 1, '97203000', 0, '2021-05-30 01:23:14.079', 0, NULL, '2021-05-30 01:23:14.079', 0, NULL),
                    (1, 1, '97210000', 0, '2021-05-30 01:23:14.080', 0, NULL, '2021-05-30 01:23:14.080', 0, NULL),
                    (1, 1, '97211000', 0, '2021-05-30 01:23:14.081', 0, NULL, '2021-05-30 01:23:14.081', 0, NULL),
                    (1, 1, '97220000', 0, '2021-05-30 01:23:14.081', 0, NULL, '2021-05-30 01:23:14.081', 0, NULL),
                    (1, 1, '97900000', 0, '2021-05-30 01:23:14.082', 0, NULL, '2021-05-30 01:23:14.082', 0, NULL),
                    (1, 1, '99013000', 0, '2021-05-30 01:23:14.065', 0, NULL, '2021-05-30 01:23:14.065', 0, NULL),
                    (1, 1, '99201003', 0, '2021-05-30 01:23:14.067', 0, NULL, '2023-11-22 10:42:59.113', 1, 'CATNROSE'),
                    (1, 1, '97221000', 0, '2023-08-19 22:02:10.821', 2, '', '2023-08-19 22:02:10.864', 2, ''),
                    (1, 1, '97301000', 0, '2023-08-19 22:02:13.483', 2, '', '2023-08-19 22:02:13.483', 2, ''),
                    (1, 1, '97310000', 0, '2023-08-19 22:02:13.486', 2, '', '2023-08-19 22:02:13.486', 2, ''),
                    (1, 1, '97320000', 0, '2023-08-19 22:02:13.488', 2, '', '2023-08-19 22:02:13.488', 2, ''),
                    (1, 1, '97330000', 0, '2023-08-19 22:02:13.490', 2, '', '2023-08-19 22:02:13.490', 2, ''),
                    (1, 1, '97340000', 0, '2023-08-19 22:02:13.492', 2, '', '2023-08-19 22:02:13.492', 2, ''),
                    (1, 1, '97341000', 0, '2023-08-19 22:02:13.494', 2, '', '2023-08-19 22:02:13.494', 2, ''),
                    (1, 1, '97350000', 0, '2023-08-19 22:02:13.496', 2, '', '2023-08-19 22:02:13.496', 2, ''),
                    (1, 1, '97360000', 0, '2023-08-19 22:02:13.498', 2, '', '2023-08-19 22:02:13.498', 2, ''),
                    (1, 1, '97361000', 0, '2023-08-19 22:02:13.501', 2, '', '2023-08-19 22:02:13.501', 2, ''),
                    (1, 1, '97370000', 0, '2023-08-19 22:02:13.503', 2, '', '2023-08-19 22:02:13.503', 2, ''),
                    (1, 1, '97380000', 0, '2023-08-19 22:02:13.507', 2, '', '2023-08-19 22:02:13.507', 2, ''),
                    (1, 1, '97390000', 0, '2023-08-19 22:02:13.509', 2, '', '2023-08-19 22:02:13.509', 2, ''),
                    (1, 1, '99201000', 0, '2023-08-19 22:02:13.511', 2, '', '2023-08-19 22:02:13.511', 2, ''),
                    (1, 1, '99999000', 0, '2023-08-19 22:02:13.513', 2, '', '2023-08-19 22:02:13.513', 2, ''),
                    (1, 1, '02000000', 0, '2021-05-30 01:23:14.065', 0, NULL, '2023-11-22 10:42:59.112', 1, 'CATNROSE'),
                    (1, 1, '99005000', 0, '2021-05-30 01:23:14.084', 0, NULL, '2023-11-22 10:42:59.113', 1, 'CATNROSE'),
                    (1, 1, '99201013', 0, '2021-05-30 01:23:14.071', 0, NULL, '2023-11-22 10:42:59.113', 1, 'CATNROSE'),
                    (1, 1, '02001000', 0, '2023-11-22 10:42:59.114', 1, 'CATNROSE', '2023-11-22 10:42:59.114', 1, 'CATNROSE');";
        public static string SqlUser = "INSERT INTO public.\"USER_MST\" (\"HP_ID\",\r\n\"ID\",\r\n\"USER_ID\",\r\n\"JOB_CD\",\r\n\"MANAGER_KBN\",\r\n\"KA_ID\",\r\n\"KANA_NAME\",\r\n\"NAME\",\r\n\"SNAME\",\r\n\"LOGIN_ID\",\r\n\"LOGIN_PASS\",\r\n\"MAYAKU_LICENSE_NO\",\r\n\"START_DATE\",\r\n\"END_DATE\",\r\n\"SORT_NO\",\r\n\"IS_DELETED\",\r\n\"CREATE_DATE\",\r\n\"CREATE_ID\",\r\n\"CREATE_MACHINE\",\r\n\"UPDATE_DATE\",\r\n\"UPDATE_ID\",\r\n\"UPDATE_MACHINE\",\r\n\"RENKEI_CD1\",\r\n\"DR_NAME\",\r\n\"LOGIN_TYPE\",\r\n\"HPKI_SN\",\r\n\"HPKI_ISSUER_DN\")\r\nVALUES(1, 0, 1001, 1, 7, 1, '', '', '', '{0}', '{1}', '', 0, 99999999, 0, 0, '2004-01-10 00:00:00.000', 0, NULL, '2023-10-10 00:11:18.558', 0, '', '', '', 0, NULL, NULL);";
    }
}
