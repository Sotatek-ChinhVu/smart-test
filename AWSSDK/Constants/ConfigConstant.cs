using Amazon;

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
        public static string RdsSnapshotUpdate = "Update";
        public static string RdsSnapshotBackupRestore = "Bak-Restore";
        public static string ManagedCachingOptimized = "658327ea-f89d-4fab-a63d-7e88639e58f6";
        public static int PgPostDefault = 5432;
        public static string PgUserDefault = "postgres";
        public static string PgPasswordDefault = "Emr!23456789";

        public static string DestinationBucketName = "phuc-test-s3";
        public static string RestoreBucketName = "phuc-test-s3";
        public static string SourceBucketName = "phuc-test-s3-replication";
        public static RegionEndpoint RegionDestination = RegionEndpoint.GetBySystemName("ap-northeast-1");
        public static RegionEndpoint RegionSource = RegionEndpoint.GetBySystemName("ap-southeast-1");

        public static byte StatusTenantPending = 1;
        public static byte StatusTenantStopping = 4;
        public static byte StatusTenantRunning = 3;
        public static byte StatusTenantFailded = 2;
        public static byte StatusTenantTeminated = 7;
        public static byte StatusTenantStopped = 5;
        public static byte StatusSuttingDown = 6;
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
            {"starting", 18},
            {"update-schema", 19},
            {"update-schema-failed", 20}
        };

            return rdsStatusDictionary;
        }

        public static readonly List<string> listTableMaster = new List<string>()
        {
            "z_yousiki1_inf_detail",
            "z_syuno_nyukin",
            "z_syouki_inf",
            "z_syobyo_keika",
            "z_summary_inf",
            "z_seikatureki_inf",
            "z_santei_inf_detail",
            "z_rsv_inf",
            "z_rsv_day_comment",
            "z_rece_seikyu",
            "z_rece_inf_edit",
            "z_rece_cmt",
            "z_rece_check_cmt",
            "z_raiin_list_tag",
            "z_raiin_list_cmt",
            "z_raiin_inf",
            "z_pt_tag",
            "z_pt_supple",
            "z_pt_rousai_tenki",
            "z_pt_pregnancy",
            "z_pt_other_drug",
            "z_pt_otc_drug",
            "z_pt_kyusei",
            "z_pt_kohi",
            "z_pt_kio_reki",
            "z_pt_jibkar",
            "z_pt_infection",
            "z_pt_inf",
            "z_pt_hoken_scan",
            "z_pt_hoken_pattern",
            "z_pt_hoken_inf",
            "z_pt_hoken_check",
            "z_pt_family_reki",
            "z_pt_family",
            "z_pt_cmt_inf",
            "z_pt_alrgy_food",
            "z_pt_alrgy_else",
            "z_pt_alrgy_drug",
            "z_monshin_inf",
            "z_limit_list_inf",
            "z_limit_cnt_list_inf",
            "z_kensa_inf_detail",
            "z_kensa_inf",
            "z_filing_inf",
            "z_doc_inf",
            "z_raiin_cmt_inf",
            "z_rece_status",
            "z_todo_inf",
            "z_uketuke_sbt_day_inf",
            "z_yousiki1_inf",
            "yousiki1_inf_detail",
            "yousiki1_inf",
            "yoho_set_mst",
            "yoho_mst",
            "yoho_inf_mst",
            "yoho_hosoku",
            "wrk_sin_rp_inf",
            "wrk_sin_koui_detail_del",
            "wrk_sin_koui_detail",
            "wrk_sin_koui",
            "user_permission",
            "user_mst",
            "user_conf",
            "unit_mst",
            "uketuke_sbt_mst",
            "uketuke_sbt_day_inf",
            "todo_kbn_mst",
            "todo_inf",
            "todo_grp_mst",
            "ten_mst_mother",
            "ten_mst",
            "template_mst",
            "template_menu_mst",
            "template_menu_detail",
            "template_dsp_conf",
            "template_detail",
            "tekiou_byomei_mst_excluded",
            "tekiou_byomei_mst",
            "tag_grp_mst",
            "syuno_seikyu",
            "syuno_nyukin",
            "system_generation_conf",
            "system_conf_menu",
            "system_conf_item",
            "system_conf",
            "syouki_kbn_mst",
            "syouki_inf",
            "syobyo_keika",
            "summary_inf",
            "sta_menu",
            "sta_csv",
            "sta_conf",
            "sin_rp_no_inf",
            "sin_rp_inf",
            "sinreki_filter_mst_detail",
            "sinreki_filter_mst",
            "sin_koui_detail",
            "sin_koui_count",
            "sin_koui",
            "single_dose_mst",
            "set_odr_inf_detail",
            "set_odr_inf_cmt",
            "set_mst",
            "set_kbn_mst",
            "set_karte_inf",
            "set_karte_img_inf",
            "set_generation_mst",
            "set_byomei",
            "session_inf",
            "sentence_list",
            "seikatureki_inf",
            "schema_cmt_mst",
            "santei_inf_detail",
            "santei_inf",
            "santei_grp_mst",
            "santei_grp_detail",
            "santei_cnt_check",
            "santei_auto_order_detail",
            "santei_auto_order",
            "rsv_renkei_inf_tk",
            "rsv_renkei_inf",
            "rsvkrt_odr_inf_detail",
            "rsvkrt_odr_inf_cmt",
            "rsvkrt_odr_inf",
            "rsvkrt_mst",
            "rsvkrt_karte_inf",
            "rsvkrt_karte_img_inf",
            "rsvkrt_byomei",
            "rsv_inf",
            "rsv_grp_mst",
            "rsv_frame_with",
            "rsv_frame_week_ptn",
            "rsv_frame_mst",
            "rsv_frame_inf",
            "rsv_frame_day_ptn",
            "rsv_day_comment",
            "roudou_mst",
            "renkei_timing_mst",
            "renkei_timing_conf",
            "renkei_template_mst",
            "renkei_req",
            "renkei_path_conf",
            "renkei_mst",
            "renkei_conf",
            "releasenote_read",
            "rece_status",
            "rece_seikyu",
            "rece_inf_pre_edit",
            "rece_inf_jd",
            "rece_inf_edit",
            "rece_inf",
            "rece_futan_kbn",
            "receden_rireki_inf",
            "receden_hen_jiyuu",
            "receden_cmt_select",
            "rece_cmt",
            "rece_check_opt",
            "rece_check_err",
            "rece_check_cmt",
            "raiin_list_tag",
            "raiin_list_mst",
            "raiin_list_koui",
            "raiin_list_item",
            "raiin_list_file",
            "raiin_list_doc",
            "raiin_list_detail",
            "raiin_list_cmt",
            "raiin_kbn_yoyaku",
            "raiin_kbn_mst",
            "raiin_kbn_koui",
            "raiin_kbn_inf",
            "raiin_kbn_detail",
            "raiin_kbn_item",
            "raiin_filter_state",
            "raiin_filter_sort",
            "raiin_filter_mst",
            "raiin_filter_kbn",
            "raiin_cmt_inf",
            "pt_tag",
            "pt_supple",
            "pt_rousai_tenki",
            "pt_pregnancy"
        };
    }
}
