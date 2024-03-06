using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sin_rp_inf")]
    public class SinRpInf : EmrCloneable<SinRpInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("sin_rp_inf_idx01", 1)]
        //[Index("sin_rp_inf_idx02", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        //[Index("sin_rp_inf_idx01", 2)]
        //[Index("sin_rp_inf_idx02", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 3)]
        //[Index("sin_rp_inf_idx01", 3)]
        //[Index("sin_rp_inf_idx02", 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 剤番号
        /// 
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RpNo { get; set; }

        /// <summary>
        /// 初回算定日
        /// 
        /// </summary>
        [Column("first_day")]
        [CustomAttribute.DefaultValue(0)]
        public int FirstDay { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        [Column("hoken_kbn")]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 診療行為区分
        /// 
        /// </summary>
        [Column("sin_koui_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SinKouiKbn { get; set; }

        /// <summary>
        /// 診療識別
        /// レセプト電算に記録する診療識別
        /// </summary>
        [Column("sin_id")]
        [CustomAttribute.DefaultValue(0)]
        public int SinId { get; set; }

        /// <summary>
        /// 代表コード表用番号
        /// 
        /// </summary>
        [Column("cd_no")]
        [MaxLength(15)]
        public string? CdNo { get; set; } = string.Empty;

        /// <summary>
        /// 算定区分
        /// 2:自費算定
        /// </summary>
        [Column("santei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 診療行為データ
        /// RP_NOに属するSIN_KOUI.DETAIL_DATAを結合したもの　※
        /// </summary>
        [Column("koui_data")]
        public string? KouiData { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("sin_rp_inf_idx02", 4)]
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

        /// <summary>
        /// EF対象フラグ
        ///     1:EFファイル出力対象の削除項目   
        /// </summary>
        [Column("ef_flg")]
        [CustomAttribute.DefaultValue(0)]
        public int EfFlg { get; set; }
    }
}
