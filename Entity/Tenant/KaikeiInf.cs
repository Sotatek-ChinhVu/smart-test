using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kaikei_inf")]
    [Index(nameof(HpId), nameof(RaiinNo), Name = "kaikei_inf_idx01")]
    public class KaikeiInf : EmrCloneable<KaikeiInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>

        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>

        [Column("sin_date", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>

        [Column("raiin_no", Order = 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険ID
        /// PT_HOKEN_INF.HOKEN_ID
        /// </summary>

        [Column("hoken_id", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１ID
        /// 
        /// </summary>
        [Column("kohi1_id")]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２ID
        /// 
        /// </summary>
        [Column("kohi2_id")]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３ID
        /// 
        /// </summary>
        [Column("kohi3_id")]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４ID
        /// 
        /// </summary>
        [Column("kohi4_id")]
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
        [Column("hoken_kbn")]
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
        [Column("hoken_sbt_cd")]
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
        [Column("rece_sbt")]
        [MaxLength(4)]
        public string? ReceSbt { get; set; } = string.Empty;

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        [Column("houbetu")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        [Column("kohi1_houbetu")]
        [MaxLength(3)]
        public string? Kohi1Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        [Column("kohi2_houbetu")]
        [MaxLength(3)]
        public string? Kohi2Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        [Column("kohi3_houbetu")]
        [MaxLength(3)]
        public string? Kohi3Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        [Column("kohi4_houbetu")]
        [MaxLength(3)]
        public string? Kohi4Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        [Column("honke_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        [Column("hoken_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenRate { get; set; }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        [Column("pt_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int PtRate { get; set; }

        /// <summary>
        /// 表示用負担率
        /// 
        /// </summary>
        [Column("disp_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int DispRate { get; set; }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        [Column("tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Tensu { get; set; }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        [Column("total_iryohi")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIryohi { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("pt_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 自費負担額
        /// 
        /// </summary>
        [Column("jihi_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutan { get; set; }

        /// <summary>
        /// 自費内税
        /// 
        /// </summary>
        [Column("jihi_tax")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTax { get; set; }

        /// <summary>
        /// 自費外税
        /// 
        /// </summary>
        [Column("jihi_outtax")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttax { get; set; }

        /// <summary>
        /// 自費負担額(非課税)
        /// 
        /// </summary>
        [Column("jihi_futan_taxfree")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxfree { get; set; }

        /// <summary>
        /// 自費負担額(内税・通常税率)
        /// 
        /// </summary>
        [Column("jihi_futan_tax_nr")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxNr { get; set; }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        [Column("jihi_futan_tax_gen")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanTaxGen { get; set; }

        /// <summary>
        /// 自費負担額(外税・通常税率)
        /// 
        /// </summary>
        [Column("jihi_futan_outtax_nr")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanOuttaxNr { get; set; }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        [Column("jihi_futan_outtax_gen")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiFutanOuttaxGen { get; set; }

        /// <summary>
        /// 自費内税(通常税率)
        /// 
        /// </summary>
        [Column("jihi_tax_nr")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTaxNr { get; set; }

        /// <summary>
        /// 自費内税(軽減税率)
        /// 
        /// </summary>
        [Column("jihi_tax_gen")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiTaxGen { get; set; }

        /// <summary>
        /// 自費外税(通常税率)
        /// 
        /// </summary>
        [Column("jihi_outtax_nr")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttaxNr { get; set; }

        /// <summary>
        /// 自費外税(軽減税率)
        /// 
        /// </summary>
        [Column("jihi_outtax_gen")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiOuttaxGen { get; set; }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        [Column("adjust_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        [Column("adjust_round")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRound { get; set; }

        /// <summary>
        /// 患者負担合計額
        /// 
        /// </summary>
        [Column("total_pt_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalPtFutan { get; set; }

        /// <summary>
        /// 調整額設定値
        /// 
        /// </summary>
        [Column("adjust_futan_val")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutanVal { get; set; }

        /// <summary>
        /// 調整額設定範囲
        /// 
        /// </summary>
        [Column("adjust_futan_range")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutanRange { get; set; }

        /// <summary>
        /// 調整率設定値
        /// 
        /// </summary>
        [Column("adjust_rate_val")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRateVal { get; set; }

        /// <summary>
        /// 調整率設定範囲
        /// 
        /// </summary>
        [Column("adjust_rate_range")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRateRange { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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

    }
}