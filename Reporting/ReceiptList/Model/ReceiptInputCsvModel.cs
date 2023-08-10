using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.CommonMasters.Config;

namespace Reporting.ReceiptList.Model
{
    public class ReceiptInputCsvModel
    {
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId { get; set; }

        /// <summary>
        ///  請求区分
        /// </summary>
        public int SeikyuKbn { get; set; }

        public string SeikyuKbnisplay
        {
            get
            {
                string result = string.Empty;
                switch (SeikyuKbn)
                {
                    case 1:
                        result = "月遅れ";
                        break;
                    case 2:
                        result = "返戻";
                        break;
                    case 3:
                        result = "オ返戻";
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm { get; set; }

        public string SinYmDisplay
        {
            get
            {
                return CIUtil.SMonthToShowSMonth(SinYm);
            }
        }

        /// <summary>
        /// 変更
        /// </summary>
        /// <returns></returns>
        public int IsReceInfDetailExist { get; set; }

        public string ReceInfDetailExistDisplay
        {
            get => GetStringFromPropertyValue(IsReceInfDetailExist);
        }

        public int RosaiReceden { get; set; }

        /// <summary>
        /// 紙
        /// </summary>
        public int IsPaperRece { get; set; }

        public string PaperReceDisplay
        {
            get => GetStringFromPropertyValue(IsPaperRece);
        }

        /// <summary>
        /// 印刷
        /// </summary>
        /// <returns></returns>
        public int Output { get; set; }

        public string OutputDisplay
        {
            get => GetStringFromPropertyValue(Output);
        }

        /// <summary>
        /// 付箋
        /// </summary>
        public int FusenKbn { get; set; }

        public string FusenKbnDisplay
        {
            get
            {
                if (FusenKbn >= 1) return "☆";
                return string.Empty;
            }
        }

        public bool IsLoginUserFusen
        {
            get
            {
                if (ReceStatusCreateId == Session.UserID) return true;
                return false;
            }
        }

        /// <summary>
        /// 確認
        /// </summary>
        public int StatusKbn
        {
            get; set;
        }

        public string StatusKbnDisplay
        {
            get
            {
                string status = string.Empty;
                switch (StatusKbn)
                {
                    case 1:
                        status = "システム保留";
                        break;
                    case 2:
                        status = "保留1";
                        break;
                    case 3:
                        status = "保留2";
                        break;
                    case 4:
                        status = "保留3";
                        break;
                    case 8:
                        status = "仮";
                        break;
                    case 9:
                        status = "済";
                        break;
                }
                return status;
            }
        }

        /// <summary>
        /// コメント
        /// </summary>
        public string ReceCheckCmt { get; set; }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum { get; set; }

        public string PtNumDisplay
        {
            get
            {
                if (IsDefaultModel)
                {
                    return string.Empty;
                }
                return PtNum.AsString();
            }
        }

        /// <summary>
        /// カナ
        /// </summary>
        public string KanaName { get; set; }

        /// <summary>
        /// 氏名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性
        /// </summary>
        public int Sex { get; set; }

        public string SexDisplay
        {
            get
            {
                if (Sex == 1)
                {
                    return "男";
                }
                else if (Sex == 2)
                {
                    return "女";
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 年齢
        /// </summary>
        public int Age { get; set; }

        public string AgeDisplay
        {
            get
            {
                if (IsDefaultModel)
                {
                    return string.Empty;
                }
                return Age.AsString();
            }
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay { get; set; }

        public string BirthDayDisplay
        {
            get => CIUtil.SDateToShowSDate(BirthDay);
        }

        /// <summary>
        /// レセプト種別
        /// </summary>
        public string ReceSbt { get; set; }

        public int HokenKbn { get; set; }

        public int HokenId { get; set; }


        public string ReceSbtDisplay
        {
            get
            {
                string result = "ー";
                switch (HokenKbn)
                {
                    case 0:
                        if (!string.IsNullOrWhiteSpace(ReceSbt) && ReceSbt.Length > 0)
                        {
                            if (ReceSbt[0] == '8')
                            {
                                result = "自費";
                            }
                            else if (ReceSbt[0] == '9')
                            {
                                result = "自費レセ";
                            }
                        }
                        break;
                    case 1:
                        if (ReceiptListConstant.ShaHoDict.ContainsKey(ReceSbt))
                        {
                            result = ReceiptListConstant.ShaHoDict[ReceSbt];
                        }
                        break;
                    case 2:
                        if (ReceiptListConstant.KokuHoDict.ContainsKey(ReceSbt))
                        {
                            result = ReceiptListConstant.KokuHoDict[ReceSbt];
                        }
                        break;
                    case 11:
                        result = "労災(短期給付)";
                        break;
                    case 12:
                        result = "労災(傷病年金)";
                        break;
                    case 13:
                        result = "アフターケア";
                        break;
                    case 14:
                        result = "自賠責";
                        break;
                }

                if (IsDefaultModel)
                {
                    result = string.Empty;
                }

                return result;
            }
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo { get; set; }

        /// <summary>
        /// 診療点数
        /// </summary>
        public int Tensu { get; set; }

        public string TensuDisplay
        {
            get
            {
                if (IsDefaultModel)
                {
                    return string.Empty;
                }
                return Tensu.AsString();
            }
        }

        /// <summary>
        /// 実日数
        /// </summary>
        public int HokenNissu { get; set; }

        public string HokenNissuDisplay
        {
            get
            {
                if (IsDefaultModel)
                {
                    return string.Empty;
                }
                return HokenNissu.AsString();
            }
        }

        public int Kohi1Nissu { get; set; }

        public int Nissu
        {
            get
            {
                int ret = HokenNissu.AsInteger();

                if (new int[] { 1, 2 }.Contains(HokenKbn) && ReceSbt.StartsWith("12"))
                {
                    ret = Kohi1Nissu;
                }
                return ret;
            }
        }

        public string NissuDisplay
        {
            get { return Nissu.AsString(); }
        }

        /// <summary>
        /// 症状詳記
        /// </summary>
        public int IsSyoukiInfExist { get; set; }

        public string SyoukiInfExistDisplay
        {
            get => GetStringFromPropertyValue(IsSyoukiInfExist);
        }

        /// <summary>
        /// レセコメント
        /// </summary>
        public int IsReceCmtExist { get; set; }

        public string ReceCmtExistDisplay
        {
            get => GetStringFromPropertyValue(IsReceCmtExist);
        }

        /// <summary>
        /// 傷病の経過
        /// </summary>
        public int IsSyobyoKeikaExist
        {
            get; set;
        }

        public string SyobyoKeikaExistDisplay
        {
            get => GetStringFromPropertyValue(IsSyobyoKeikaExist);
        }

        /// <summary>
        /// 再請求コメント
        /// </summary>
        public string ReceSeikyuCmt { get; set; }

        /// <summary>
        /// 最終来院
        /// </summary>
        public int LastVisitDate { get; set; }

        public string LastVisitDateDisplay
        {
            get => CIUtil.SDateToShowSDate(LastVisitDate);
        }

        /// <summary>
        /// 診療科
        /// </summary>
        public string KaName { get; set; }

        /// <summary>
        /// 担当医
        /// </summary>
        public string SName { get; set; }

        /// <summary>
        /// 旧姓
        /// </summary>
        public int IsPtKyuseiExist
        {
            get; set;
        }

        public string PtKyuseiExistDisplay
        {
            get => GetStringFromPropertyValue(IsPtKyuseiExist);
        }

        /// <summary>
        /// 公１負担者番号
        /// </summary>
        public string FutansyaNoKohi1 { get; set; }

        /// <summary>
        /// 公２負担者番号
        /// </summary>
        public string FutansyaNoKohi2 { get; set; }

        /// <summary>
        /// 公３負担者番号
        /// </summary>
        public string FutansyaNoKohi3 { get; set; }

        /// <summary>
        /// 公４負担者番号
        /// </summary>
        public string FutansyaNoKohi4 { get; set; }

        public string GetStringFromPropertyValue(int propertyValue)
        {
            if (propertyValue == 1) return "○";
            return string.Empty;
        }


        public bool IsDefaultModel
        {
            get => PtId == 0;
        }

        public int ReceStatusCreateId { get; set; }
        public bool CheckDefaultValue()
        {
            return IsDefaultModel;
        }
        public bool IsPtTest { get; set; }
        public string IsPtTestDisplay
        {
            get
            {
                if (IsPtTest)
                {
                    return "○";
                }
                return string.Empty;
            }
        }

        public int ExpectedPayment { get; set; }


        /// <summary>
        /// 労災交付番号
        /// </summary>
        public string RousaiKofuNo { get; set; }

        /// <summary>
        /// 労災事業所名
        /// </summary>
        public string RousaiJigyosyoName { get; set; }

        /// <summary>
        /// 労災都道府県名
        /// </summary>
        public string RousaiPrefName { get; set; }

        /// <summary>
        /// 労災所在地郡市区名
        /// </summary>
        public string RousaiCityName { get; set; }

        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName { get; set; }

        /// <summary>
        /// 自賠保険担当者
        /// </summary>
        public string JibaiHokenTanto { get; set; }

        /// <summary>
        /// 自賠保険連絡先
        /// </summary>
        public string JibaiHokenTel { get; set; }
    }
}
