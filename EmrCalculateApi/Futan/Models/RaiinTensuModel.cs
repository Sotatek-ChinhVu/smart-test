namespace EmrCalculateApi.Futan.Models
{
    public class RaiinTensuModel
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo { get; set; }

        /// <summary>
        /// 親来院番号
        /// </summary>
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 診察開始時間
        /// HH24MISS
        /// </summary>
        public string SinStartTime { get; set; } = string.Empty;

        /// <summary>
        /// 保険組合せID
        /// </summary>
        public int HokenPid { get; set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId { get; set; }

        /// <summary>
        /// 診療点数
        /// </summary>
        public int Tensu { get; set; }

        /// <summary>
        /// 自費
        /// </summary>
        public int JihiFutan { get; set; }

        /// <summary>
        /// 外税
        /// </summary>
        public int OutTax { get; set; }

        /// <summary>
        /// 内税
        /// </summary>
        public int InclTax { get; set; }

        /// <summary>
        /// 自費負担額(非課税)
        /// </summary>
        public int JihiTaxFree { get; set; }

        /// <summary>
        /// 自費負担額(外税・通常税率)
        /// </summary>
        public int JihiOutTaxNr { get; set; }

        /// <summary>
        /// 自費負担額(外税・軽減税率)
        /// </summary>
        public int JihiOutTaxGen { get; set; }

        /// <summary>
        /// 自費負担額(内税・通常税率)
        /// </summary>
        public int JihiTaxNr { get; set; }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// </summary>
        public int JihiTaxGen { get; set; }

        /// <summary>
        /// 自費外税(通常税率)
        /// </summary>
        public int OutTaxNr { get; set; }

        /// <summary>
        /// 自費外税(軽減税率)
        /// </summary>
        public int OutTaxGen { get; set; }

        /// <summary>
        /// 自費内税(通常税率)
        /// </summary>
        public int InclTaxNr { get; set; }

        /// <summary>
        /// 自費内税(軽減税率)
        /// </summary>
        public int InclTaxGen { get; set; }

        /// <summary>
        /// 公費１の法別番号
        /// </summary>
        public string Kohi1Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公費１の優先順位
        /// </summary>
        public string Kohi1PriorityNo { get; set; } = string.Empty;

        /// <summary>
        /// 公費２の法別番号
        /// </summary>
        public string Kohi2Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公費２の優先順位
        /// </summary>
        public string Kohi2PriorityNo { get; set; } = string.Empty;

        /// <summary>
        /// 公費３の法別番号
        /// </summary>
        public string Kohi3Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公費３の優先順位
        /// </summary>
        public string Kohi3PriorityNo { get; set; } = string.Empty;

        /// <summary>
        /// 公費４の法別番号
        /// </summary>
        public string Kohi4Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公費４の優先順位
        /// </summary>
        public string Kohi4PriorityNo { get; set; } = string.Empty;

        /// <summary>
        /// 計算順番
        ///     診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        public string SortKey
        {
            get
            {
                return string.Format(
                "{0}{1}{2:D10}{3}{4}{5}{6}{7}{8}{9}{10}{11:D4}0",
                SinDate, SinStartTime?.PadRight(6, '0') ?? "000000", RaiinNo,
                Kohi1PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi1Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi2PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi2Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi3PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi3Houbetu?.PadLeft(3, '0') ?? "999",
                Kohi4PriorityNo?.PadLeft(5, '9') ?? "99999", Kohi4Houbetu?.PadLeft(3, '0') ?? "999",
                HokenPid);
            }
        }

        /// <summary>
        /// 実日数カウント有無
        /// </summary>
        public bool JituNisu { get; set; }

        /// <summary>
        /// 初再診区分
        ///  0:なし 1:初診 2:初診紹介(廃止) 3:再診 4:電話再診 5:なし(×自動算定) 6:同日初診 7:再診(2科目) 8:電話再診(2科目)
        /// </summary>
        public int SyosaisinKbn { get; set; }

        /// <summary>
        /// 保険区分 (SIN_RP_INF.HOKEN_KBN)
        ///  0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn { get; set; }

        /// <summary>
        /// 労災イ点負担額
        /// </summary>
        public int RousaiIFutan { get; set; }

        /// <summary>
        /// 労災ロ円負担額
        /// </summary>
        public int RousaiRoFutan { get; set; }

        /// <summary>
        /// 自賠イ技術点数
        /// </summary>
        public int JibaiITensu { get; set; }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// </summary
        public int JibaiRoTensu { get; set; }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// </summary>
        public int JibaiHaFutan { get; set; }

        /// <summary>
        /// 自賠ニ円他負担額
        /// </summary>
        public int JibaiNiFutan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料
        /// </summary>
        public int JibaiHoSindan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// </summary>
        public int JibaiHoSindanCount { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料
        /// </summary>
        public int JibaiHeMeisai { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// </summary>
        public int JibaiHeMeisaiCount { get; set; }

        /// <summary>
        /// 自賠健保点数
        /// </summary>
        public int JibaiKenpoTensu { get; set; }

        /// <summary>
        /// 妊婦
        /// </summary>
        public bool IsNinpu { get; set; }

        /// <summary>
        /// 在医総及び在医総管
        /// </summary>
        public bool IsZaiiso { get; set; }

        /// <summary>
        /// 労災用（円→点数換算）
        /// </summary>
        public int RousaiEnTensu { get; set; }
    }
}
