using Entity.Tenant;

namespace Reporting.Receipt.Models
{
    public class CoKaikeiDetailModel
    {
        public KaikeiDetail KaikeiDetail { get; }

        public CoKaikeiDetailModel(KaikeiDetail kaikeiDetail)
        {
            KaikeiDetail = kaikeiDetail;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KaikeiDetail.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return KaikeiDetail.PtId; }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return KaikeiDetail.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return KaikeiDetail.RaiinNo; }
        }

        /// <summary>
        /// 親来院番号
        /// 
        /// </summary>
        public long OyaRaiinNo
        {
            get { return KaikeiDetail.OyaRaiinNo; }
        }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid
        {
            get { return KaikeiDetail.HokenPid; }
        }

        /// <summary>
        /// 合算調整Pid
        /// 
        /// </summary>
        public int AdjustPid
        {
            get { return KaikeiDetail.AdjustPid; }
        }

        /// <summary>
        /// 合算調整KohiId
        /// 
        /// </summary>
        public int AdjustKid
        {
            get { return KaikeiDetail.AdjustKid; }
        }

        /// <summary>
        /// 保険区分
        /// 
        /// </summary>
        public int HokenKbn
        {
            get { return KaikeiDetail.HokenKbn; }
        }

        /// <summary>
        /// 保険種別コード
        /// 
        /// </summary>
        public int HokenSbtCd
        {
            get { return KaikeiDetail.HokenSbtCd; }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return KaikeiDetail.HokenId; }
        }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        public int Kohi1Id
        {
            get { return KaikeiDetail.Kohi1Id; }
        }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        public int Kohi2Id
        {
            get { return KaikeiDetail.Kohi2Id; }
        }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        public int Kohi3Id
        {
            get { return KaikeiDetail.Kohi3Id; }
        }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        public int Kohi4Id
        {
            get { return KaikeiDetail.Kohi4Id; }
        }

        /// <summary>
        /// 労災保険ID
        /// 
        /// </summary>
        public int RousaiId
        {
            get { return KaikeiDetail.RousaiId; }
        }

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        public string Houbetu
        {
            get { return KaikeiDetail.Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return KaikeiDetail.Kohi1Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return KaikeiDetail.Kohi2Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return KaikeiDetail.Kohi3Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return KaikeiDetail.Kohi4Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 公費１優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi1Priority
        {
            get { return KaikeiDetail.Kohi1Priority ?? string.Empty; }
        }

        /// <summary>
        /// 公費２優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi2Priority
        {
            get { return KaikeiDetail.Kohi2Priority ?? string.Empty; }
        }

        /// <summary>
        /// 公費３優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi3Priority
        {
            get { return KaikeiDetail.Kohi3Priority ?? string.Empty; }
        }

        /// <summary>
        /// 公費４優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi4Priority
        {
            get { return KaikeiDetail.Kohi4Priority ?? string.Empty; }
        }

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        public int HonkeKbn
        {
            get { return KaikeiDetail.HonkeKbn; }
        }

        /// <summary>
        /// 高額療養費区分
        /// PT_HOKEN_INF.KOGAKU_KBN
        /// </summary>
        public int KogakuKbn
        {
            get { return KaikeiDetail.KogakuKbn; }
        }

        /// <summary>
        /// 高額療養費適用区分
        /// PT_HOKEN_INF.KOGAKU_TEKIYO_KBN
        /// </summary>
        public int KogakuTekiyoKbn
        {
            get { return KaikeiDetail.KogakuTekiyoKbn; }
        }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        public int IsTokurei
        {
            get { return KaikeiDetail.IsTokurei; }
        }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        public int IsTasukai
        {
            get { return KaikeiDetail.IsTasukai; }
        }

        /// <summary>
        /// 高額療養費合算対象
        ///     1:合算対象外
        ///     2:21,000未満合算対象
        /// </summary>
        public int KogakuTotalKbn
        {
            get { return KaikeiDetail.KogakuTotalKbn; }
        }

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        public int IsChoki
        {
            get { return KaikeiDetail.IsChoki; }
        }

        /// <summary>
        /// 高額療養費限度額
        /// 
        /// </summary>
        public int KogakuLimit
        {
            get { return KaikeiDetail.KogakuLimit; }
        }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        public int TotalKogakuLimit
        {
            get { return KaikeiDetail.TotalKogakuLimit; }
        }

        /// <summary>
        /// 国保減免区分
        /// PT_HOKEN_INF.GENMEN_KBN
        /// </summary>
        public int GenmenKbn
        {
            get { return KaikeiDetail.GenmenKbn; }
        }

        /// <summary>
        /// 点数単価
        /// PT_HOKEN_INF.EN_TEN
        /// </summary>
        public double EnTen
        {
            get { return KaikeiDetail.EnTen; }
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get { return KaikeiDetail.HokenRate; }
        }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate
        {
            get { return KaikeiDetail.PtRate; }
        }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        public int Kohi1Limit
        {
            get { return KaikeiDetail.Kohi1Limit; }
        }

        /// <summary>
        /// 公１他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi1OtherFutan
        {
            get { return KaikeiDetail.Kohi1OtherFutan; }
        }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        public int Kohi2Limit
        {
            get { return KaikeiDetail.Kohi2Limit; }
        }

        /// <summary>
        /// 公２他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi2OtherFutan
        {
            get { return KaikeiDetail.Kohi2OtherFutan; }
        }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        public int Kohi3Limit
        {
            get { return KaikeiDetail.Kohi3Limit; }
        }

        /// <summary>
        /// 公３他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi3OtherFutan
        {
            get { return KaikeiDetail.Kohi3OtherFutan; }
        }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        public int Kohi4Limit
        {
            get { return KaikeiDetail.Kohi4Limit; }
        }

        /// <summary>
        /// 公４他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi4OtherFutan
        {
            get { return KaikeiDetail.Kohi4OtherFutan; }
        }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu
        {
            get { return KaikeiDetail.Tensu; }
        }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi
        {
            get { return KaikeiDetail.TotalIryohi; }
        }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        public int HokenFutan
        {
            get { return KaikeiDetail.HokenFutan; }
        }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        public int KogakuFutan
        {
            get { return KaikeiDetail.KogakuFutan; }
        }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        public int Kohi1Futan
        {
            get { return KaikeiDetail.Kohi1Futan; }
        }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        public int Kohi2Futan
        {
            get { return KaikeiDetail.Kohi2Futan; }
        }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        public int Kohi3Futan
        {
            get { return KaikeiDetail.Kohi3Futan; }
        }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        public int Kohi4Futan
        {
            get { return KaikeiDetail.Kohi4Futan; }
        }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        public int IchibuFutan
        {
            get { return KaikeiDetail.IchibuFutan; }
        }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        public int GenmenGaku
        {
            get { return KaikeiDetail.GenmenGaku; }
        }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        public int HokenFutan10en
        {
            get { return KaikeiDetail.HokenFutan10en; }
        }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        public int KogakuFutan10en
        {
            get { return KaikeiDetail.KogakuFutan10en; }
        }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        public int Kohi1Futan10en
        {
            get { return KaikeiDetail.Kohi1Futan10en; }
        }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        public int Kohi2Futan10en
        {
            get { return KaikeiDetail.Kohi2Futan10en; }
        }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        public int Kohi3Futan10en
        {
            get { return KaikeiDetail.Kohi3Futan10en; }
        }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        public int Kohi4Futan10en
        {
            get { return KaikeiDetail.Kohi4Futan10en; }
        }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        public int IchibuFutan10en
        {
            get { return KaikeiDetail.IchibuFutan10en; }
        }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        public int GenmenGaku10en
        {
            get { return KaikeiDetail.GenmenGaku10en; }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return KaikeiDetail.PtFutan; }
        }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        public int KogakuOverKbn
        {
            get { return KaikeiDetail.KogakuOverKbn; }
        }

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
        public string ReceSbt
        {
            get { return KaikeiDetail.ReceSbt ?? string.Empty; }
        }

        /// <summary>
        /// 実日数
        /// 
        /// </summary>
        public int Jitunisu
        {
            get { return KaikeiDetail.Jitunisu; }
        }

        /// <summary>
        /// 労災イ点数
        /// 
        /// </summary>
        public int RousaiITensu
        {
            get { return KaikeiDetail.RousaiITensu; }
        }


        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        public int RousaiIFutan
        {
            get { return KaikeiDetail.RousaiIFutan; }
        }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        public int RousaiRoFutan
        {
            get { return KaikeiDetail.RousaiRoFutan; }
        }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        public int JibaiITensu
        {
            get { return KaikeiDetail.JibaiITensu; }
        }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        public int JibaiRoTensu
        {
            get { return KaikeiDetail.JibaiRoTensu; }
        }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        public int JibaiHaFutan
        {
            get { return KaikeiDetail.JibaiHaFutan; }
        }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        public int JibaiNiFutan
        {
            get { return KaikeiDetail.JibaiNiFutan; }
        }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        public int JibaiHoSindan
        {
            get { return KaikeiDetail.JibaiHoSindan; }
        }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        public int JibaiHeMeisai
        {
            get { return KaikeiDetail.JibaiHeMeisai; }
        }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        public int JibaiAFutan
        {
            get { return KaikeiDetail.JibaiAFutan; }
        }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        public int JibaiBFutan
        {
            get { return KaikeiDetail.JibaiBFutan; }
        }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        public int JibaiCFutan
        {
            get { return KaikeiDetail.JibaiCFutan; }
        }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        public int JibaiDFutan
        {
            get { return KaikeiDetail.JibaiDFutan; }
        }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        public int JibaiKenpoFutan
        {
            get { return KaikeiDetail.JibaiKenpoFutan; }
        }

        /// <summary>
        /// 自費負担額
        /// 
        /// </summary>
        public int JihiFutan
        {
            get { return KaikeiDetail.JihiFutan; }
        }

        /// <summary>
        /// 自費内税
        /// 
        /// </summary>
        public int JihiTax
        {
            get { return KaikeiDetail.JihiTax; }
        }

        /// <summary>
        /// 自費外税
        /// 
        /// </summary>
        public int JihiOuttax
        {
            get { return KaikeiDetail.JihiOuttax; }
        }

        /// <summary>
        /// 患者負担合計額
        /// 
        /// </summary>
        public int TotalPtFutan
        {
            get { return KaikeiDetail.TotalPtFutan; }
        }

        /// <summary>
        /// 計算順番
        ///     診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        public string SortKey
        {
            get { return KaikeiDetail.SortKey ?? string.Empty; }
        }

        /// <summary>
        /// 妊婦フラグ
        /// 1:対象
        /// </summary>
        public int IsNinpu
        {
            get { return KaikeiDetail.IsNinpu; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KaikeiDetail.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KaikeiDetail.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KaikeiDetail.CreateMachine ?? string.Empty; }
        }
    }
}
