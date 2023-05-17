using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P13KokhoSokatuCoReportService : IP13KokhoSokatuCoReportService
    {
        #region Constant
        private const int MyPrefNo = 13;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKokhoSokatuFinder _kokhoFinder;
        private readonly ISystemConfig _systemConfig;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        private bool _hasNextPage;
        private int _hpId;
        private int _seikyuYm;
        private int _currentPage;
        private SeikyuType _seikyuType;

        /// <summary>
        /// OutPut Data
        /// </summary>
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        private readonly Dictionary<string, string> _fileNamePageMap = new Dictionary<string, string>();
        private readonly string _rowCountFieldName = string.Empty;
        private readonly int _reportType = (int)CoReportType.KokhoSokatu;

        #endregion
        public P13KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder, ISystemConfig systemConfig)
        {
            _kokhoFinder = kokhoFinder;
            _systemConfig = systemConfig;
        }

        public CommonReportingRequestModel GetP13KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
            _seikyuType = seikyuType;
            var getData = GetData();

            //専用紙タイプの場合は枠を非表示
            _visibleFieldData.Add("Frame", _systemConfig.P13KokhoSokatuType() == 0);
            //紙請求の場合は電子媒体欄を非表示
            _visibleFieldData.Add("FrameDisk", diskKind != -1);

            _hasNextPage = true;
            _currentPage = 1;

            while (getData && _hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }

            _extralData.Add("maxRow", "8");
            _extralData.Add("totalPage", (_currentPage - 1).ToString());
            _fileNamePageMap.Add("1", "p13KokhoSokatu.rse");
            return new KokhoSokatuMapper(_singleFieldData, _tableFieldData, _extralData, _visibleFieldData, _fileNamePageMap, _rowCountFieldName, _reportType).GetData();
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.ReceHpCd);
                //医療機関情報
                SetFieldData("postCd1", CIUtil.Copy(hpInf.PostCd, 1, 3));
                SetFieldData("postCd2", hpInf.PostCd.Length == 7 ? CIUtil.Copy(hpInf.PostCd, 4, 4) : CIUtil.Copy(hpInf.PostCd, 5, 4));
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                SetFieldData("hpTel", string.Format("({0})", hpInf.Tel));
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
                //印
                SetVisibleFieldData("inkan", _seikyuYm < KaiseiDate.m202210);
                SetVisibleFieldData("inkanMaru", _seikyuYm < KaiseiDate.m202210);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                const int maxRow = 8;
                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var rowNoKey = string.Empty;
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = receInfs.Where(r => r.IsNrAll && r.IsPrefIn).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsRetAll && r.IsPrefIn).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.IsNrAll && !r.IsPrefIn).ToList(); break;
                        case 4: wrkReces = receInfs.Where(r => r.IsRetAll && !r.IsPrefIn).ToList(); break;
                        //後期
                        case 6: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                        case 7: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                        default:
                            //計
                            rowNoKey = rowNo + "_" + _currentPage;
                            _extralData.Add("count_0_" + rowNoKey, totalData.Count.ToString());
                            _extralData.Add("tensu_0_" + rowNoKey, totalData.Tensu.ToString());
                            _extralData.Add("kohiCount_0_" + rowNoKey, totalData.KohiCount.ToString());
                            totalData.Clear();
                            break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    rowNoKey = rowNo + "_" + _currentPage;
                    _extralData.Add("count_0_" + rowNoKey, wrkData.Count.ToString());
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    _extralData.Add("tensu_0_" + rowNoKey, wrkData.Tensu.ToString());
                    //公費併用件数
                    wrkData.KohiCount = wrkReces.Where(r => r.IsHeiyo).ToList().Count;
                    _extralData.Add("kohiCount_0_" + rowNoKey, wrkData.KohiCount.ToString());

                    //計
                    totalData.AddValue(wrkData);
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

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);

            return (receInfs?.Count ?? 0) > 0;
        }
    }
}
