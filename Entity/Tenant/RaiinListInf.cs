using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_LIST_INF")]
    public class RaiinListInf : EmrCloneable<RaiinListInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_DATE", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 分類ID
        /// 
        /// </summary>
        //[Key]
        [Column("GRP_ID", Order = 5)]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// 
        /// </summary>
        [Column("KBN_CD")]
        public int KbnCd { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
        public string UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// 来院リスト区分
        ///		1: 行為
        ///		2: 項目
        ///		3: 文書
        ///		4: ファイル
        /// </summary>
        //[Key]
        [Column("RAIIN_LIST_KBN", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int RaiinListKbn { get; set; }
    }
}
