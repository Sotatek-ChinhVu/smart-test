using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RECE_INF_EDIT")]
    public class ReceInfEdit : EmrCloneable<ReceInfEdit>
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
        /// 請求年月
        /// 
        /// </summary>
        [Key]
        [Column("SEIKYU_YM", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        [Key]
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        [Key]
        [Column("HOKEN_ID", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// レセプト種別
        /// 11x2: 本人
        ///                     11x4: 未就学者          
        ///                     11x6: 家族          
        ///                     11x8: 高齢一般・低所          
        ///                     11x0: 高齢７割          
        ///                     12x2: 公費          
        ///                     13x8: 後期一般・低所          
        ///                     13x0: 後期７割          
        ///                     14x2: 退職本人          
        ///                     14x4: 退職未就学者          
        ///                     14x6: 退職家族          
        /// </summary>
        [Column("RECE_SBT")]
        [MaxLength(4)]
        public string ReceSbt { get; set; }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        [Column("HOUBETU")]
        [MaxLength(3)]
        public string Houbetu { get; set; }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        [Column("KOHI1_HOUBETU")]
        [MaxLength(3)]
        public string Kohi1Houbetu { get; set; }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        [Column("KOHI2_HOUBETU")]
        [MaxLength(3)]
        public string Kohi2Houbetu { get; set; }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        [Column("KOHI3_HOUBETU")]
        [MaxLength(3)]
        public string Kohi3Houbetu { get; set; }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        [Column("KOHI4_HOUBETU")]
        [MaxLength(3)]
        public string Kohi4Houbetu { get; set; }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        [Column("HOKEN_RECE_TENSU")]
        public int? HokenReceTensu { get; set; }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        [Column("HOKEN_RECE_FUTAN")]
        public int? HokenReceFutan { get; set; }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        [Column("KOHI1_RECE_TENSU")]
        public int? Kohi1ReceTensu { get; set; }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        [Column("KOHI1_RECE_FUTAN")]
        public int? Kohi1ReceFutan { get; set; }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI1_RECE_KYUFU")]
        public int? Kohi1ReceKyufu { get; set; }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        [Column("KOHI2_RECE_TENSU")]
        public int? Kohi2ReceTensu { get; set; }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        [Column("KOHI2_RECE_FUTAN")]
        public int? Kohi2ReceFutan { get; set; }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI2_RECE_KYUFU")]
        public int? Kohi2ReceKyufu { get; set; }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        [Column("KOHI3_RECE_TENSU")]
        public int? Kohi3ReceTensu { get; set; }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        [Column("KOHI3_RECE_FUTAN")]
        public int? Kohi3ReceFutan { get; set; }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI3_RECE_KYUFU")]
        public int? Kohi3ReceKyufu { get; set; }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        [Column("KOHI4_RECE_TENSU")]
        public int? Kohi4ReceTensu { get; set; }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        [Column("KOHI4_RECE_FUTAN")]
        public int? Kohi4ReceFutan { get; set; }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI4_RECE_KYUFU")]
        public int? Kohi4ReceKyufu { get; set; }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        [Column("HOKEN_NISSU")]
        public int? HokenNissu { get; set; }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        [Column("KOHI1_NISSU")]
        public int? Kohi1Nissu { get; set; }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        [Column("KOHI2_NISSU")]
        public int? Kohi2Nissu { get; set; }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        [Column("KOHI3_NISSU")]
        public int? Kohi3Nissu { get; set; }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        [Column("KOHI4_NISSU")]
        public int? Kohi4Nissu { get; set; }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        [Column("TOKKI")]
        [MaxLength(10)]
        public string Tokki { get; set; }

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        [Column("TOKKI1")]
        [MaxLength(10)]
        public string Tokki1 { get; set; }

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        [Column("TOKKI2")]
        [MaxLength(10)]
        public string Tokki2 { get; set; }

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        [Column("TOKKI3")]
        [MaxLength(10)]
        public string Tokki3 { get; set; }

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        [Column("TOKKI4")]
        [MaxLength(10)]
        public string Tokki4 { get; set; }

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        [Column("TOKKI5")]
        [MaxLength(10)]
        public string Tokki5 { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        public string UpdateMachine { get; set; }

    }
}
