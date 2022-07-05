using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "WRK_SIN_KOUI")]
    public class WrkSinKoui : EmrCloneable<WrkSinKoui>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("WRK_SIN_KOUI_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        //[Index("WRK_SIN_KOUI_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        //[Index("WRK_SIN_KOUI_IDX01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        [Key]
        [Column("HOKEN_KBN", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        [Key]
        [Column("RP_NO", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 5)]
        public int SeqNo { get; set; }
        
        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        [Column("HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        [Column("HOKEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId { get; set; }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        [Column("SYUKEI_SAKI")]
        [MaxLength(4)]
        public string SyukeiSaki { get; set; }

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
        [Column("HOKATU_KENSA")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKensa { get; set; }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        [Column("COUNT")]
        public int Count { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("IS_NODSP_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("IS_NODSP_PAPER_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspPaperRece { get; set; }

        /// <summary>
        /// 院外処方区分
        /// 1:院外処方
        /// </summary>
        [Column("INOUT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        [Column("CD_KBN")]
        [MaxLength(2)]
        public string CdKbn { get; set; }

        /// <summary>
        /// レコード期別
        /// 代表項目のレコード識別
        /// </summary>
        [Column("REC_ID")]
        [MaxLength(2)]
        public string RecId { get; set; }

        /// <summary>
        /// 自費種別
        /// 代表項目のJIHI_SBT_MST.JIHI_SBT
        /// </summary>
        [Column("JIHI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiSbt { get; set; }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        [Column("KAZEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }

    }
}
