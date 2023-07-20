using Helper.Common;
using Helper.Extension;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;
using System.Text;

namespace Reporting.Sokatu.WelfareDisk.Service;

public class P24WelfareDiskService : IP24WelfareDiskService
{
    private List<int> KohiHokenNos = new List<int> { 101, 102, 103, 105, 106, 107, 203, 206 };

    private List<CoP24WelfareReceInfModel> receInfs;
    private CoHpInfModel hpInf;

    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;

    private readonly ICoWelfareSeikyuFinder _welfareFinder;

    public P24WelfareDiskService(ICoWelfareSeikyuFinder welfareFinder)
    {
        _welfareFinder = welfareFinder;
    }

    #region OutPut method
    public OutPutExitCode OutPutFile(string outputPath)
    {
        try
        {
            if (!GetData()) return OutPutExitCode.EndNoData;
            //出力パス
            string fileName = string.Format("FKS_241{0}.csv", hpInf.HpCd.PadLeft(7, '0'));
            string filePath = Path.Combine(outputPath, fileName);

            if (!CIUtil.IsDirectoryExisting(outputPath))
            {
                return OutPutExitCode.NotFoundDirectory;
            }
            else if (CIUtil.IsFileExisting(filePath))
            {
                return OutPutExitCode.FileExists;
            }

            List<string> retDatas = new List<string>();

            foreach (var receInf in receInfs)
            {
                retDatas.Add(RecordData(receInf));
            }

            var encoding = Encoding.GetEncoding("shift_jis");
            File.WriteAllLines(filePath, retDatas, encoding);

            return OutPutExitCode.EndSuccess;
        }
        catch (Exception ex)
        {
            Log.WriteLogError(ModuleName, this, nameof(OutPutFile), ex);
            return OutPutExitCode.EndError;
        }
    }
    #endregion

    #region Private function
    private bool GetData()
    {
        hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
        var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, KohiHokenNos, FutanCheck.None, 0);

        //三重県用のモデルクラスにコピー
        receInfs = wrkReces
            .Select(x => new CoP24WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, KohiHokenNos))
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
    #endregion
}
