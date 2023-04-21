using Entity.Tenant;

namespace Reporting.Accounting.Model;

public class CoSystemGenerationConfModel
{
    public SystemGenerationConf SystemGenerationConf { get; }

    public CoSystemGenerationConfModel(SystemGenerationConf systemGenerationConf)
    {
        SystemGenerationConf = systemGenerationConf;
    }

    /// <summary>
    /// Id
    /// </summary>
    public long Id
    {
        get { return SystemGenerationConf.Id; }
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return SystemGenerationConf.HpId; }
    }

    /// <summary>
    /// 分類コード
    /// 
    /// </summary>
    public int GrpCd
    {
        get { return SystemGenerationConf.GrpCd; }
    }

    /// <summary>
    /// 分類枝番
    /// 
    /// </summary>
    public int GrpEdaNo
    {
        get { return SystemGenerationConf.GrpEdaNo; }
    }

    /// <summary>
    /// 開始日
    /// 
    /// </summary>
    public int StartDate
    {
        get { return SystemGenerationConf.StartDate; }
    }

    /// <summary>
    /// 終了日
    /// 
    /// </summary>
    public int EndDate
    {
        get { return SystemGenerationConf.EndDate; }
    }

    /// <summary>
    /// 設定値
    /// 
    /// </summary>
    public int Val
    {
        get { return SystemGenerationConf.Val; }
    }

    /// <summary>
    /// パラメーター
    /// 
    /// </summary>
    public string Param
    {
        get { return SystemGenerationConf.Param ?? string.Empty; }
    }
    /// <summary>
    /// カンマ区切りのパラメータ取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetParam(int index)
    {
        string ret = "";

        string[] fields = Param.Split(',');

        if (fields.Count() > index)
        {
            ret = fields[index];
        }

        return ret;
    }

    /// <summary>
    /// 備考
    /// 
    /// </summary>
    public string Biko
    {
        get { return SystemGenerationConf.Biko ?? string.Empty; }
    }


}
