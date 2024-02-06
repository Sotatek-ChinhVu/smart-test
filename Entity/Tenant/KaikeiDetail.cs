using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kaikei_detail")]
    public class KaikeiDetail : EmrCloneable<KaikeiDetail>
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
        /// 親来院番号
        /// 
        /// </summary>
        [Column("oya_raiin_no")]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        
        [Column("hoken_pid", Order = 5)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 合算調整Pid
        /// 
        /// </summary>
        
        [Column("adjust_pid", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustPid { get; set; }

        /// <summary>
        /// 合算調整KohiId
        /// 
        /// </summary>
        [Column("adjust_kid")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustKid { get; set; }

        /// <summary>
        /// 保険区分
        /// 
        /// </summary>
        [Column("hoken_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険種別コード
        /// 
        /// </summary>
        [Column("hoken_sbt_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        [Column("hoken_id")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        [Column("kohi1_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        [Column("kohi2_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        [Column("kohi3_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        [Column("kohi4_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 労災保険ID
        /// 
        /// </summary>
        [Column("rousai_id")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiId { get; set; }

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
        /// 公費１優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("kohi1_priority")]
        [MaxLength(8)]
        public string? Kohi1Priority { get; set; } = string.Empty;

        /// <summary>
        /// 公費２優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("kohi2_priority")]
        [MaxLength(8)]
        public string? Kohi2Priority { get; set; } = string.Empty;

        /// <summary>
        /// 公費３優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("kohi3_priority")]
        [MaxLength(8)]
        public string? Kohi3Priority { get; set; } = string.Empty;

        /// <summary>
        /// 公費４優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("kohi4_priority")]
        [MaxLength(8)]
        public string? Kohi4Priority { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        [Column("honke_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 高額療養費区分
        /// PT_HOKEN_INF.KOGAKU_KBN
        /// </summary>
        [Column("kogaku_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKbn { get; set; }

        /// <summary>
        /// 高額療養費適用区分
        /// PT_HOKEN_INF.KOGAKU_TEKIYO_KBN
        /// </summary>
        [Column("kogaku_tekiyo_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyoKbn { get; set; }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        [Column("is_tokurei")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokurei { get; set; }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        [Column("is_tasukai")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTasukai { get; set; }

        /// <summary>
        /// 高額療養費合算対象
        ///     1:合算対象外
        ///     2:21,000未満合算対象
        /// </summary>
        [Column("kogaku_total_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTotalKbn { get; set; }

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        [Column("is_choki")]
        [CustomAttribute.DefaultValue(0)]
        public int IsChoki { get; set; }

        /// <summary>
        /// 高額療養費限度額
        /// 
        /// </summary>
        [Column("kogaku_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuLimit { get; set; }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        [Column("total_kogaku_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalKogakuLimit { get; set; }

        /// <summary>
        /// 国保減免区分
        /// PT_HOKEN_INF.GENMEN_KBN
        /// </summary>
        [Column("genmen_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenKbn { get; set; }

        /// <summary>
        /// 点数単価
        /// PT_HOKEN_INF.EN_TEN
        /// </summary>
        [Column("en_ten")]
        [CustomAttribute.DefaultValue(0)]
        public double EnTen { get; set; }

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
        /// 公１負担限度額
        /// 
        /// </summary>
        [Column("kohi1_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Limit { get; set; }

        /// <summary>
        /// 公１他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("kohi1_other_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1OtherFutan { get; set; }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        [Column("kohi2_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Limit { get; set; }

        /// <summary>
        /// 公２他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("kohi2_other_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2OtherFutan { get; set; }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        [Column("kohi3_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Limit { get; set; }

        /// <summary>
        /// 公３他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("kohi3_other_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3OtherFutan { get; set; }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        [Column("kohi4_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Limit { get; set; }

        /// <summary>
        /// 公４他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("kohi4_other_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4OtherFutan { get; set; }

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
        /// 保険負担額
        /// 
        /// </summary>
        [Column("hoken_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan { get; set; }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        [Column("kogaku_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan { get; set; }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        [Column("kohi1_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan { get; set; }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        [Column("kohi2_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan { get; set; }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        [Column("kohi3_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan { get; set; }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        [Column("kohi4_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan { get; set; }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        [Column("ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan { get; set; }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        [Column("genmen_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku { get; set; }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        [Column("hoken_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan10en { get; set; }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        [Column("kogaku_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan10en { get; set; }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        [Column("kohi1_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan10en { get; set; }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        [Column("kohi2_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan10en { get; set; }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        [Column("kohi3_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan10en { get; set; }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        [Column("kohi4_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan10en { get; set; }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        [Column("ichibu_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan10en { get; set; }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        [Column("genmen_gaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku10en { get; set; }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        [Column("adjust_round")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRound { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("pt_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        [Column("kogaku_over_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuOverKbn { get; set; }

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
        [Column("rece_sbt")]
        [MaxLength(4)]
        public string? ReceSbt { get; set; } = string.Empty;

        /// <summary>
        /// 実日数
        /// 
        /// </summary>
        [Column("jitunisu")]
        [CustomAttribute.DefaultValue(0)]
        public int Jitunisu { get; set; }

        /// <summary>
        /// 労災イ点数
        /// 
        /// </summary>
        [Column("rousai_i_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiITensu { get; set; }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        [Column("rousai_i_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiIFutan { get; set; }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        [Column("rousai_ro_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiRoFutan { get; set; }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        [Column("jibai_i_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiITensu { get; set; }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        [Column("jibai_ro_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiRoTensu { get; set; }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        [Column("jibai_ha_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHaFutan { get; set; }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        [Column("jibai_ni_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiNiFutan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        [Column("jibai_ho_sindan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        [Column("jibai_ho_sindan_count")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindanCount { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        [Column("jibai_he_meisai")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisai { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        [Column("jibai_he_meisai_count")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisaiCount { get; set; }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        [Column("jibai_a_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiAFutan { get; set; }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        [Column("jibai_b_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiBFutan { get; set; }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        [Column("jibai_c_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiCFutan { get; set; }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        [Column("jibai_d_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiDFutan { get; set; }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        [Column("jibai_kenpo_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoTensu { get; set; }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        [Column("jibai_kenpo_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoFutan { get; set; }

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
        /// 患者負担合計額
        /// 
        /// </summary>
        [Column("total_pt_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalPtFutan { get; set; }

        /// <summary>
        /// 計算順番
        ///     診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        [Column("sort_key")]
        [MaxLength(61)]
        public string? SortKey { get; set; } = string.Empty;

        /// <summary>
        /// 妊婦フラグ
        /// 1:対象
        /// </summary>
        [Column("is_ninpu")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNinpu { get; set; }

        /// <summary>
        /// 在医総フラグ
        /// 1:1:在医総管又は在医総
        /// </summary>
        [Column("is_zaiiso")]
        [CustomAttribute.DefaultValue(0)]
        public int IsZaiiso { get; set; }

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
