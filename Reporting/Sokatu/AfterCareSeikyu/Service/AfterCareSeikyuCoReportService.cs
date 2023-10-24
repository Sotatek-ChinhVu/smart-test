using Reporting.Sokatu.AfterCareSeikyu.Model;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.AfterCareSeikyu.DB;
using Reporting.Structs;
using Helper.Common;
using Reporting.CommonMasters.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.AfterCareSeikyu.Mapper;

namespace Reporting.Sokatu.AfterCareSeikyu.Service;

public class AfterCareSeikyuCoReportService : IAfterCareSeikyuCoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, bool> _visibleAtPrint;
    private readonly ICoAfterCareSeikyuFinder _finder;

    private CoSeikyuInfModel seikyuInf;
    private CoHpInfModel hpInf;
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;

    public AfterCareSeikyuCoReportService(ICoAfterCareSeikyuFinder finder)
    {
        _finder = finder;
        _singleFieldData = new();
        _visibleAtPrint = new();
        hpInf = new();
        seikyuInf = new();
    }

    public CommonReportingRequestModel GetAfterCareSeikyuPrintData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        this.seikyuYm = seikyuYm;
        this.seikyuType = seikyuType;
        this.hpId = hpId;

        //印
        _visibleAtPrint.Add("Frame", true);

        GetData();
        UpdateDrawForm();

        string formFileName = "p99AfterCareSeikyu.rse";

        return new AfterCareSeikyuMapper(_singleFieldData, _visibleAtPrint, formFileName).GetData();
    }

    #region Private function
    private void UpdateDrawForm()
    {
        #region SubMethod
        void UpdateFormBody()
        {
            //指定病院等の番号
            SetFieldData("hpCode", hpInf.RousaiHpCd);

            //請求金額
            SetFieldData("seikyuGaku", string.Format("{0, 9}", seikyuInf.SeikyuGaku));

            //内訳書添付枚数
            SetFieldData("meisaiCount", string.Format("{0, 3}", seikyuInf.MeisaiCount));

            //請求年月
            int seikyuYmd = CIUtil.SDateToWDateForRousai(seikyuYm * 100 + 1);
            SetFieldData("seikyuGengo", seikyuYmd.ToString().Substring(0, 1));
            SetFieldData("seikyuYear", seikyuYmd.ToString().Substring(1, 2));
            SetFieldData("seikyuMonth", seikyuYmd.ToString().Substring(3, 2));

            //代表者名
            SetFieldData("daihyoName", seikyuInf.DaihyoName);

            //請求人数（代表者以外の人数）
            SetFieldData("seikyuNinzu", seikyuInf.SeikyuNinzu.ToString());

            //提出年月日
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd"))
            );
            SetFieldData("reportGengo", wrkYmd.Gengo);
            SetFieldData("reportYear", wrkYmd.Year.ToString());
            SetFieldData("reportMonth", wrkYmd.Month.ToString());
            SetFieldData("reportDay", wrkYmd.Day.ToString());
            //郵便番号
            SetFieldData("postCd1", hpInf.PostCd.PadRight(7).Substring(0, 3));
            SetFieldData("postCd2", hpInf.PostCd.PadRight(7).Substring(3, 4));
            //住所
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            //名称
            SetFieldData("hpName", hpInf.ReceHpName);
            //代表者氏名
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            //電話番号
            string[] tels = hpInf.Tel.Split('-');
            for (int i = 0; i < tels.Length; i++)
            {
                if (i == 3) break;

                SetFieldData(string.Format("hpTel{0}", i), tels[i]);
            }
            //都道府県名
            var prefName = PrefCode.PrefName(hpInf.PrefNo);
            if (prefName != "北海道" && prefName.Length > 1)
            {
                prefName = prefName.Substring(0, prefName.Length - 1);
            }
            SetFieldData("prefName", prefName);
        }

        #endregion

        UpdateFormBody();
    }

    private void GetData()
    {
        seikyuInf = _finder.GetSeikyuInf(hpId, seikyuYm, seikyuType);
        hpInf = _finder.GetHpInf(hpId, seikyuYm);
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
