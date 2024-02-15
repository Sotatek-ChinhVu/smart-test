using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sokatu_mst")]
    public class SokatuMst : EmrCloneable<SokatuMst>
    {
        /// <summary>
        /// 都道府県番号
        /// 
        /// </summary>

        [Column("pref_no")]
        public int PrefNo { get; set; }

        /// <summary>
        /// 開始年月
        /// 
        /// </summary>

        [Column("start_ym")]
        public int StartYm { get; set; }

        /// <summary>
        /// 終了年月
        /// 
        /// </summary>
        [Column("end_ym")]
        [CustomAttribute.DefaultValue(999999)]
        public int EndYm { get; set; }

        /// <summary>
        /// 帳票ID
        /// 1:レセプト
        ///                     2:光ディスク等送付書          
        ///                     3:症状詳記          
        ///                     4:アフターケア委託費請求書          
        ///                     5:複写レセプト          
        ///                     6:小児ぜん息レセプト          
        ///                     10:レセプト一覧表          
        ///                     101:社保総括表          
        ///                     102:国保総括表          
        ///                     103:国保請求書          
        ///                     104:後期高齢請求書          
        ///                     105:福祉請求書          
        ///                     106:福祉電子媒体請求          
        /// </summary>

        [Column("report_id")]
        public int ReportId { get; set; }

        /// <summary>
        /// 帳票枝番
        /// 帳票ID枝番(都道府県ごとに異なる)
        /// </summary>

        [Column("report_eda_no")]
        public int ReportEdaNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 帳票名
        /// 
        /// </summary>
        [Column("report_name")]
        [MaxLength(30)]
        public string? ReportName { get; set; } = string.Empty;

        /// <summary>
        /// 印刷タイプ
        /// 0:印刷 1:データ出力
        /// </summary>
        [Column("print_type")]
        [CustomAttribute.DefaultValue(0)]
        public int PrintType { get; set; }

        /// <summary>
        /// 印刷番号指定タイプ
        /// 0:不可 1:患者番号 2:保険者番号
        /// </summary>
        [Column("print_no_type")]
        [CustomAttribute.DefaultValue(0)]
        public int PrintNoType { get; set; }

        /// <summary>
        /// 対象レセすべて
        /// 0:未使用 1:使用
        /// </summary>
        [Column("data_all")]
        [CustomAttribute.DefaultValue(0)]
        public int DataAll { get; set; }

        /// <summary>
        /// 対象レセ電子請求
        /// 0:未使用 1:使用
        /// </summary>
        [Column("data_disk")]
        [CustomAttribute.DefaultValue(0)]
        public int DataDisk { get; set; }

        /// <summary>
        /// 対象レセ紙請求
        /// 0:未使用 1:使用
        /// </summary>
        [Column("data_paper")]
        [CustomAttribute.DefaultValue(0)]
        public int DataPaper { get; set; }

        /// <summary>
        /// 対象レセ区分
        /// 0:すべて 1:電子請求 2:紙請求
        /// </summary>
        [Column("data_kbn")]
        [CustomAttribute.DefaultValue(1)]
        public int DataKbn { get; set; }

        /// <summary>
        /// 媒体種類
        /// 0 0:非表示 1:表示
        ///                     1桁目:FD 2桁目:MO 3桁目:CD 4桁目:ｵﾝﾗｲﾝ          
        /// </summary>
        [Column("disk_kind")]
        [MaxLength(10)]
        public string? DiskKind { get; set; } = string.Empty;

        /// <summary>
        /// 媒体枚数
        /// 0:非表示 1:表示
        /// </summary>
        [Column("disk_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int DiskCnt { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 印刷順指定
        /// 0:未使用 1:使用
        /// </summary>
        [Column("is_sort")]
        [CustomAttribute.DefaultValue(0)]
        public int IsSort { get; set; }
    }
}
