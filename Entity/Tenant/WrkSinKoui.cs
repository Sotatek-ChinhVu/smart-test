using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "wrk_sin_koui")]
    public class WrkSinKoui : EmrCloneable<WrkSinKoui>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("wrk_sin_koui_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        //[Index("wrk_sin_koui_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        //[Index("wrk_sin_koui_idx01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        
        [Column("hoken_kbn", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        [Column("hoken_pid")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        [Column("hoken_id")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId { get; set; }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        [Column("syukei_saki")]
        [MaxLength(4)]
        public string? SyukeiSaki { get; set; } = string.Empty;

        /// <summary>
        /// 0: 1～12以外の診療行為 
        /// 1: 血液化学検査の包括項目 
        /// 2: 内分泌学的検査の包括項目 
        /// 3: 肝炎ウイルス関連検査の包括項目 
        /// 5: 腫瘍マーカーの包括項目 
        /// 6: 出血・凝固検査の包括項目 
        /// 7: 自己抗体検査の包括項目 
        /// 8: 内分泌負荷試験の包括項目 
        /// 9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        /// 10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        /// 11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        /// 12: 悪性腫瘍遺伝子検査の包括項目
        /// </summary>
        [Column("hokatu_kensa")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKensa { get; set; }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        [Column("count")]
        public int Count { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("is_nodsp_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("is_nodsp_paper_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspPaperRece { get; set; }

        /// <summary>
        /// 院外処方区分
        /// 1:院外処方
        /// </summary>
        [Column("inout_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        [Column("cd_kbn")]
        [MaxLength(2)]
        public string? CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// レコード期別
        /// 代表項目のレコード識別
        /// </summary>
        [Column("rec_id")]
        [MaxLength(2)]
        public string? RecId { get; set; } = string.Empty;

        /// <summary>
        /// 自費種別
        /// 代表項目のJIHI_SBT_MST.JIHI_SBT
        /// </summary>
        [Column("jihi_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiSbt { get; set; }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        [Column("kazei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        /// 更新者ID
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
    }
}
