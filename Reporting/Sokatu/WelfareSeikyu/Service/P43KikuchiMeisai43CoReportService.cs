using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P43KikuchiMeisai43CoReportService : IP43KikuchiMeisai43CoReportService
    {
        #region Constant
        private const int myPrefNo = 43;
        private List<int> kohiHokenNos = new List<int> { 143 };
        private List<int> kohiHokenEdaNos = new List<int> { 0, 4 };

        /// <summary>
        /// ページタイプ
        /// 国保と社保でページを分けて印刷する
        /// </summary>
        private enum pageType
        {
            Kokuho,
            Syaho
        }
        private struct countData
        {
            public int Tensu;
            public int Seikyu;
            public int Futan;
        }
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP43WelfareReceInfModel> receInfs = new();
        private List<CoP43WelfareReceInfModel> curReceInfs = new();
        private CoHpInfModel hpInf = new();
        private pageType curPgType = 0;
        private List<pageType> tgtPgType = new List<pageType>();
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p43KikuchiMeisai43.rse";

        #region Constructor and Init
        public P43KikuchiMeisai43CoReportService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private int currentPage;
        private bool hasNextPage;
        #endregion

        public CommonReportingRequestModel GetP43KikuchiMeisai43ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();

                if (getData)
                {
                    foreach (pageType pgType in Enum.GetValues(typeof(pageType)))
                    {
                        // 国保/社保に分けて作成
                        curPgType = pgType;

                        switch (curPgType)
                        {
                            case pageType.Kokuho:
                                curReceInfs = receInfs.Where(r => r.HokenKbn == HokenKbn.Kokho).ToList();
                                break;
                            case pageType.Syaho:
                                curReceInfs = receInfs.Where(r => r.HokenKbn == HokenKbn.Syaho).ToList();
                                break;
                            default:
                                break;
                        }
                        if (curReceInfs == null || curReceInfs.Count == 0)
                        {
                            continue;
                        }

                        currentPage = 1;
                        hasNextPage = true;

                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            currentPage++;
                        }
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new WelfareSeikyuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
            }
            finally
            {
                _welfareFinder.ReleaseResource();
            }
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
                //国保・社保に丸付け
                switch (curPgType)
                {
                    case pageType.Kokuho: SetVisibleFieldData("cirKokuho", true); break;
                    case pageType.Syaho: SetVisibleFieldData("cirSyaho", true); break;
                    default: break;
                }

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                const int maxRow = 14;
                int ptIndex = (currentPage - 1) * maxRow;
                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var wrkRece = curReceInfs[ptIndex];

                    //受給者証番号
                    listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, wrkRece.TokusyuNo(kohiHokenNos)));
                    //氏名
                    listDataPerPage.Add(new("ptName", 0, rowNo, wrkRece.PtName));
                    //生年月日(g ee・MM・dd)
                    string wYmd = CIUtil.SDateToShowWDate(wrkRece.BirthDay);
                    wYmd = wYmd.Replace("/", "・");
                    listDataPerPage.Add(new("birthday", 0, rowNo, wYmd));
                    //住所
                    listDataPerPage.Add(new("homeAddress", 0, rowNo, wrkRece.HomeAddress1 + wrkRece.HomeAddress2));
                    //総点数
                    listDataPerPage.Add(new("tensu", 0, rowNo, wrkRece.Tensu.ToString()));
                    totalData.Tensu += wrkRece.Tensu;
                    //一部負担額　※１の位四捨五入
                    int futan = CIUtil.RoundInt(wrkRece.HokenIchibuFutan, 1);
                    listDataPerPage.Add(new("futan", 0, rowNo, futan.ToString()));
                    totalData.Futan += futan;
                    //請求金額 = 一部負担額 × ３分の２　※小数点以下切り捨て
                    int seikyu = (int)CIUtil.RoundDown(futan * 2 / 3, 1);
                    listDataPerPage.Add(new("seikyu", 0, rowNo, seikyu.ToString()));
                    totalData.Seikyu += seikyu;

                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //ページ計
                //総点数
                listDataPerPage.Add(new("tensu", 0, maxRow, totalData.Tensu.ToString()));
                //一部負担額
                listDataPerPage.Add(new("futan", 0, maxRow, totalData.Futan.ToString()));
                //請求金額
                listDataPerPage.Add(new("seikyu", 0, maxRow, totalData.Seikyu.ToString()));
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
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.KohiFutan, -1);
            //熊本県用のモデルにコピー
            var wrkReces2 = wrkReces.Select(x => new CoP43WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos));
            //菊池市ひとり親医療費の対象に絞る
            receInfs = wrkReces2.Where(x => kohiHokenEdaNos.Contains(x.KohiHokenEdaNo(kohiHokenNos)) && x.TokusyuNo(kohiHokenNos)?.Length == 8)
                .OrderBy(r => r.TokusyuNo(kohiHokenNos)).ToList();
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
