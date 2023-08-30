﻿using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P26KoukiSokatuInCoReportService : IP26KoukiSokatuInCoReportService
    {
        #region Constant
        private const int myPrefNo = 26;
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
        private CoHpInfModel hpInf;
        #endregion

        #region Constructor and Init
        public P26KoukiSokatuInCoReportService(ICoKokhoSokatuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _listTextData = new();
            _extralData = new();
            _visibleFieldData = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private bool hasNextPage;
        private int currentPage;
        #endregion

        private const string _formFileName = "p26KoukiSokatuIn.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.ReceHpCd);
                //医療機関情報
                SetFieldData("postCd", hpInf.PostCdDsp);
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                SetFieldData("hpTel", hpInf.Tel);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
                //印
                SetVisibleFieldData("inkan", seikyuYm < KaiseiDate.m202210);
                SetVisibleFieldData("inkanMaru", seikyuYm < KaiseiDate.m202210);
                SetVisibleFieldData("kbnRate9", seikyuYm < KaiseiDate.m202210);
                SetVisibleFieldData("kbnIppan", seikyuYm >= KaiseiDate.m202210);
                SetVisibleFieldData("bikoRate9", seikyuYm < KaiseiDate.m202210);
                SetVisibleFieldData("bikoIppan", seikyuYm >= KaiseiDate.m202210);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                #region Body
                const int maxRow = 3;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsKoukiIppan).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsKoukiUpper).ToList(); break;
                        case 2: wrkReces = receInfs.ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                    listDataPerPage.Add(new("futan", 0, rowNo, wrkData.Futan.ToString()));

                    //他
                    int prefOutKohiCnt = wrkReces.Where(r => r.IsPrefOutKohi).Count();
                    listDataPerPage.Add(new("prefOutKohiCnt", 0, rowNo, prefOutKohiCnt.ToString()));
                    //長
                    int chokiCnt = wrkReces.Where(r => r.IsChoki).Count();
                    listDataPerPage.Add(new("chokiCnt", 0, rowNo, chokiCnt.ToString()));
                }

                //一部負担金減免・猶予
                var menReces = receInfs.Where(r =>
                    new int[] { GenmenKbn.Menjyo, GenmenKbn.Yuyo }.Contains(r.GenmenKbn)
                ).ToList();

                Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                //fieldDataPerPage.Add("genmenCnt", menReces.Count.ToString());
                fieldDataPerPage.Add("genmenCnt", menReces.Sum(r => r.Tensu).ToString());

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }
                #endregion
                _listTextData.Add(pageIndex, listDataPerPage);
                return 1;
            }
            #endregion

            #endregion

            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kouki, PrefKbn.PrefIn, myPrefNo, HokensyaNoKbn.SumAll);

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion

        public CommonReportingRequestModel GetP26KoukiSokatuInReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            if (getData)
            {
                currentPage = 1;
                hasNextPage = true;

                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
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
    }
}