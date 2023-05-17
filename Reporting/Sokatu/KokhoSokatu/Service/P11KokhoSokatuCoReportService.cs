using Helper.Common;
using Reporting.Calculate.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P11KokhoSokatuCoReportService : IP11KokhoSokatuCoReportService
    {
        #region Constant
        private const int myPrefNo = 11;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKokhoSokatuFinder _kokhoFinder;

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
        private SeikyuType _seikyuType;

        /// <summary>
        /// OutPut Data
        /// </summary>
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        private readonly Dictionary<string, string> _fileNamePageMap = new Dictionary<string, string>();
        private readonly string _rowCountFieldName = string.Empty;
        private readonly int _reportType = (int)CoReportType.KokhoSokatu;
        #endregion

        public P11KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
        }

        public CommonReportingRequestModel GetP11KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
            _seikyuType = seikyuType;

            currentKbnIndex = 0;
            currentHokIndex = 0;
            currentPrefIndex = 0;
            currentKohiIndex = 0;

            var getData = GetData();

            _hasNextPage = true;
            _currentPage = 1;

            AddFileNamePageMap();

            while (getData && _hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }

            _extralData.Add("maxRow", "17");
            _extralData.Add("totalPage", (_currentPage - 1).ToString());
            return new KokhoSokatuMapper(_singleFieldData, _tableFieldData, _extralData, _fileNamePageMap, _rowCountFieldName, _reportType).GetData();
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //請求年
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());

                for (int i = 1; i <= 2; i++)
                {
                    //医療機関コード
                    SetFieldData($"hpCode_{i}", hpInf.HpCd);
                    //医療機関情報
                    SetFieldData($"address1_{i}", hpInf.Address1);
                    SetFieldData($"address2_{i}", hpInf.Address2);
                    SetFieldData($"hpName_{i}", hpInf.ReceHpName);
                    SetFieldData($"kaisetuName_{i}", hpInf.KaisetuName);
                    SetFieldData($"hpTel_{i}", hpInf.Tel);
                    //請求月
                    SetFieldData($"seikyuMonth_{i}", wrkYmd.Month.ToString());
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

                            var rowNokey = string.Empty;
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
                                rowNokey = rowNo + "_" + _currentPage;

                                _extralData.Add("hokensyaName_0_" + rowNokey, hokensyaNos[hokIndex]);

                                countData wrkHokData = new countData();
                                //件数
                                wrkHokData.Count = wrkReces.Count;
                                _extralData.Add("count_0_" + rowNokey, wrkHokData.Count.ToString());
                                //日数
                                wrkHokData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                                _extralData.Add("nissu_0_" + rowNokey, wrkHokData.Nissu.ToString());
                                //点数
                                wrkHokData.Tensu = wrkReces.Sum(r => r.Tensu);
                                _extralData.Add("tensu_0_" + rowNokey, wrkHokData.Tensu.ToString());
                                //請求額払の金額
                                wrkHokData.Futan = wrkReces.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                _extralData.Add("futan_0_" + rowNokey, wrkHokData.Futan.ToString());

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
                                rowNokey = rowNo + "_" + _currentPage;
                                //都道府県名
                                _extralData.Add("hokensyaName_0_" + rowNokey, PrefCode.PrefName(prefNos[prefIndex]));
                                //件数
                                wrkPrefData.Count = wrkReces.Count;
                                _extralData.Add("count_0_" + rowNokey, wrkPrefData.Count.ToString());
                                //日数
                                wrkPrefData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                                _extralData.Add("nissu_0_" + rowNokey, wrkPrefData.Nissu.ToString());
                                //点数
                                wrkPrefData.Tensu = wrkReces.Sum(r => r.Tensu);
                                _extralData.Add("tensu_0_" + rowNokey, wrkPrefData.Tensu.ToString());
                                //請求額払の金額
                                wrkPrefData.Futan = wrkReces.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                _extralData.Add("futan_0_" + rowNokey, wrkPrefData.Futan.ToString());

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
                                rowNokey = rowNo + "_" + _currentPage;
                                //法別番号
                                _extralData.Add("hokensyaName_0_" + rowNokey, kohiHoubetus[kohiIndex]);
                                //件数
                                wrkKohiData.Count = wrkReces.Count;
                                _extralData.Add("count_0_" + rowNokey, wrkKohiData.Count.ToString());
                                //日数
                                wrkKohiData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                                _extralData.Add("nissu_0_" + rowNokey, wrkKohiData.Nissu.ToString());
                                //点数
                                wrkKohiData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                                _extralData.Add("tensu_0_" + rowNokey, wrkKohiData.Tensu.ToString());
                                //請求額払の金額
                                wrkKohiData.Futan = wrkReces.Sum(r => r.KohiFutan(kohiHoubetus[kohiIndex]));
                                _extralData.Add("futan_0_" + rowNokey, wrkKohiData.Futan.ToString());

                                kohiTotalData.AddValue(wrkKohiData);

                                rowNo++;
                            }

                            break;
                        #endregion
                        #region 8:公費計
                        case 8:
                            rowNokey = rowNo + "_" + _currentPage;
                            _extralData.Add("hokensyaName_0_" + rowNokey, "公費計");
                            //件数
                            _extralData.Add("count_0_" + rowNokey, kohiTotalData.Count.ToString());
                            //日数
                            _extralData.Add("nissu_0_" + rowNokey, kohiTotalData.Nissu.ToString());
                            //点数
                            _extralData.Add("tensu_0_" + rowNokey, kohiTotalData.Tensu.ToString());
                            //請求額払の金額
                            _extralData.Add("futan_0_" + rowNokey, kohiTotalData.Futan.ToString());

                            break;
                        #endregion
                        #region その他
                        default:
                            countData wrkData = new countData();
                            //件数
                            rowNokey = rowNo + "_" + _currentPage;

                            wrkData.Count = curReceInfs.Count;
                            _extralData.Add("count_0_" + rowNokey, wrkData.Count.ToString());
                            //日数
                            wrkData.Nissu = curReceInfs.Sum(r => r.HokenNissu);
                            _extralData.Add("nissu_0_" + rowNokey, wrkData.Nissu.ToString());
                            //点数
                            wrkData.Tensu = curReceInfs.Sum(r => r.Tensu);
                            _extralData.Add("tensu_0_" + rowNokey, wrkData.Tensu.ToString());
                            //請求額払の金額
                            if (kbnIndex >= 4)
                            {
                                wrkData.Futan = curReceInfs.Sum(r => r.Tensu * (100 - r.HokenRate) / 10);
                                _extralData.Add("futan_0_" + rowNokey, wrkData.Futan.ToString());
                            }

                            if (new int[] { 0, 1, 6 }.Contains(kbnIndex))
                            {
                                totalData.AddValue(wrkData);
                            }

                            switch (kbnIndex)
                            {
                                case 2:
                                    _extralData.Add("hokensyaName_0_" + rowNokey, "特別療養費");
                                    break;
                                case 3:
                                    _extralData.Add("hokensyaName_0_" + rowNokey, "特別療養費(後期分)");
                                    break;
                                case 6:
                                    _extralData.Add("hokensyaName_0_" + rowNokey, "国保計");
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
                    var rowNokey = maxRow + "_" + _currentPage;
                    //合計 - 件数
                    _extralData.Add("count_0_" + rowNokey, totalData.Count.ToString());
                    //合計 - 日数
                    _extralData.Add("nissu_0_" + rowNokey, totalData.Nissu.ToString());
                    //合計 - 点数
                    _extralData.Add("tensu_0_" + rowNokey, totalData.Tensu.ToString());
                    //合計 - 請求額払の金額
                    _extralData.Add("futan_0_" + rowNokey, (totalData.Futan + kohiTotalData.Futan).ToString());

                    //平均点数
                    var avgTensu = CIUtil.RoundoffNum((double)totalData.Tensu / totalData.Count, 2);

                    _extralData.Add("avgTensu", avgTensu.ToString());

                    //国民健康保険及び公費請求額払票 - 請求額払の金額
                    _extralData.Add("totalFutan", string.Format("{0, 9}", totalData.Futan + kohiTotalData.Futan));
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
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(_hpId, receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList());

            //特別療養費
            tokuyohiReceInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.Tokuyohi, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }

        private void AddFileNamePageMap()
        {
            _fileNamePageMap.Add("1", "p11KokhoSokatu.rse");
        }
    }
}
