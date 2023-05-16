using Helper.Common;
using Reporting.Calculate.Constants;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P11KokhoSokatuCoReportService : IP11KokhoSokatuCoReportService
    {
        #region Constant
        private const int MyPrefNo = 11;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSokatuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoReceInfModel> receInfs;
        private List<CoReceInfModel> tokuyohiReceInfs;
        private CoHpInfModel hpInf;
        private List<CoKaMstModel> kaMsts;

        private int currentKbnIndex;
        private int currentHokIndex;
        private int currentPrefIndex;
        private int currentKohiIndex;
        private List<CoHokensyaMstModel> hokensyaNames;
        countData kohiTotalData = new countData();
        countData totalData = new countData();

        private bool _hasNextPage;
        private int _hpId;
        private int _seikyuYm;
        private int _currentPage;
        #endregion

        private bool UpdateDrawForm()
        {
            _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //請求年
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                CoRep.SetFieldData("seikyuGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("seikyuYear", wrkYmd.Year);

                for (int i = 1; i <= 2; i++)
                {
                    //医療機関コード
                    CoRep.SetFieldData($"hpCode_{i}", hpInf.HpCd);
                    //医療機関情報
                    CoRep.SetFieldData($"address1_{i}", hpInf.Address1);
                    CoRep.SetFieldData($"address2_{i}", hpInf.Address2);
                    CoRep.SetFieldData($"hpName_{i}", hpInf.ReceHpName);
                    CoRep.SetFieldData($"kaisetuName_{i}", hpInf.KaisetuName);
                    CoRep.SetFieldData($"hpTel_{i}", hpInf.Tel);
                    //請求月
                    CoRep.SetFieldData($"seikyuMonth_{i}", wrkYmd.Month);
                }

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                const int maxRow = 17;
                const int maxKbnIndex = 9;

                short rowNo = 0;
                if (_currentPage >= 2)
                {
                    rowNo = 2;
                }

                //集計
                for (int kbnIndex = currentKbnIndex; kbnIndex < maxKbnIndex; kbnIndex++)
                {
                    currentKbnIndex = kbnIndex;

                    if (rowNo >= maxRow)
                    {
                        _hasNextPage = true;
                        break;
                    }

                    List<CoReceInfModel> curReceInfs = null;
                    switch (kbnIndex)
                    {
                        //後期高齢者
                        case 0: curReceInfs = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        //退職者
                        case 1: curReceInfs = receInfs.Where(r => r.IsRetAll).ToList(); break;
                        //特別療養費（一般）
                        case 2: curReceInfs = tokuyohiReceInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                        //特別療養費（後期）
                        case 3: curReceInfs = tokuyohiReceInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        //一般（県内）
                        case 4: curReceInfs = receInfs.Where(r => r.IsPrefIn && (r.IsNrAll || r.IsRetAll)).ToList(); break;
                        //一般（県外）
                        case 5: curReceInfs = receInfs.Where(r => !r.IsPrefIn && (r.IsNrAll || r.IsRetAll)).ToList(); break;
                        //国保計
                        case 6: curReceInfs = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                        //公費再掲
                        case 7: curReceInfs = receInfs.Where(r => r.IsHeiyo).ToList(); break;
                        //公費計
                        case 8: curReceInfs = receInfs.Where(r => r.IsHeiyo).ToList(); break;
                    }
                    if (curReceInfs == null || (kbnIndex >= 2 && curReceInfs.Count == 0)) continue;

                    //
                    switch (kbnIndex)
                    {
                        #region 4:一般（県内）
                        case 4:
                            //保険者ごとにまとめる
                            var hokensyaNos = curReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

                            for (int hokIndex = currentHokIndex; hokIndex < hokensyaNos.Count; hokIndex++)
                            {
                                currentHokIndex = hokIndex;
                                if (rowNo >= maxRow)
                                {
                                    _hasNextPage = true;
                                    break;
                                }

                                var wrkReces = curReceInfs.Where(r => r.HokensyaNo == hokensyaNos[hokIndex]).ToList();

                                //保険者名
                                //CoRep.ListText("hokensyaName", 0, rowNo, hokensyaNames.Find(h => h.HokensyaNo == hokensyaNos[hokIndex])?.Name ?? hokensyaNos[hokIndex]);
                                CoRep.ListText("hokensyaName", 0, rowNo, hokensyaNos[hokIndex]);

                                countData wrkHokData = new countData();
                                //件数
                                wrkHokData.Count = wrkReces.Count;
                                CoRep.ListText("count", 0, rowNo, wrkHokData.Count);
                                //日数
                                wrkHokData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                                CoRep.ListText("nissu", 0, rowNo, wrkHokData.Nissu);
                                //点数
                                wrkHokData.Tensu = wrkReces.Sum(r => r.Tensu);
                                CoRep.ListText("tensu", 0, rowNo, wrkHokData.Tensu);
                                //請求額払の金額
                                wrkHokData.Futan = wrkReces.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                CoRep.ListText("futan", 0, rowNo, wrkHokData.Futan);

                                rowNo++;
                            }
                            break;
                        #endregion
                        #region 5:一般（県外）
                        case 5:
                            //都道府県ごとにまとめる
                            var prefNos = curReceInfs.GroupBy(r => r.PrefNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

                            for (int prefIndex = currentPrefIndex; prefIndex < prefNos.Count; prefIndex++)
                            {
                                currentPrefIndex = prefIndex;
                                if (rowNo >= maxRow)
                                {
                                    _hasNextPage = true;
                                    break;
                                }

                                var wrkReces = curReceInfs.Where(r => r.PrefNo == prefNos[prefIndex]).ToList();

                                countData wrkPrefData = new countData();
                                //都道府県名
                                CoRep.ListText("hokensyaName", 0, rowNo, PrefCode.PrefName(prefNos[prefIndex]));
                                //件数
                                wrkPrefData.Count = wrkReces.Count;
                                CoRep.ListText("count", 0, rowNo, wrkPrefData.Count);
                                //日数
                                wrkPrefData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                                CoRep.ListText("nissu", 0, rowNo, wrkPrefData.Nissu);
                                //点数
                                wrkPrefData.Tensu = wrkReces.Sum(r => r.Tensu);
                                CoRep.ListText("tensu", 0, rowNo, wrkPrefData.Tensu);
                                //請求額払の金額
                                wrkPrefData.Futan = wrkReces.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                CoRep.ListText("futan", 0, rowNo, wrkPrefData.Futan);

                                rowNo++;
                            }

                            break;
                        #endregion
                        #region 7:公費再掲
                        case 7:
                            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs, null);

                            for (int kohiIndex = currentKohiIndex; kohiIndex < kohiHoubetus.Count; kohiIndex++)
                            {
                                currentKohiIndex = kohiIndex;
                                if (rowNo >= maxRow)
                                {
                                    _hasNextPage = true;
                                    break;
                                }

                                var wrkReces = curReceInfs.Where(r => r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                                countData wrkKohiData = new countData();
                                //法別番号
                                CoRep.ListText("hokensyaName", 0, rowNo, kohiHoubetus[kohiIndex]);
                                //件数
                                wrkKohiData.Count = wrkReces.Count;
                                CoRep.ListText("count", 0, rowNo, wrkKohiData.Count);
                                //日数
                                wrkKohiData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                                CoRep.ListText("nissu", 0, rowNo, wrkKohiData.Nissu);
                                //点数
                                wrkKohiData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                                CoRep.ListText("tensu", 0, rowNo, wrkKohiData.Tensu);
                                //請求額払の金額
                                wrkKohiData.Futan = wrkReces.Sum(r => r.KohiFutan(kohiHoubetus[kohiIndex]));
                                CoRep.ListText("futan", 0, rowNo, wrkKohiData.Futan);

                                kohiTotalData.AddValue(wrkKohiData);

                                rowNo++;
                            }

                            break;
                        #endregion
                        #region 8:公費計
                        case 8:
                            CoRep.ListText("hokensyaName", 0, rowNo, "公費計");
                            //件数
                            CoRep.ListText("count", 0, rowNo, kohiTotalData.Count);
                            //日数
                            CoRep.ListText("nissu", 0, rowNo, kohiTotalData.Nissu);
                            //点数
                            CoRep.ListText("tensu", 0, rowNo, kohiTotalData.Tensu);
                            //請求額払の金額
                            CoRep.ListText("futan", 0, rowNo, kohiTotalData.Futan);

                            break;
                        #endregion
                        #region その他
                        default:
                            countData wrkData = new countData();

                            //件数
                            wrkData.Count = curReceInfs.Count;
                            CoRep.ListText("count", 0, rowNo, wrkData.Count);
                            //日数
                            wrkData.Nissu = curReceInfs.Sum(r => r.HokenNissu);
                            CoRep.ListText("nissu", 0, rowNo, wrkData.Nissu);
                            //点数
                            wrkData.Tensu = curReceInfs.Sum(r => r.Tensu);
                            CoRep.ListText("tensu", 0, rowNo, wrkData.Tensu);
                            //請求額払の金額
                            if (kbnIndex >= 4)
                            {
                                wrkData.Futan = curReceInfs.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                CoRep.ListText("futan", 0, rowNo, wrkData.Futan);
                            }

                            if (new int[] { 0, 1, 6 }.Contains(kbnIndex))
                            {
                                totalData.AddValue(wrkData);
                            }

                            switch (kbnIndex)
                            {
                                case 2:
                                    CoRep.ListText("hokensyaName", 0, rowNo, "特別療養費");
                                    break;
                                case 3:
                                    CoRep.ListText("hokensyaName", 0, rowNo, "特別療養費(後期分)");
                                    break;
                                case 6:
                                    CoRep.ListText("hokensyaName", 0, rowNo, "国保計");
                                    break;
                            }
                            rowNo++;

                            break;
                            #endregion
                    }

                    if (_hasNextPage)
                    {
                        break;
                    }
                }

                //最終ページのみ記載する
                if (!_hasNextPage)
                {
                    //合計 - 件数
                    CoRep.ListText("count", 0, maxRow, totalData.Count);
                    //合計 - 日数
                    CoRep.ListText("nissu", 0, maxRow, totalData.Nissu);
                    //合計 - 点数
                    CoRep.ListText("tensu", 0, maxRow, totalData.Tensu);
                    //合計 - 請求額払の金額
                    CoRep.ListText("futan", 0, maxRow, totalData.Futan + kohiTotalData.Futan);

                    //平均点数
                    var avgTensu = CIUtil.RoundoffNum((double)totalData.Tensu / totalData.Count, 2);
                    CoRep.SetFieldData("avgTensu", avgTensu);

                    //国民健康保険及び公費請求額払票 - 請求額払の金額
                    CoRep.SetFieldData("totalFutan", string.Format("{0, 9}", totalData.Futan + kohiTotalData.Futan));
                }

                return 1;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
            kaMsts = _kokhoFinder.GetKaMst(_hpId);
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(_hpId, receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList());

            //特別療養費
            tokuyohiReceInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Tokuyohi, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);

            return (receInfs?.Count ?? 0) > 0;
        }
    }
}
