using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public class P44WelfareDiskService : IP44WelfareDiskService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "84" };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP44WelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        #region Constructor and Init
        public P44WelfareDiskService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private string bikoKisai;
        #endregion

        public CommonExcelReportingModel GetDataP44WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuType = seikyuType;
                this.seikyuYm = seikyuYm;
                GetData();
                List<string> retDatas = new List<string>();

                if (GetData())
                {
                    //比較用の変数を初期化
                    long beforePtId = -1;
                    int beforeSinYm = -1;
                    string beforeHokensyaNo = "";
                    string beforeHonka = "";

                    foreach (var receInf in receInfs)
                    {
                        bikoKisai = "";

                        //2レコード以上ある場合
                        if (((receInf.PtId == beforePtId) && (beforePtId != -1)) && ((receInf.SinYm == beforeSinYm) && (beforeSinYm != -1)))
                        {
                            //保険者番号
                            if (receInf.HokensyaNo != beforeHokensyaNo)
                            {
                                bikoKisai = "月途中で保険者番号が変更あり２レコード以上作成";
                            }

                            //本人家族区分
                            if (receInf.Honka != beforeHonka)
                            {
                                if (bikoKisai == "")
                                {
                                    bikoKisai = "月途中で本人家族区分が変更あり２レコード以上作成";
                                }
                                else
                                {
                                    bikoKisai = bikoKisai + "、" + "月途中で本人家族区分が変更あり";
                                }
                            }
                        }

                        //公費が3つ以上
                        if (receInf.KohiHoubetu(3) != null && receInf.KohiHoubetu(3) != "")
                        {
                            if (bikoKisai == "")
                            {
                                bikoKisai = "公費番号" + receInf.KohiHoubetu(3) + "あり";
                            }
                            else
                            {
                                bikoKisai = bikoKisai + "、" + "公費番号" + receInf.KohiHoubetu(3) + "あり";
                            }
                        }

                        if (receInf.KohiHoubetu(4) != null && receInf.KohiHoubetu(4) != "")
                        {
                            if (bikoKisai == "")
                            {
                                bikoKisai = "公費番号" + receInf.KohiHoubetu(4) + "あり";
                            }
                            else
                            {
                                bikoKisai = bikoKisai + "、" + "公費番号" + receInf.KohiHoubetu(4) + "あり";
                            }
                        }

                        //特記事項が3つ以上
                        if (receInf.Tokki(3) != "")
                        {
                            if (bikoKisai == "")
                            {
                                bikoKisai = "特記事項" + receInf.Tokki(3) + "あり";
                            }
                            else
                            {
                                bikoKisai = bikoKisai + "、" + "特記事項" + receInf.Tokki(3) + "あり";
                            }
                        }

                        if (receInf.Tokki(4) != "")
                        {
                            if (bikoKisai == "")
                            {
                                bikoKisai = "特記事項" + receInf.Tokki(4) + "あり";
                            }
                            else
                            {
                                bikoKisai = bikoKisai + "、" + "特記事項" + receInf.Tokki(4) + "あり";
                            }
                        }

                        if (receInf.Tokki(5) != "")
                        {
                            if (bikoKisai == "")
                            {
                                bikoKisai = "特記事項" + receInf.Tokki(5) + "あり";
                            }
                            else
                            {
                                bikoKisai = bikoKisai + "、" + "特記事項" + receInf.Tokki(5) + "あり";
                            }
                        }

                        retDatas.Add(RecordData(receInf));

                        //比較用に退避
                        beforePtId = receInf.PtId;
                        beforeSinYm = receInf.SinYm;
                        beforeHokensyaNo = receInf.HokensyaNo;
                        beforeHonka = receInf.Honka;

                    }
                }

                DateTime seiYm = CIUtil.IntToDate(seikyuYm * 100 + 1);
                string houYm = seiYm.AddMonths(1).ToString("yyyyMM");
                string sheetName = string.Format("{0}_{1}1{2}_{3}", houYm, hpInf.PrefNo.ToString().PadLeft(2, '0'), hpInf.HpCd.PadLeft(7, '0'), DateTime.Now.ToString("yyyyMMdd"));

                return new CommonExcelReportingModel(sheetName + ".xlsx", sheetName, retDatas);
            }
            finally
            {
                _welfareFinder.ReleaseResource();
            }
        }

        #region SubMethod
        string RecordData(CoP44WelfareReceInfModel receInf)
        {
            List<string> colDatas = new List<string>();

            //データ区分
            colDatas.Add("1");
            //報告年月
            DateTime seiYm = CIUtil.IntToDate(seikyuYm * 100 + 1);
            int iYm = CIUtil.DateTimeToInt(seiYm.AddMonths(1));
            string houYm = CIUtil.SDateToWDate(iYm).ToString();
            colDatas.Add(houYm.Substring(0, 5));
            //機関区分
            colDatas.Add("1");
            //医療機関コード
            string hpCd = string.Format("{0}1{1}", hpInf.PrefNo, hpInf.HpCd);
            colDatas.Add(hpCd);
            //医療機関名称
            colDatas.Add("\"" + CIUtil.Copy(hpInf.ReceHpName, 1, 30) + "\"");
            //保険種別
            colDatas.Add(receInf.HokenSbt(receInf.HokenKbn).ToString());
            //保険者番号
            colDatas.Add(receInf.HokensyaNo.PadLeft(8, '0'));
            //事業番号
            colDatas.Add("1");
            //公費負担者番号
            colDatas.Add(receInf.FutansyaNo(kohiHoubetus).PadLeft(8, '0'));
            //公費受給者番号
            colDatas.Add(receInf.JyukyusyaNo(kohiHoubetus).PadLeft(7, '0'));
            //患者カナ氏名(全角)
            string kanKana = CIUtil.Copy(receInf.PtKanaName, 1, 30);
            colDatas.Add("\"" + CIUtil.ToWide(kanKana) + "\"");
            //生年月日
            colDatas.Add(CIUtil.SDateToWDate(receInf.BirthDay).ToString());
            //性別
            colDatas.Add(receInf.Sex.ToString());
            //入院・入院外区分
            colDatas.Add("2");
            //負担割合
            colDatas.Add((receInf.HokenRate / 10).ToString());
            //実日数
            colDatas.Add(receInf.HokenNissu.ToString());
            //保険請求点数
            colDatas.Add(receInf.Tensu.ToString());
            //自己負担支払額
            colDatas.Add(receInf.PtFutan.ToString());
            //食事療養費
            colDatas.Add("");
            //診療年月
            colDatas.Add((CIUtil.SDateToWDate(receInf.SinYm * 100 + 1) / 100).ToString());
            //処方箋発行医療機関等番号
            colDatas.Add("");
            //前月の処方箋に関わる自己負担支払額
            colDatas.Add("");
            //公費番号１
            colDatas.Add(receInf.KohiHoubetu(1));
            //公費番号２
            colDatas.Add(receInf.KohiHoubetu(2));
            //特記事項１
            colDatas.Add(receInf.Tokki(1));
            //特記事項２
            colDatas.Add(receInf.Tokki(2));
            //備考
            if (bikoKisai == "")
            {
                colDatas.Add("");
            }
            else
            {
                colDatas.Add("\"" + bikoKisai + "\"");
            }

            return string.Join(",", colDatas);
        }
        #endregion

        #region Private function
        private bool GetData()
        {
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.IchibuFutan, 0);
            //大分県用のモデルクラスにコピー
            receInfs = wrkReces.Select(x => new CoP44WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
            //患者負担あり、生活保護を併用していない
            receInfs = receInfs.Where(r => r.PtFutan > 0 && !r.IsSeiho).ToList();

            return (receInfs?.Count ?? 0) > 0;

        }
        #endregion
    }
}
