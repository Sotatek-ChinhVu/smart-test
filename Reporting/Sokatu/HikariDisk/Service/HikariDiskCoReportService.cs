using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.HikariDisk.DB;
using Reporting.Sokatu.HikariDisk.Mapper;

namespace Reporting.Sokatu.HikariDisk.Service;

public class HikariDiskCoReportService : IHikariDiskCoReportService
{
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;

    private int seikyuYm;
    private int hokenKbn;
    private int diskKind;
    private int diskCnt;
    private int hpId;

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, bool> _visibleFieldList;
    private readonly ISystemConfig _systemConfig;
    private readonly ICoHikariDiskFinder _finder;

    public HikariDiskCoReportService(ISystemConfig systemConfig, ICoHikariDiskFinder finder)
    {
        _systemConfig = systemConfig;
        _finder = finder;
        _visibleFieldList = new();
        _singleFieldData = new();
        receInfs = new();
        hpInf = new();
    }

    public CommonReportingRequestModel GetHikariDiskPrintData(int hpId, int seikyuYm, int hokenKbn, int diskKind, int diskCnt)
    {
        try
        {
            this.seikyuYm = seikyuYm;
            this.hokenKbn = hokenKbn;
            this.diskKind = diskKind;
            this.diskCnt = diskCnt;
            this.hpId = hpId;

            //印
            _visibleFieldList.Add("inkan", seikyuYm < KaiseiDate.m202210);
            _visibleFieldList.Add("inkanMaru", seikyuYm < KaiseiDate.m202210);
            GetData();
            UpdateDrawForm();

            string formFileName = "p99HikariDisk.rse";
            if (hpInf.PrefNo == PrefCode.Ibaraki)
            {
                formFileName = "p08HikariDisk.rse";
            }

            return new HikariDiskMapper(_singleFieldData, _visibleFieldList, formFileName).GetData();
        }
        finally
        {
            _systemConfig.ReleaseResource();
            _finder.ReleaseResource();
        }
    }

    private void UpdateDrawForm()
    {
        #region SubMethod
        void UpdateFormHeader()
        {
            string seikyuName1 = string.Empty;
            string seikyuName2 = string.Empty;
            if (hokenKbn == Helper.Constants.HokenKbn.Syaho)
            {
                seikyuName1 = "社会保険診療報酬支払基金";
                if (hpInf.PrefNo != PrefCode.Hokkaido)
                {
                    seikyuName2 = PrefCode.PrefShortName(hpInf.PrefNo) + "支部";
                }
            }
            else
            {
                if (hpInf.PrefNo != PrefCode.Fukui)
                {
                    seikyuName1 = PrefCode.PrefName(hpInf.PrefNo) + "国民健康保険団体連合会";
                }
            }
            SetFieldData("seikyuName1", seikyuName1);
            SetFieldData("seikyuName2", seikyuName2);

            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("kaisetuName", hpInf.KaisetuName);

        }

        void UpdateFormBody()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関名称
            SetFieldData("hpName", hpInf.ReceHpName);
            //診療月分
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", wrkYmd.Gengo);
            SetFieldData("seikyuYear", wrkYmd.Year.ToString());
            SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            //提出年月日
            wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //媒体種類
            SetFieldData(string.Format("diskKind{0}", diskKind), "〇");
            //媒体枚数
            SetFieldData("diskCnt", diskCnt.ToString());
            //備考
            if (_systemConfig.HikariDiskIsTotalCnt() == 1)
            {
                if (hpInf.PrefNo == PrefCode.Ibaraki && hokenKbn == Helper.Constants.HokenKbn.Kokho)
                {
                    //茨城県国保は記載しない
                }
                else
                {
                    //総件数を記載する
                    int totalCnt = receInfs.Sum(r => r.ReceCnt);
                    SetFieldData("biko", string.Format("{0} 件", totalCnt));
                }
            }
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private void GetData()
    {
        receInfs = _finder.GetReceInf(hpId, seikyuYm, hokenKbn);
        hpInf = _finder.GetHpInf(hpId, seikyuYm);
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
}
