using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public class P13WelfareSeikyuCoReportService : IP13WelfareSeikyuCoReportService
{
    #region Constant
    private List<string> kohiHoubetus;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoWelfareSeikyuFinder _welfareFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoWelfareReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    #endregion

    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private readonly ISystemConfig _systemConfig;

    #region Constructor and Init
    public P13WelfareSeikyuCoReportService(ISystemConfig systemConfig, ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
        _visibleAtPrint = new();
        _systemConfig = systemConfig;
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

    public CommonReportingRequestModel GetP13WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;

            switch (welfareType)
            {
                //負担医療費請求書
                case 0: kohiHoubetus = new List<string> { "82" }; break;
                //難病医療費請求書
                case 1: kohiHoubetus = new List<string> { "83" }; break;
            }
            var getData = GetData();

            string _formFileName = "";
            switch (welfareType)
            {
                case 0: _formFileName = "p13WelfareSeikyuGreen.rse"; break;
                case 1: _formFileName = "p13WelfareSeikyuBlue.rse"; break;
            }

            switch (welfareType)
            {
                case 0: _visibleAtPrint.Add("Frame", _systemConfig.P13WelfareGreenSeikyuType(hpId) == 0); break;
                case 1: _visibleAtPrint.Add("Frame", _systemConfig.P13WelfareBlueSeikyuType(hpId) == 0); break;
            }

            currentPage = 1;
            hasNextPage = true;

            if (getData)
            {
                while (getData && hasNextPage)
                {
                    if (_formFileName == "") continue;
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new WelfareSeikyuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }
        finally
        {
            _systemConfig.ReleaseResource();
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
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("hpTel", hpInf.Tel);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //ページ数
            SetFieldData("currentPage", currentPage.ToString());

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            int ptIndex = (currentPage - 1) * maxRow;

            int totalCount = 0;
            int totalSeikyu = 0;

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (receInfs.Count <= ptIndex)
                {
                    _hasNextPage = false;
                    break;
                }
                var curReceInf = receInfs[ptIndex];

                //負担者番号
                if (curReceInf.FutansyaNo(kohiHoubetus) != "")
                {
                    listDataPerPage.Add(new("futansyaNo0", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(0, 2)));
                    listDataPerPage.Add(new("futansyaNo1", 0, rowNo, curReceInf.FutansyaNo(kohiHoubetus).Substring(4, 4)));

                }
                //受給者番号
                listDataPerPage.Add(new("jyukyusyaNo", 0, rowNo, string.Format("{0, 7}", curReceInf.JyukyusyaNo(kohiHoubetus))));
                //保険者番号
                listDataPerPage.Add(new("hokensyaNo", 0, rowNo, curReceInf.HokensyaNo));
                //氏名
                listDataPerPage.Add(new("ptName", 0, rowNo, curReceInf.PtName));
                //負担割合
                listDataPerPage.Add(new("hokenRate", 0, (short)(rowNo * 3 + (int)Math.Floor((double)curReceInf.HokenRate / 10) - 1), "○"));
                //入外区分
                listDataPerPage.Add(new("gairai", 0, rowNo, "○"));
                //診療日数
                listDataPerPage.Add(new("nissu", 0, rowNo, curReceInf.HokenNissu.ToString()));
                //請求額
                listDataPerPage.Add(new("seikyu", 0, rowNo, curReceInf.KohiFutan(kohiHoubetus).ToString()));
                totalSeikyu += curReceInf.KohiFutan(kohiHoubetus);
                //公費分点数
                listDataPerPage.Add(new("kohiTensu", 0, rowNo, curReceInf.KohiReceTensu(kohiHoubetus).ToString()));
                if (welfareType == 1)
                {
                    //一部負担金相当額
                    listDataPerPage.Add(new("futan", 0, rowNo, curReceInf.KohiReceFutan(kohiHoubetus).ToString()));
                }
                //備考
                if (curReceInf.IsChoki)
                {
                    listDataPerPage.Add(new("biko", 0, rowNo, "長"));
                    listDataPerPage.Add(new("bikoMaru", 0, rowNo, "○"));
                }
                else if (curReceInf.KogakuOverKbn != KogakuOverStatus.None)
                {
                    switch (curReceInf.KogakuKbn)
                    {
                        case 4: listDataPerPage.Add(new("biko", 0, rowNo, "Ⅱ")); break;
                        case 5: listDataPerPage.Add(new("biko", 0, rowNo, "Ⅰ")); break;
                        case 26: listDataPerPage.Add(new("biko", 0, rowNo, "ア")); break;
                        case 27: listDataPerPage.Add(new("biko", 0, rowNo, "イ")); break;
                        case 28: listDataPerPage.Add(new("biko", 0, rowNo, "ウ")); break;
                        case 29: listDataPerPage.Add(new("biko", 0, rowNo, "エ")); break;
                        case 30: listDataPerPage.Add(new("biko", 0, rowNo, "オ")); break;
                    }
                }

                //件数
                totalCount++;

                ptIndex++;
                if (ptIndex >= receInfs.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            //合計
            SetFieldData("totalCount", totalCount.ToString());
            SetFieldData("totalSeikyu", totalSeikyu.ToString());
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
        var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Kokho);
        //都外国保+マル都(82)
        List<string> prefIn = new List<string> { "13", "63" };
        receInfs = wrkReces.Where(r => !prefIn.Contains(r.HokensyaNo.Substring(r.HokensyaNo.Length - 6, 2))).ToList();

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
