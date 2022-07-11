using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "WRK_SIN_RP_INF")]
    public class WrkSinRpInf : EmrCloneable<WrkSinRpInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("WRK_SIN_RP_INF_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        //[Index("WRK_SIN_RP_INF_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        //[Index("WRK_SIN_RP_INF_IDX01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        //[Key]
        [Column("HOKEN_KBN", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        //[Key]
        [Column("RP_NO", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 診療行為区分
        /// 
        /// </summary>
        [Column("SIN_KOUI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SinKouiKbn { get; set; }

        /// <summary>
        /// 診療識別
        /// レセプト電算に記録する診療識別
        /// </summary>
        [Column("SIN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int SinId { get; set; }

        /// <summary>
        /// 代表コード表用番号
        /// </summary>
        [Column("CD_NO")]
        [MaxLength(15)]
        public string CdNo { get; set; }

        /// <summary>
        /// 算定区分
        /// 1:自費算定
        /// </summary>
        [Column("SANTEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

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
