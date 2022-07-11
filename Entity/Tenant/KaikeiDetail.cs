using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KAIKEI_DETAIL")]
    public class KaikeiDetail : EmrCloneable<KaikeiDetail>
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
        /// 患者を識別するためのシステム固有の番号
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
        /// 親来院番号
        /// 
        /// </summary>
        [Column("OYA_RAIIN_NO")]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        //[Key]
        [Column("HOKEN_PID", Order = 5)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 合算調整Pid
        /// 
        /// </summary>
        //[Key]
        [Column("ADJUST_PID", Order = 6)]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustPid { get; set; }

        /// <summary>
        /// 合算調整KohiId
        /// 
        /// </summary>
        [Column("ADJUST_KID")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustKid { get; set; }

        /// <summary>
        /// 保険区分
        /// 
        /// </summary>
        [Column("HOKEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険種別コード
        /// 
        /// </summary>
        [Column("HOKEN_SBT_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        [Column("HOKEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        [Column("KOHI1_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        [Column("KOHI2_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        [Column("KOHI3_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        [Column("KOHI4_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 労災保険ID
        /// 
        /// </summary>
        [Column("ROUSAI_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiId { get; set; }

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
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
        /// 公費１優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("KOHI1_PRIORITY")]
        [MaxLength(8)]
        public string Kohi1Priority { get; set; }

        /// <summary>
        /// 公費２優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("KOHI2_PRIORITY")]
        [MaxLength(8)]
        public string Kohi2Priority { get; set; }

        /// <summary>
        /// 公費３優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("KOHI3_PRIORITY")]
        [MaxLength(8)]
        public string Kohi3Priority { get; set; }

        /// <summary>
        /// 公費４優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        [Column("KOHI4_PRIORITY")]
        [MaxLength(8)]
        public string Kohi4Priority { get; set; }

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        [Column("HONKE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 高額療養費区分
        /// PT_HOKEN_INF.KOGAKU_KBN
        /// </summary>
        [Column("KOGAKU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKbn { get; set; }

        /// <summary>
        /// 高額療養費適用区分
        /// PT_HOKEN_INF.KOGAKU_TEKIYO_KBN
        /// </summary>
        [Column("KOGAKU_TEKIYO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyoKbn { get; set; }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        [Column("IS_TOKUREI")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokurei { get; set; }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        [Column("IS_TASUKAI")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTasukai { get; set; }

        /// <summary>
        /// 高額療養費合算対象
        ///     1:合算対象外
        ///     2:21,000未満合算対象
        /// </summary>
        [Column("KOGAKU_TOTAL_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTotalKbn { get; set; }

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        [Column("IS_CHOKI")]
        [CustomAttribute.DefaultValue(0)]
        public int IsChoki { get; set; }

        /// <summary>
        /// 高額療養費限度額
        /// 
        /// </summary>
        [Column("KOGAKU_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuLimit { get; set; }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        [Column("TOTAL_KOGAKU_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalKogakuLimit { get; set; }

        /// <summary>
        /// 国保減免区分
        /// PT_HOKEN_INF.GENMEN_KBN
        /// </summary>
        [Column("GENMEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenKbn { get; set; }

        /// <summary>
        /// 点数単価
        /// PT_HOKEN_INF.EN_TEN
        /// </summary>
        [Column("EN_TEN")]
        [CustomAttribute.DefaultValue(0)]
        public int EnTen { get; set; }

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
        /// 公１負担限度額
        /// 
        /// </summary>
        [Column("KOHI1_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Limit { get; set; }

        /// <summary>
        /// 公１他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("KOHI1_OTHER_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1OtherFutan { get; set; }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        [Column("KOHI2_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Limit { get; set; }

        /// <summary>
        /// 公２他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("KOHI2_OTHER_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2OtherFutan { get; set; }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        [Column("KOHI3_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Limit { get; set; }

        /// <summary>
        /// 公３他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("KOHI3_OTHER_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3OtherFutan { get; set; }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        [Column("KOHI4_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Limit { get; set; }

        /// <summary>
        /// 公４他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        [Column("KOHI4_OTHER_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4OtherFutan { get; set; }

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
        /// 保険負担額
        /// 
        /// </summary>
        [Column("HOKEN_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan { get; set; }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        [Column("KOGAKU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan { get; set; }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        [Column("KOHI1_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan { get; set; }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        [Column("KOHI2_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan { get; set; }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        [Column("KOHI3_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan { get; set; }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        [Column("KOHI4_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan { get; set; }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        [Column("ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan { get; set; }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        [Column("GENMEN_GAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku { get; set; }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        [Column("HOKEN_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan10en { get; set; }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        [Column("KOGAKU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan10en { get; set; }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI1_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan10en { get; set; }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI2_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan10en { get; set; }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI3_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan10en { get; set; }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI4_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan10en { get; set; }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        [Column("ICHIBU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan10en { get; set; }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        [Column("GENMEN_GAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku10en { get; set; }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        [Column("ADJUST_ROUND")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustRound { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("PT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        [Column("KOGAKU_OVER_KBN")]
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
        [Column("RECE_SBT")]
        [MaxLength(4)]
        public string ReceSbt { get; set; }

        /// <summary>
        /// 実日数
        /// 
        /// </summary>
        [Column("JITUNISU")]
        [CustomAttribute.DefaultValue(0)]
        public int Jitunisu { get; set; }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        [Column("ROUSAI_I_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiIFutan { get; set; }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        [Column("ROUSAI_RO_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiRoFutan { get; set; }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        [Column("JIBAI_I_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiITensu { get; set; }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        [Column("JIBAI_RO_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiRoTensu { get; set; }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        [Column("JIBAI_HA_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHaFutan { get; set; }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        [Column("JIBAI_NI_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiNiFutan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        [Column("JIBAI_HO_SINDAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        [Column("JIBAI_HO_SINDAN_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindanCount { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        [Column("JIBAI_HE_MEISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisai { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        [Column("JIBAI_HE_MEISAI_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisaiCount { get; set; }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        [Column("JIBAI_A_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiAFutan { get; set; }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        [Column("JIBAI_B_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiBFutan { get; set; }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        [Column("JIBAI_C_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiCFutan { get; set; }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        [Column("JIBAI_D_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiDFutan { get; set; }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        [Column("JIBAI_KENPO_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoTensu { get; set; }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        [Column("JIBAI_KENPO_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoFutan { get; set; }

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
        /// 患者負担合計額
        /// 
        /// </summary>
        [Column("TOTAL_PT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalPtFutan { get; set; }

        /// <summary>
        /// 計算順番
        ///     診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        [Column("SORT_KEY")]
        [MaxLength(61)]
        public string SortKey { get; set; }

        /// <summary>
        /// 妊婦フラグ
        /// 1:対象
        /// </summary>
        [Column("IS_NINPU")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNinpu { get; set; }

        /// <summary>
        /// 在医総フラグ
        /// 1:1:在医総管又は在医総
        /// </summary>
        [Column("IS_ZAIISO")]
        [CustomAttribute.DefaultValue(0)]
        public int IsZaiiso { get; set; }

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

    }
}
