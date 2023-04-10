using Entity.Tenant;

namespace Reporting.CommonMasters.Models;

public class KensaMstModel
{
    public KensaMst KensaMst { get; }

    public KensaMstModel(KensaMst kensaMst)
    {
        KensaMst = kensaMst;
    }

    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    public int HpId
    {
        get { return KensaMst.HpId; }
    }

    /// <summary>
    /// 検査項目コード
    /// 
    /// </summary>
    public string KensaItemCd
    {
        get { return KensaMst.KensaItemCd; }
    }

    /// <summary>
    /// 連番
    /// 
    /// </summary>
    public int KensaItemSeqNo
    {
        get { return KensaMst.KensaItemSeqNo; }
    }

    /// <summary>
    /// センターコード
    /// 
    /// </summary>
    public string CenterCd
    {
        get { return KensaMst.CenterCd ?? string.Empty; }
    }

    /// <summary>
    /// 漢字名称
    /// 
    /// </summary>
    public string KensaName
    {
        get { return KensaMst.KensaName ?? string.Empty; }
    }

    /// <summary>
    /// カナ名称
    /// 
    /// </summary>
    public string KensaKana
    {
        get { return KensaMst.KensaKana ?? string.Empty; }
    }

    /// <summary>
    /// 単位
    /// 
    /// </summary>
    public string Unit
    {
        get { return KensaMst.Unit ?? string.Empty; }
    }

    /// <summary>
    /// 材料コード
    /// 
    /// </summary>
    public int MaterialCd
    {
        get { return KensaMst.MaterialCd; }
    }

    /// <summary>
    /// 容器コード
    /// 
    /// </summary>
    public int ContainerCd
    {
        get { return KensaMst.ContainerCd; }
    }

    /// <summary>
    /// 男性基準値
    /// 
    /// </summary>
    public string MaleStd
    {
        get { return KensaMst.MaleStd ?? string.Empty; }
    }

    /// <summary>
    /// 男性基準値下限
    /// 
    /// </summary>
    public string MaleStdLow
    {
        get { return KensaMst.MaleStdLow ?? string.Empty; }
    }

    /// <summary>
    /// 男性基準値上限
    /// 
    /// </summary>
    public string MaleStdHigh
    {
        get { return KensaMst.MaleStdHigh ?? string.Empty; }
    }

    /// <summary>
    /// 女性基準値
    /// 
    /// </summary>
    public string FemaleStd
    {
        get { return KensaMst.FemaleStd ?? string.Empty; }
    }

    /// <summary>
    /// 女性基準値下限
    /// 
    /// </summary>
    public string FemaleStdLow
    {
        get { return KensaMst.FemaleStdLow ?? string.Empty; }
    }

    /// <summary>
    /// 女性基準値上限
    /// 
    /// </summary>
    public string FemaleStdHigh
    {
        get { return KensaMst.FemaleStdHigh ?? string.Empty; }
    }

    /// <summary>
    /// 式
    /// 
    /// </summary>
    public string Formula
    {
        get { return KensaMst.Formula ?? string.Empty; }
    }

    /// <summary>
    /// 親検査項目コード
    /// 
    /// </summary>
    public string OyaItemCd
    {
        get { return KensaMst.OyaItemCd ?? string.Empty; }
    }

    /// <summary>
    /// 親検査項目連番
    /// 
    /// </summary>
    public int OyaItemSeqNo
    {
        get { return KensaMst.OyaItemSeqNo; }
    }

    /// <summary>
    /// 並び順
    /// 
    /// </summary>
    public long SortNo
    {
        get { return KensaMst.SortNo; }
    }

    /// <summary>
    /// 外注コード１
    /// 
    /// </summary>
    public string CenterItemCd1
    {
        get { return KensaMst.CenterItemCd1 ?? string.Empty; }
    }

    /// <summary>
    /// 外注コード２
    /// 
    /// </summary>
    public string CenterItemCd2
    {
        get { return KensaMst.CenterItemCd2 ?? string.Empty; }
    }

    /// <summary>
    /// 削除区分
    /// 
    /// </summary>
    public int IsDelete
    {
        get { return KensaMst.IsDelete; }
    }
}
