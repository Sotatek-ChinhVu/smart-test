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
    public class P46WelfareSeikyu99CoReportService : IP46WelfareSeikyu99CoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "99" };
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoWelfareReceInfModel> receInfs = new();
        private CoWelfareReceInfModel curReceInf = new();
        private CoHpInfModel hpInf = new();
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
        #endregion

        #region Constructor and Init
        public P46WelfareSeikyu99CoReportService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
            _reportConfigPerPage = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private bool hasNextPage;
        private int currentPage;
        #endregion

        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関情報
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("hpTel", hpInf.Tel);
                SetFieldData("kaisetuName", hpInf.KaisetuName);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                //市町村番号
                //二桁以下の時は0埋め二桁、二桁より多いときは下二桁を出力
                string townNo = curReceInf.JyukyusyaNo(kohiHoubetus)?.PadLeft(2, '0') ?? string.Empty;
                Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();
                pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                fieldDataPerPage.Add("cityCode", townNo.Substring(townNo.Length - 2));
                //受給者番号
                //九桁以下の時は0埋め九桁、九桁より多いときは下九桁を出力
                string jukyusyaNo = curReceInf.TokusyuNo(kohiHoubetus).PadLeft(9, '0');
                fieldDataPerPage.Add("jyukyusyaNo", jukyusyaNo.Substring(jukyusyaNo.Length - 9));
                //カナ氏名
                fieldDataPerPage.Add("kanaName", curReceInf.KanaName);
                //氏名
                fieldDataPerPage.Add("ptName", curReceInf.PtName);
                //生年月日
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.BirthDay);
                fieldDataPerPage.Add("birthGengo", wrkYmd.Gengo);
                fieldDataPerPage.Add("birthYear", wrkYmd.Year.ToString("D2"));
                fieldDataPerPage.Add("birthMonth", wrkYmd.Month.ToString("D2"));
                fieldDataPerPage.Add("birthDay", wrkYmd.Day.ToString("D2"));
                //保険区分
                ReportConfigModel ConfigDataPerPage = _reportConfigPerPage.ContainsKey(pageIndex) ? _reportConfigPerPage[pageIndex] : new();
                ConfigDataPerPage.VisibleFieldList.Add("hokenKbnShaho", curReceInf.HokenKbn == HokenKbn.Syaho);
                ConfigDataPerPage.VisibleFieldList.Add("hokenKbnKokuho", curReceInf.HokenKbn == HokenKbn.Kokho);
                //保険者番号
                fieldDataPerPage.Add("hokensyaNo", curReceInf.HokensyaNo.PadLeft(8, ' '));
                //保険負担割合
                ConfigDataPerPage.VisibleFieldList.Add("rate2", curReceInf.HokenRate == 20);
                ConfigDataPerPage.VisibleFieldList.Add("rate3", curReceInf.HokenRate == 30);
                
                if ((curReceInf.HokenRate != 20) && (curReceInf.HokenRate != 30))
                {
                    ConfigDataPerPage.VisibleFieldList.Add("rateElse", true);
                    string hokenRate = (curReceInf.HokenRate / 10).ToString() + "割";
                    fieldDataPerPage.Add("hokenRate", hokenRate);
                }
                else
                {
                    ConfigDataPerPage.VisibleFieldList.Add("rateElse", false);
                }

                if (!_reportConfigPerPage.ContainsKey(pageIndex))
                {
                    _reportConfigPerPage.Add(pageIndex, ConfigDataPerPage);
                }

                //保険診療合計点数
                fieldDataPerPage.Add("tensu", string.Format("{0, 6}", curReceInf.Tensu));
                //自己負担支払額
                fieldDataPerPage.Add("futan", string.Format("{0, 6}", curReceInf.PtFutan));
                //診療年月
                wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                fieldDataPerPage.Add("sinGengo", wrkYmd.Gengo);
                fieldDataPerPage.Add("sinYear", wrkYmd.Year.ToString("D2"));
                fieldDataPerPage.Add("sinMonth", wrkYmd.Month.ToString("D2"));

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }
                //公費番号(公費番号99以外の併用がある場合)
                const int maxKohi = 4, maxRow = 2;
                short kohi = 1, row = 0;
                while ((kohi <= maxKohi) && (row < maxRow))
                {
                    if ((!string.IsNullOrEmpty(curReceInf.KohiHoubetu(kohi))) && (!kohiHoubetus.Contains(curReceInf.KohiHoubetu(kohi))) && (curReceInf.KohiReceKisai(kohi) == 1))
                    {
                        listDataPerPage.Add(new("kohiHoubetu", 0, row, curReceInf.KohiHoubetu(kohi)));
                        row++;
                    }
                    kohi++;
                }
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
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.None, 0);
            //自己負担額がある人のみ
            //第一ソート市町村番号順(JyukyusyaNo)、第二ソート受給者番号順(TokusyuNo)
            receInfs = wrkReces.Where(r => r.PtFutan > 0).OrderBy(r => r.JyukyusyaNo(kohiHoubetus)).ThenBy(r => r.TokusyuNo(kohiHoubetus)).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion

        public CommonReportingRequestModel GetP46WelfareSeikyu99ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                var getData = GetData();
                string formFile = "p46WelfareSeikyu99.rse";

                if (getData)
                {
                    foreach (var receinf in receInfs)
                    {
                        curReceInf = receinf;
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
                return new P26WelfareSeikyuMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, formFile, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
            }
            finally
            {
                _welfareFinder.ReleaseResource();
            }
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
    }
}
