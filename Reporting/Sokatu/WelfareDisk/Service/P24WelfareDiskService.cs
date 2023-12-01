using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service;

public class P24WelfareDiskService : IP24WelfareDiskService
{
    private readonly List<int> kohiHokenNos = new List<int> { 101, 102, 103, 105, 106, 107, 203, 206 };

    private List<CoP24WelfareReceInfModel> receInfs;
    private CoHpInfModel hpInf;

    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;

    private readonly ICoWelfareSeikyuFinder _welfareFinder;

    public P24WelfareDiskService(ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
        receInfs = new();
        hpInf = new();
    }

    public CommonExcelReportingModel GetDataP24WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        try
        {
            this.hpId = hpId;
            this.seikyuType = seikyuType;
            this.seikyuYm = seikyuYm;
            GetData();
            List<string> retDatas = new();

            if (GetData())
            {
                foreach (var receInf in receInfs)
                {
                    retDatas.Add(RecordData(receInf));
                }
            }

            string sheetName = string.Format("FKS_241{0}", hpInf.HpCd.PadLeft(7, '0'));
            return new CommonExcelReportingModel(sheetName + ".csv", sheetName, retDatas);
        }
        finally
        {
            _welfareFinder.ReleaseResource();
        }
    }

    #region Private function
    private bool GetData()
    {
        hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
        var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.None, 0);

        //三重県用のモデルクラスにコピー
        receInfs = wrkReces
            .Select(x => new CoP24WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos))
            .OrderBy(r => r.CityCode)
            .ThenBy(r => r.KohiSbt)
            .ThenBy(r => r.WelfareJyukyusyaNo)
            .ToList();
        //処方せん発行区分
        foreach (var receInf in receInfs)
        {
            receInf.IsOutDrug = _welfareFinder.IsOutDrugOrder(hpId, receInf.PtId, receInf.SinYm);
        }

        return (receInfs?.Count ?? 0) > 0;
    }

    private string RecordData(CoP24WelfareReceInfModel receInf)
    {
        List<string> colDatas = new()
        {
            //送付年月
            (CIUtil.SDateToWDate(seikyuYm * 100 + 1) / 100).ToString(),
            //点数表コード
            "1",
            //医療機関コード
            hpInf.HpCd,
            //医療機関名称
            hpInf.ReceHpName,
            //市町村コード
            receInf.CityCode,
            //助成種別
            receInf.KohiSbt.ToString(),
            //受給者番号
            receInf.WelfareJyukyusyaNo,
            //患者氏名(半角カナまたは漢字)
            CIUtil.Copy(receInf.PtKanaName, 1, 20),
            //性別
            receInf.Sex.ToString(),
            //生年月日
            CIUtil.SDateToWDate(receInf.BirthDay).ToString(),
            //診療年月
            (CIUtil.SDateToWDate(receInf.SinYm * 100 + 1) / 100).ToString(),
            //負担割合
            (receInf.HokenRate / 10).ToString(),
            //入外区分(Null:外来)
            string.Empty,
            //入院日数
            string.Empty,
            //保険請求点数
            receInf.Tensu.ToString(),
            //一部負担額
            receInf.HokenReceFutan ?.ToString() ?? "0"
        };
        //公費・マル長区分、公費請求点数、公費・マル長一部負担額
        if (receInf.IsChoki)
        {
            colDatas.Add("99");
            colDatas.Add(receInf.KohiReceTensu(1).ToString());
            colDatas.Add(receInf.Kohi1Limit.ToString());
        }
        else
        {
            string kohiHoubetu = string.Empty;
            int kohiTensu = 0;
            int kohiFutan = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (receInf.KohiReceKisai(i) == 1)
                {
                    //公費が複数ある場合は公費請求点数の高い方の法別番号を記録する
                    if (receInf.KohiReceTensu(i) > kohiTensu)
                    {
                        kohiHoubetu = receInf.KohiHoubetu(i);
                    }
                    //公費が複数ある場合は公費請求点数を合算して記録する
                    kohiTensu += receInf.KohiReceTensu(i);
                    //公費が複数ある場合は公費一部負担額を合算して記録する
                    kohiFutan += receInf.KohiReceFutan(i);
                }
            }
            if (kohiTensu > 0)
            {
                colDatas.Add(kohiHoubetu);
                colDatas.Add(kohiTensu.ToString());
                colDatas.Add(kohiFutan.ToString());
            }
            else
            {
                colDatas.Add(string.Empty);
                colDatas.Add(string.Empty);
                colDatas.Add(string.Empty);
            }
        }

        //予備
        colDatas.Add(string.Empty);
        //入院時食事・生活療養費（保険請求分）
        colDatas.Add(string.Empty);
        //入院時食事・生活療養費（標準負担分）
        colDatas.Add(string.Empty);
        //入院時食事・生活療養費（公費請求分）
        colDatas.Add(string.Empty);
        //入院時食事・生活療養費（公費標準負担分）
        colDatas.Add(string.Empty);
        //処方せん発行区分
        colDatas.Add(receInf.IsOutDrug ? "1" : string.Empty);
        //処方せん発行医療機関コード
        colDatas.Add(string.Empty);
        //処方せん発行医療機関名称
        colDatas.Add(string.Empty);

        return string.Join(",", colDatas);
    }
    #endregion
}
