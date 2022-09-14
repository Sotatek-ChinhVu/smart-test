using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class KaikeiInfModel
    {
        public KaikeiInf KaikeiInf { get; }

        public KaikeiInfModel(KaikeiInf kaikeiInf)
        {
            KaikeiInf = kaikeiInf;
            Kohi1Priority = "9999999";
            Kohi2Priority = "9999999";
            Kohi3Priority = "9999999";
            Kohi4Priority = "9999999";
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KaikeiInf.HpId; }
            set
            {
                if (KaikeiInf.HpId == value) return;
                KaikeiInf.HpId = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return KaikeiInf.PtId; }
            set
            {
                if (KaikeiInf.PtId == value) return;
                KaikeiInf.PtId = value;
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return KaikeiInf.SinDate; }
            set
            {
                if (KaikeiInf.SinDate == value) return;
                KaikeiInf.SinDate = value;
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return KaikeiInf.RaiinNo; }
            set
            {
                if (KaikeiInf.RaiinNo == value) return;
                KaikeiInf.RaiinNo = value;
            }
        }

        /// <summary>
        /// 保険ID
        /// PT_HOKEN_INF.HOKEN_ID
        /// </summary>
        public int HokenId
        {
            get { return KaikeiInf.HokenId; }
            set
            {
                if (KaikeiInf.HokenId == value) return;
                KaikeiInf.HokenId = value;
            }
        }

        /// <summary>
        /// 公費１ID
        /// 
        /// </summary>
        public int Kohi1Id
        {
            get { return KaikeiInf.Kohi1Id; }
            set
            {
                if (KaikeiInf.Kohi1Id == value) return;
                KaikeiInf.Kohi1Id = value;
            }
        }

        /// <summary>
        /// 公費２ID
        /// 
        /// </summary>
        public int Kohi2Id
        {
            get { return KaikeiInf.Kohi2Id; }
            set
            {
                if (KaikeiInf.Kohi2Id == value) return;
                KaikeiInf.Kohi2Id = value;
            }
        }

        /// <summary>
        /// 公費３ID
        /// 
        /// </summary>
        public int Kohi3Id
        {
            get { return KaikeiInf.Kohi3Id; }
            set
            {
                if (KaikeiInf.Kohi3Id == value) return;
                KaikeiInf.Kohi3Id = value;
            }
        }

        /// <summary>
        /// 公費４ID
        /// 
        /// </summary>
        public int Kohi4Id
        {
            get { return KaikeiInf.Kohi4Id; }
            set
            {
                if (KaikeiInf.Kohi4Id == value) return;
                KaikeiInf.Kohi4Id = value;
            }
        }

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
        public int HokenKbn
        {
            get { return KaikeiInf.HokenKbn; }
            set
            {
                if (KaikeiInf.HokenKbn == value) return;
                KaikeiInf.HokenKbn = value;
            }
        }

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
        public int HokenSbtCd
        {
            get { return KaikeiInf.HokenSbtCd; }
            set
            {
                if (KaikeiInf.HokenSbtCd == value) return;
                KaikeiInf.HokenSbtCd = value;
            }
        }

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
        public string ReceSbt
        {
            get { return KaikeiInf.ReceSbt; }
            set
            {
                if (KaikeiInf.ReceSbt == value) return;
                KaikeiInf.ReceSbt = value;
            }
        }

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        public string Houbetu
        {
            get { return KaikeiInf.Houbetu; }
            set
            {
                if (KaikeiInf.Houbetu == value) return;
                KaikeiInf.Houbetu = value;
            }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return KaikeiInf.Kohi1Houbetu; }
            set
            {
                if (KaikeiInf.Kohi1Houbetu == value) return;
                KaikeiInf.Kohi1Houbetu = value;
            }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return KaikeiInf.Kohi2Houbetu; }
            set
            {
                if (KaikeiInf.Kohi2Houbetu == value) return;
                KaikeiInf.Kohi2Houbetu = value;
            }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return KaikeiInf.Kohi3Houbetu; }
            set
            {
                if (KaikeiInf.Kohi3Houbetu == value) return;
                KaikeiInf.Kohi3Houbetu = value;
            }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return KaikeiInf.Kohi4Houbetu; }
            set
            {
                if (KaikeiInf.Kohi4Houbetu == value) return;
                KaikeiInf.Kohi4Houbetu = value;
            }
        }

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        public int HonkeKbn
        {
            get { return KaikeiInf.HonkeKbn; }
            set
            {
                if (KaikeiInf.HonkeKbn == value) return;
                KaikeiInf.HonkeKbn = value;
            }
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get { return KaikeiInf.HokenRate; }
            set
            {
                if (KaikeiInf.HokenRate == value) return;
                KaikeiInf.HokenRate = value;
            }
        }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate
        {
            get { return KaikeiInf.PtRate; }
            set
            {
                if (KaikeiInf.PtRate == value) return;
                KaikeiInf.PtRate = value;
            }
        }

        /// <summary>
        /// 表示用負担率
        /// 
        /// </summary>
        public int DispRate
        {
            get { return KaikeiInf.DispRate; }
            set
            {
                if (KaikeiInf.DispRate == value) return;
                KaikeiInf.DispRate = value;
            }
        }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu
        {
            get { return KaikeiInf.Tensu; }
            set
            {
                if (KaikeiInf.Tensu == value) return;
                KaikeiInf.Tensu = value;
            }
        }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi
        {
            get { return KaikeiInf.TotalIryohi; }
            set
            {
                if (KaikeiInf.TotalIryohi == value) return;
                KaikeiInf.TotalIryohi = value;
            }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return KaikeiInf.PtFutan; }
            set
            {
                if (KaikeiInf.PtFutan == value) return;
                KaikeiInf.PtFutan = value;
            }
        }

        /// <summary>
        /// 自費負担額
        /// 
        /// </summary>
        public int JihiFutan
        {
            get { return KaikeiInf.JihiFutan; }
            set
            {
                if (KaikeiInf.JihiFutan == value) return;
                KaikeiInf.JihiFutan = value;
            }
        }

        /// <summary>
        /// 自費内税
        /// 
        /// </summary>
        public int JihiTax
        {
            get { return KaikeiInf.JihiTax; }
            set
            {
                if (KaikeiInf.JihiTax == value) return;
                KaikeiInf.JihiTax = value;
            }
        }

        /// <summary>
        /// 自費外税
        /// 
        /// </summary>
        public int JihiOuttax
        {
            get { return KaikeiInf.JihiOuttax; }
            set
            {
                if (KaikeiInf.JihiOuttax == value) return;
                KaikeiInf.JihiOuttax = value;
            }
        }

        /// <summary>
        /// 自費負担額(非課税)
        /// 
        /// </summary>
        public int JihiFutanTaxfree
        {
            get { return KaikeiInf.JihiFutanTaxfree; }
            set
            {
                if (KaikeiInf.JihiFutanTaxfree == value) return;
                KaikeiInf.JihiFutanTaxfree = value;
            }
        }

        /// <summary>
        /// 自費負担額(内税・通常税率)
        /// 
        /// </summary>
        public int JihiFutanTaxNr
        {
            get { return KaikeiInf.JihiFutanTaxNr; }
            set
            {
                if (KaikeiInf.JihiFutanTaxNr == value) return;
                KaikeiInf.JihiFutanTaxNr = value;
            }
        }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        public int JihiFutanTaxGen
        {
            get { return KaikeiInf.JihiFutanTaxGen; }
            set
            {
                if (KaikeiInf.JihiFutanTaxGen == value) return;
                KaikeiInf.JihiFutanTaxGen = value;
            }
        }

        /// <summary>
        /// 自費負担額(外税・通常税率)
        /// 
        /// </summary>
        public int JihiFutanOuttaxNr
        {
            get { return KaikeiInf.JihiFutanOuttaxNr; }
            set
            {
                if (KaikeiInf.JihiFutanOuttaxNr == value) return;
                KaikeiInf.JihiFutanOuttaxNr = value;
            }
        }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        public int JihiFutanOuttaxGen
        {
            get { return KaikeiInf.JihiFutanOuttaxGen; }
            set
            {
                if (KaikeiInf.JihiFutanOuttaxGen == value) return;
                KaikeiInf.JihiFutanOuttaxGen = value;
            }
        }

        /// <summary>
        /// 自費内税(通常税率)
        /// 
        /// </summary>
        public int JihiTaxNr
        {
            get { return KaikeiInf.JihiTaxNr; }
            set
            {
                if (KaikeiInf.JihiTaxNr == value) return;
                KaikeiInf.JihiTaxNr = value;
            }
        }

        /// <summary>
        /// 自費内税(軽減税率)
        /// 
        /// </summary>
        public int JihiTaxGen
        {
            get { return KaikeiInf.JihiTaxGen; }
            set
            {
                if (KaikeiInf.JihiTaxGen == value) return;
                KaikeiInf.JihiTaxGen = value;
            }
        }

        /// <summary>
        /// 自費外税(通常税率)
        /// 
        /// </summary>
        public int JihiOuttaxNr
        {
            get { return KaikeiInf.JihiOuttaxNr; }
            set
            {
                if (KaikeiInf.JihiOuttaxNr == value) return;
                KaikeiInf.JihiOuttaxNr = value;
            }
        }

        /// <summary>
        /// 自費外税(軽減税率)
        /// 
        /// </summary>
        public int JihiOuttaxGen
        {
            get { return KaikeiInf.JihiOuttaxGen; }
            set
            {
                if (KaikeiInf.JihiOuttaxGen == value) return;
                KaikeiInf.JihiOuttaxGen = value;
            }
        }

        /// <summary>
        /// 調整額
        /// 
        /// </summary>
        public int AdjustFutan
        {
            get { return KaikeiInf.AdjustFutan; }
            set
            {
                if (KaikeiInf.AdjustFutan == value) return;
                KaikeiInf.AdjustFutan = value;
            }
        }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        public int AdjustRound
        {
            get { return KaikeiInf.AdjustRound; }
            set
            {
                if (KaikeiInf.AdjustRound == value) return;
                KaikeiInf.AdjustRound = value;
            }
        }

        /// <summary>
        /// 患者負担合計額
        /// 
        /// </summary>
        public int TotalPtFutan
        {
            get { return KaikeiInf.TotalPtFutan; }
            set
            {
                if (KaikeiInf.TotalPtFutan == value) return;
                KaikeiInf.TotalPtFutan = value;
            }
        }

        /// <summary>
        /// 調整額設定値
        /// 
        /// </summary>
        public int AdjustFutanVal
        {
            get { return KaikeiInf.AdjustFutanVal; }
            set
            {
                if (KaikeiInf.AdjustFutanVal == value) return;
                KaikeiInf.AdjustFutanVal = value;
            }
        }

        /// <summary>
        /// 調整額設定範囲
        /// 
        /// </summary>
        public int AdjustFutanRange
        {
            get { return KaikeiInf.AdjustFutanRange; }
            set
            {
                if (KaikeiInf.AdjustFutanRange == value) return;
                KaikeiInf.AdjustFutanRange = value;
            }
        }

        /// <summary>
        /// 調整率設定値
        /// 
        /// </summary>
        public int AdjustRateVal
        {
            get { return KaikeiInf.AdjustRateVal; }
            set
            {
                if (KaikeiInf.AdjustRateVal == value) return;
                KaikeiInf.AdjustRateVal = value;
            }
        }

        /// <summary>
        /// 調整率設定範囲
        /// 
        /// </summary>
        public int AdjustRateRange
        {
            get { return KaikeiInf.AdjustRateRange; }
            set
            {
                if (KaikeiInf.AdjustRateRange == value) return;
                KaikeiInf.AdjustRateRange = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KaikeiInf.CreateDate; }
            set
            {
                if (KaikeiInf.CreateDate == value) return;
                KaikeiInf.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KaikeiInf.CreateId; }
            set
            {
                if (KaikeiInf.CreateId == value) return;
                KaikeiInf.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KaikeiInf.CreateMachine ?? string.Empty; }
            set
            {
                if (KaikeiInf.CreateMachine == value) return;
                KaikeiInf.CreateMachine = value;
            }
        }

        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi1Priority;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi2Priority;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi3Priority;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi4Priority;


        /// <summary>
        /// 公費優先順位の取得
        /// </summary>
        public string GetKohiPriority(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Priority;
                case 2:
                    return Kohi2Priority;
                case 3:
                    return Kohi3Priority;
                case 4:
                    return Kohi4Priority;
                default:
                    return "".PadLeft(7, '9');
            }
        }

        /// <summary>
        /// 公費優先順位の設定
        /// </summary>
        public void SetKohiPriority(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Priority = value;
                    break;
                case 2:
                    Kohi2Priority = value;
                    break;
                case 3:
                    Kohi3Priority = value;
                    break;
                case 4:
                    Kohi4Priority = value;
                    break;
            }
        }

        /// <summary>
        /// 公費IDの取得
        /// </summary>
        public int GetKohiId(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Id;
                case 2:
                    return Kohi2Id;
                case 3:
                    return Kohi3Id;
                case 4:
                    return Kohi4Id;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費IDの設定
        /// </summary>
        public void SetKohiId(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Id = value;
                    break;
                case 2:
                    Kohi2Id = value;
                    break;
                case 3:
                    Kohi3Id = value;
                    break;
                case 4:
                    Kohi4Id = value;
                    break;
            }
        }

        /// <summary>
        /// 公費法別番号の取得
        /// </summary>
        public string GetKohiHoubetu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Houbetu;
                case 2:
                    return Kohi2Houbetu;
                case 3:
                    return Kohi3Houbetu;
                case 4:
                    return Kohi4Houbetu;
                default:
                    return "";
            }
        }

        /// <summary>
        /// 公費法別番号の設定
        /// </summary>
        public void SetKohiHoubetu(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Houbetu = value;
                    break;
                case 2:
                    Kohi2Houbetu = value;
                    break;
                case 3:
                    Kohi3Houbetu = value;
                    break;
                case 4:
                    Kohi4Houbetu = value;
                    break;
            }
        }
    }
}
