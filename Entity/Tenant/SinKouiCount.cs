using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sin_koui_count")]
    public class SinKouiCount : EmrCloneable<SinKouiCount>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("sin_koui_count_idx01", 1)]
        //[Index("sin_koui_count_idx02", 1)]
        //[Index("sin_koui_count_idx03", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        //[Index("sin_koui_count_idx01", 2)]
        //[Index("sin_koui_count_idx02", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 3)]
        //[Index("sin_koui_count_idx01", 3)]
        //[Index("sin_koui_count_idx03", 2)]
        public int SinYm { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_day", Order = 4)]
        //[Index("sin_koui_count_idx01", 4)]
        public int SinDay { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        //[Index("sin_koui_count_idx02", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 5)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 6)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        
        [Column("seq_no", Order = 7)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 回数
        /// 来院ごとの回数
        /// </summary>
        [Column("count")]
        [CustomAttribute.DefaultValue(0)]
        public int Count { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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

