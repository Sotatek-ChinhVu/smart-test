using Helper.Common;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P22WelfareSeikyuCoReportService : IP22WelfareSeikyuCoReportService
{
    #region Constant
    private const int myPrefNo = 22;
    private List<string> kohiHoubetus;

    private struct countData
    {
        public int RecordCount;
        public int Nissu;
        public int Tensu;
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
    private List<CoP22WelfareReceInfModel> receInfs = new();
    private CoHpInfModel hpInf;

    private List<(int sinym, string code, string name)> cityNames = new();
    private int currentSinYm;
    private string currentFutansyaNo = "";
    private string currentCityName = "";
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;

    countData totalData = new countData();
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private string _formFileName = "p22WelfareSeikyu83.rse";

    #region Constructor and Init
    public P22WelfareSeikyuCoReportService(ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
        hpInf = new();
        receInfs = new();
        hokensyaNames = new();
        kohiHoubetuMsts = new();
        kohiHoubetus = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private int welfareType;
    private int currentPage;
    private bool hasNextPage;
    #endregion

    public CommonReportingRequestModel GetP22WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            this.welfareType = welfareType;

            switch (welfareType)
            {
                //こども医療費
                case 0: kohiHoubetus = new List<string> { "83" }; break;
                //母子障害
                case 1: kohiHoubetus = new List<string> { "84", "85" }; break;
            }

            var getData = GetData();

            if (welfareType == 1)
            {
                _formFileName = "p22WelfareSeikyu84.rse";
            }

            if (getData)
            {
                foreach ((int currentYm, string currentCode, string currentCity) in cityNames)
                {
                    totalData = new countData();

                    currentFutansyaNo = currentCode;
                    currentCityName = currentCity;
                    currentSinYm = currentYm;
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
        const int maxRow = 10;
        bool _hasNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            Dictionary<string, string> fieldDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", hpInf.Tel);
            //診療年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(currentSinYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //福祉区分
            if (welfareType == 1)
            {
                if (currentFutansyaNo.StartsWith("84"))
                {
                    SetFieldData("welfareName", "母子家庭等");
                }
                else if (currentFutansyaNo.StartsWith("85"))
                {
                    SetFieldData("welfareName", "重度障害者(児)");
                }
            }
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //市町村
            if (welfareType == 0)
            {
                fieldDataPerPage.Add("futansyaNo", CIUtil.Copy(currentFutansyaNo, 5, 4));
                _setFieldData.Add(pageIndex, fieldDataPerPage);
            }
            else
            {
                fieldDataPerPage.Add("futansyaNo", currentFutansyaNo);
                _setFieldData.Add(pageIndex, fieldDataPerPage);
            }
            SetFieldData("hokensyaName", currentCityName);
            //合計金額
            var curReceInfs = receInfs.Where(r => r.CityName == currentCityName);
            SetFieldData("totalFutan", curReceInfs.Sum(r => r.KohiFutan).ToString());
            //ページ数
            int totalPage = (int)Math.Ceiling((double)curReceInfs.Count() / maxRow);
            SetFieldData("currentPage", currentPage.ToString());
            SetFieldData("totalPage", totalPage.ToString());


            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            var curReceInfs = receInfs.Where(r => r.SinYm == currentSinYm && r.WelfareFutansyaNo == currentFutansyaNo).OrderBy(r => r.WelfareTokusyuNo).ThenBy(r => r.PtNum).ToList();
            int ptIndex = (currentPage - 1) * maxRow;

            countData subTotalData = new countData();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                var curReceInf = curReceInfs[ptIndex];

                subTotalData.RecordCount++;
                totalData.RecordCount++;

                //受給者証番号
                listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, curReceInf.WelfareTokusyuNo ?? string.Empty));
                //氏名
                listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                //生年月日
                listDataPerPage.Add(new("birthday", 0, rowNo, (CIUtil.SDateToWDate(curReceInf.Birthday) % 1000000).AsString().PadLeft(6, '0')));
                //割合
                listDataPerPage.Add(new("hokenRate", 0, rowNo, (curReceInf.HokenRate / 10).ToString()));
                //実日数
                listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.HokenNissu.ToString()));
                subTotalData.Nissu += curReceInf.HokenNissu;
                totalData.Nissu += curReceInf.HokenNissu;
                //総点数
                listDataPerPage.Add(new("tensu", 0, rowNo, curReceInf.Tensu.ToString()));
                subTotalData.Tensu += curReceInf.Tensu;
                totalData.Tensu += curReceInf.Tensu;
                //窓口負担額
                listDataPerPage.Add(new("futan", 0, rowNo, String.Format("{0:#,0}", curReceInf.PtFutan)));
                subTotalData.Futan += curReceInf.PtFutan;
                totalData.Futan += curReceInf.PtFutan;

                ptIndex++;
                if (ptIndex >= curReceInfs.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
            Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

            fieldDataPerPage.Add("subTotalCount", subTotalData.RecordCount.ToString());

            if (!_setFieldData.ContainsKey(pageIndex))
            {
                _setFieldData.Add(pageIndex, fieldDataPerPage);
            }
            listDataPerPage.Add(new("nissu", 0, 10, subTotalData.Nissu.ToString()));
            listDataPerPage.Add(new("tensu", 0, 10, subTotalData.Tensu.ToString()));
            listDataPerPage.Add(new("futan", 0, 10, String.Format("{0:#,0}", subTotalData.Futan)));

            if (_hasNextPage == false)
            {
                fieldDataPerPage.Add("totalCount", totalData.RecordCount.ToString());

                if (!_setFieldData.ContainsKey(pageIndex))
                {
                    _setFieldData.Add(pageIndex, fieldDataPerPage);
                }

                listDataPerPage.Add(new("nissu", 0, 11, totalData.Nissu.ToString()));
                listDataPerPage.Add(new("tensu", 0, 11, totalData.Tensu.ToString()));
                listDataPerPage.Add(new("futan", 0, 11, String.Format("{0:#,0}", totalData.Futan)));
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

        FutanCheck futanCheck = FutanCheck.KohiFutan;
        if (welfareType == 1)
        {
            // 母子、障害の場合は公費負担をチェックしない
            futanCheck = FutanCheck.None;
        }
        var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, futanCheck, 0);
        //静岡県用のモデルクラスにコピー
        receInfs = wrkReces.Select(x => new CoP22WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHoubetus)).ToList();
        //負担者番号の一覧を取得
        cityNames =
            receInfs.GroupBy(r => (r.SinYm, r.WelfareFutansyaNo, r.CityName))
            .OrderBy(r => r.Key.WelfareFutansyaNo)
            .ThenBy(r => r.Key.SinYm)
            .Select(r => (r.Key.SinYm, r.Key.WelfareFutansyaNo, r.Key.CityName)).ToList();

        //保険者番号リストを取得
        var hokensyaNos = receInfs.Where(r => r.IsKokuhoKumiai).GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList();
        //保険者名を取得
        hokensyaNames = _welfareFinder.GetHokensyaName(hpId, hokensyaNos);
        //公費法別番号リストを取得
        kohiHoubetuMsts = _welfareFinder.GetKohiHoubetuMst(hpId, seikyuYm);


        return (receInfs?.Count ?? 0) > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
    #endregion
}
