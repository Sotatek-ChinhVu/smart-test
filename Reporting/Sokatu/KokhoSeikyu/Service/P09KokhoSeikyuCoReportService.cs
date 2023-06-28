using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Mapper;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P09KokhoSeikyuCoReportService : IP09KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 9;
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoKokhoSeikyuFinder _kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private string currentHokensyaNo;
        private List<string> hokensyaNos;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        /// <summary>
        /// OutPut Data
        /// </summary>
        private const string _formFileNameP1 = "p09KokhoSeikyuP1.rse";
        private const string _formFileNameP2 = "p09KokhoSeikyuP2.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        #region Constructor and Init
        public P09KokhoSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _visibleFieldData = new();
            _listTextData = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> printHokensyaNos;
        private bool hasNextPage;
        private int currentPage;

        #endregion

        public CommonReportingRequestModel GetP09KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            int indexPage = 1;
            var fileName = new Dictionary<string, string>();
            foreach (string currentNo in hokensyaNos)
            {
                currentPage = 1;
                currentHokensyaNo = currentNo;
                hasNextPage = true;
                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    if (currentPage == 2)
                    {
                        fileName.Add(indexPage.ToString(), _formFileNameP2);
                    }
                    else
                    {
                        fileName.Add(indexPage.ToString(), _formFileNameP1);
                    }
                    currentPage++;
                    indexPage++;
                }
            }
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new P09KokhoSeikyuMapper(_setFieldData, _listTextData, _extralData, fileName, _singleFieldData, _visibleFieldData).GetData();
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //保険者
                Dictionary<string, string> fielDataPerPage = new();
                fielDataPerPage.Add("hokensyaNo", currentHokensyaNo);
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _listTextData.Add(pageIndex, fielDataPerPage);

                if (CurrentPage >= 2) return 1;

                //医療機関情報
                CoRep.SetFieldData("address1", hpInf.Address1);
                CoRep.SetFieldData("address2", hpInf.Address2);
                CoRep.SetFieldData("hpName", hpInf.ReceHpName);
                CoRep.SetFieldData("kaisetuName", hpInf.KaisetuName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                CoRep.SetFieldData("seikyuGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("seikyuYear", wrkYmd.Year);
                CoRep.SetFieldData("seikyuMonth", wrkYmd.Month);
                //提出年月日
                wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                CoRep.SetFieldData("reportGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("reportYear", wrkYmd.Year);
                CoRep.SetFieldData("reportMonth", wrkYmd.Month);
                CoRep.SetFieldData("reportDay", wrkYmd.Day);
                //印
                CoRep.ObjectVisible("inkan", seikyuYm < KaiseiDate.m202210);
                CoRep.ObjectVisible("inkanMaru", seikyuYm < KaiseiDate.m202210);
                CoRep.ObjectVisible("bikoRate9", seikyuYm < KaiseiDate.m202210);
                CoRep.ObjectVisible("bikoIppan", seikyuYm >= KaiseiDate.m202210);

                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                const int maxRow = 7;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 6: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    CoRep.ListText("count", 0, rowNo, wrkData.Count);
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    CoRep.ListText("nissu", 0, rowNo, wrkData.Nissu);
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    CoRep.ListText("tensu", 0, rowNo, wrkData.Tensu);
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                    CoRep.ListText("futan", 0, rowNo, wrkData.Futan);
                }

                return 1;
            }
            #endregion

            #region BodyP2
            int UpdateFormBodyP2()
            {
                var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentHokensyaNo);

                const int maxKohiRow = 4;
                int kohiIndex = (CurrentPage - 2) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                    //法別番号
                    CoRep.ListText("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]);

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    CoRep.ListText("kohiCount", 0, rowNo, wrkData.Count);
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiNissu", 0, rowNo, wrkData.Nissu);
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiTensu", 0, rowNo, wrkData.Tensu);
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiFutan", 0, rowNo, wrkData.Futan);

                    kohiIndex++;
                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                return 1;
            }
            #endregion

            #endregion

            switch (currentPage)
            {
                case 1:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                    {
                        hasNextPage = _hasNextPage;
                        return false;
                    }
                    break;
                default:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                    {
                        hasNextPage = _hasNextPage;
                        return false;
                    }
                    break;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            //保険者番号リストを取得
            hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }
        #endregion
    }
}
