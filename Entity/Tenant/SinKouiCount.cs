using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SIN_KOUI_COUNT")]
    public class SinKouiCount : EmrCloneable<SinKouiCount>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("SIN_KOUI_COUNT_IDX01", 1)]
        //[Index("SIN_KOUI_COUNT_IDX02", 1)]
        //[Index("SIN_KOUI_COUNT_IDX03", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
        //[Index("SIN_KOUI_COUNT_IDX01", 2)]
        //[Index("SIN_KOUI_COUNT_IDX02", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Key]
        [Column("SIN_YM", Order = 3)]
        //[Index("SIN_KOUI_COUNT_IDX01", 3)]
        //[Index("SIN_KOUI_COUNT_IDX03", 2)]
        public int SinYm { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Key]
        [Column("SIN_DAY", Order = 4)]
        //[Index("SIN_KOUI_COUNT_IDX01", 4)]
        public int SinDay { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        //[Index("SIN_KOUI_COUNT_IDX02", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 5)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        [Key]
        [Column("RP_NO", Order = 6)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 7)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        [Column("COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int Count { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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

