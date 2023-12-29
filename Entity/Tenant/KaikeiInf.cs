using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KAIKEI_INF")]
    public class KaikeiInf : EmrCloneable<KaikeiInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("SIN_DATE", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("RAIIN_NO", Order = 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険ID
        /// PT_HOKEN_INF.HOKEN_ID
        /// </summary>
        
        [Column("HOKEN_ID", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１ID
        /// 
        /// </summary>
        [Column("KOHI1_ID")]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２ID
        /// 
        /// </summary>
        [Column("KOHI2_ID")]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３ID
        /// 
        /// </summary>
        [Column("KOHI3_ID")]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４ID
        /// 
        /// </summary>
        [Column("KOHI4_ID")]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 保険区分
        ///     0:自費
        ///     1:社保          
        ///     2:国保          
        ///     11:労災(短期給付)          
        ///     12:労災(傷病年金)          
        ///     13:アフターケア          
        ///     14:自賠責          
        /// </summary>
        [Column("HOKEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険種別コード
        ///     0: 下記以外
        ///     左から          
        ///         1桁目 - 1:社保 2:国保 3:後期 4:退職 5:公費          
        ///         2桁目 - 組合せ数          
        ///         3桁目 - 1:単独 2:２併 .. 5:５併          
        ///     例) 社保単独             = 111    
        ///         社保２併(54)         = 122    
        ///         社保２併(マル長+54)  = 132    
        ///         国保単独             = 211    
        ///         国保２併(54)         = 222    
        ///         国保２併(マル長+54)  = 232    
        ///         公費単独(12)         = 511    
        ///         公費２併(21+12)      = 522    
        /// </summary>
        [Column("HOKEN_SBT_CD")]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// レセプト種別
        ///     11x2: 本人
        ///     11x4: 未就学者          
        ///     11x6: 家族          
        ///     11x8: 高齢一般・低所          
        ///     11x0: 高齢７割          
        ///     12x2: 公費          
        ///     13x8: 後期一般・低所          
        ///     13x0: 後期７割          
        ///     14x2: 退職本人          
        ///     14x4: 退職未就学者          
        ///     14x6: 退職家族          
        /// </summary>
        [Column("RECE_SBT")]
        [MaxLength(4)]
        public string? ReceSbt { get; set; } = string.Empty;

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        [Column("HOUBETU")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        [Column("KOHI1_HOUBETU")]
        [MaxLength(3)]
        public string? Kohi1Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        [Column("KOHI2_HOUBETU")]
        [MaxLength(3)]
        public string? Kohi2Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        [Column("KOHI3_HOUBETU")]
        [MaxLength(3)]
        public string? Kohi3Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        [Column("KOHI4_HOUBETU")]
        [MaxLength(3)]
        public string? Kohi4Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        [Column("HONKE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        [Column("HOKEN_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenRate { get; set; }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        [Column("PT_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int PtRate { get; set; }

        /// <summary>
        /// 表示用負担率
        /// 
        /// </summary>
        [Column("DISP_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int DispRate { get; set; }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        [Column("TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Tensu { get; set; }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        [Column("TOTAL_IRYOHI")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIryohi { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("PT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 自費負担額
        /// 
        /// </summary>
        [Column("JIHI_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutan { get; set; }

        /// <summary>
        /// 自費内税
        /// 
        /// </summary>
        [Column("JIHI_TAX")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTax { get; set; }

        /// <summary>
        /// 自費外税
        /// 
        /// </summary>
        [Column("JIHI_OUTTAX")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttax { get; set; }

        /// <summary>
        /// 自費負担額(非課税)
        /// 
        /// </summary>
        [Column("JIHI_FUTAN_TAXFREE")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxfree { get; set; }

        /// <summary>
        /// 自費負担額(内税・通常税率)
        /// 
        /// </summary>
        [Column("JIHI_FUTAN_TAX_NR")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxNr { get; set; }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        [Column("JIHI_FUTAN_TAX_GEN")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxGen { get; set; }

        /// <summary>
        /// 自費負担額(外税・通常税率)
        /// 
        /// </summary>
        [Column("JIHI_FUTAN_OUTTAX_NR")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanOuttaxNr { get; set; }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        [Column("JIHI_FUTAN_OUTTAX_GEN")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanOuttaxGen { get; set; }

        /// <summary>
        /// 自費内税(通常税率)
        /// 
        /// </summary>
        [Column("JIHI_TAX_NR")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTaxNr { get; set; }

        /// <summary>
        /// 自費内税(軽減税率)
        /// 
        /// </summary>
        [Column("JIHI_TAX_GEN")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTaxGen { get; set; }

        /// <summary>
        /// 自費外税(通常税率)
        /// 
        /// </summary>
        [Column("JIHI_OUTTAX_NR")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttaxNr { get; set; }

        /// <summary>
        /// 自費外税(軽減税率)
        /// 
        /// </summary>
        [Column("JIHI_OUTTAX_GEN")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttaxGen { get; set; }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        [Column("ADJUST_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        [Column("ADJUST_ROUND")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRound { get; set; }

        /// <summary>
        /// 患者負担合計額
        /// 
        /// </summary>
        [Column("TOTAL_PT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalPtFutan { get; set; }

        /// <summary>
        /// 調整額設定値
        /// 
        /// </summary>
        [Column("ADJUST_FUTAN_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutanVal { get; set; }

        /// <summary>
        /// 調整額設定範囲
        /// 
        /// </summary>
        [Column("ADJUST_FUTAN_RANGE")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutanRange { get; set; }

        /// <summary>
        /// 調整率設定値
        /// 
        /// </summary>
        [Column("ADJUST_RATE_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRateVal { get; set; }

        /// <summary>
        /// 調整率設定範囲
        /// 
        /// </summary>
        [Column("ADJUST_RATE_RANGE")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRateRange { get; set; }

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
        public string? CreateMachine { get; set; } = string.Empty;

    }
}