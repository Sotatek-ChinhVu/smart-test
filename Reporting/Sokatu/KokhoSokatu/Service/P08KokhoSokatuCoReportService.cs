using Helper.Common;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P08KokhoSokatuCoReportService
    {
        #region Constructor and Init

        private const int MyPrefNo = 8;
        private int _hpId;
        private int _seikyuYm;
        private SeikyuType _seikyuType;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        /// <summary>
        /// OutPut Data
        /// </summary>
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        private readonly Dictionary<string, string> _fileNamePageMap;
        private readonly int _reportType = (int)CoReportType.P08KokhoSokatu;

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKokhoSokatuFinder _kokhoFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;


        public P08KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder, IReadRseReportFileService readRseReportFileService)
        {
            _kokhoFinder = kokhoFinder;
            _readRseReportFileService = readRseReportFileService;
        }
        #endregion

        public CommonReportingRequestModel GetP08KokhoSokatuReportingData(int hpId, int seikyuYm)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
            return (receInfs?.Count ?? 0) > 0;
        }

        private bool UpdateDrawForm(out bool hasNextPage)
        {
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                //医療機関情報
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.AsString());
                SetFieldData("seikyuMonth", wrkYmd.Month.AsString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                const int maxRow = 7;

                //福祉
                List<string> prefInHoubetus = new List<string> { "60", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "94", "95" };
                //全国公費
                var prefAllHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), prefInHoubetus);

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    Dictionary<string, CellModel> data = new();

                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsNrAll).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsRetAll).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo).ToList(); break;
                        case 4: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        case 5: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo).ToList(); break;
                        case 6: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    if (rowNo == 2)
                    {
                        //福祉                        
                        foreach (var houbetu in prefInHoubetus)
                        {
                            wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                            wrkData.Count += wrkReces.Count;
                            wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                        }
                    }
                    else if (rowNo == 3)
                    {
                        //全国公費
                        foreach (var houbetu in prefAllHoubetus)
                        {
                            wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                            wrkData.Count += wrkReces.Count;
                            wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                        }
                    }
                    else if (rowNo == 5)
                    {
                        //福祉
                        foreach (var houbetu in prefInHoubetus)
                        {
                            wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                            wrkData.Count += wrkReces.Count;
                            wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                        }
                    }
                    else if (rowNo == 6)
                    {
                        //全国公費
                        foreach (var houbetu in prefAllHoubetus)
                        {
                            wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsHeiyo && r.IsKohi(houbetu)).ToList();
                            wrkData.Count += wrkReces.Count;
                            wrkData.Tensu += wrkReces.Sum(r => r.KohiReceTensu(houbetu));
                        }
                    }
                    else
                    {
                        wrkData.Count = wrkReces.Count;
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    }

                    //件数
                    AddListData(ref data, "count", wrkData.Count.AsString());
                    //点数                   
                    AddListData(ref data, "tensu", wrkData.Tensu.AsString());

                    _tableFieldData.Add(data);
                }

                return 1;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            catch (Exception e)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
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
            _fileNamePageMap.Add("1", "p08KokhoSokatu.rse");
        }

    }
}
