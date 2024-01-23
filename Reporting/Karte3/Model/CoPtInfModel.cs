using Entity.Tenant;
using Helper.Extension;

namespace Reporting.Karte3.Model;

public class CoPtInfModel
{
    public PtInf PtInf { get; }

    public CoPtInfModel(PtInf ptInf)
    {
        PtInf = ptInf;
    }

    public CoPtInfModel()
    {
        PtInf = new();
    }

    /// <summary>
    /// 患者情報
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return PtInf.HpId; }
    }

    /// <summary>
    /// 患者ID
    ///  患者を識別するためのシステム固有の番号       
    /// </summary>
    public long PtId
    {
        get { return PtInf.PtId; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long SeqNo
    {
        get { return PtInf.SeqNo; }
    }

    /// <summary>
    /// 患者番号
    ///  医療機関が患者特定するための番号
    /// </summary>
    public long PtNum
    {
        get { return PtInf.PtNum.AsLong(); }
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName
    {
        get { return PtInf.KanaName ?? string.Empty; }
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name
    {
        get { return PtInf.Name ?? string.Empty; }
    }

    /// <summary>
    /// 性別
    ///  1:男 
    ///  2:女
    /// </summary>
    public int Sex
    {
        get { return PtInf.Sex; }
    }

    /// <summary>
    /// 生年月日
    ///  yyyymmdd 
    /// </summary>
    public int Birthday
    {
        get { return PtInf.Birthday; }
    }

    /// <summary>
    /// 死亡区分
    ///  0:生存 
    ///  1:死亡 
    ///  2:消息不明
    /// </summary>
    public int IsDead
    {
        get { return PtInf.IsDead; }
    }

    /// <summary>
    /// 死亡日
    ///  yyyymmdd  
    /// </summary>
    public int DeathDate
    {
        get { return PtInf.DeathDate; }
    }

    /// <summary>
    /// 自宅郵便番号
    ///  区切り文字("-") を除く   
    /// </summary>
    public string HomePost
    {
        get { return PtInf.HomePost ?? string.Empty; }
    }

    /// <summary>
    /// 自宅住所１
    /// </summary>
    public string HomeAddress1
    {
        get { return PtInf.HomeAddress1 ?? string.Empty; }
    }

    /// <summary>
    /// 自宅住所２
    /// </summary>
    public string HomeAddress2
    {
        get { return PtInf.HomeAddress2 ?? string.Empty; }
    }

    /// <summary>
    /// 電話番号１
    /// </summary>
    public string Tel1
    {
        get { return PtInf.Tel1 ?? string.Empty; }
    }

    /// <summary>
    /// 電話番号２
    /// </summary>
    public string Tel2
    {
        get { return PtInf.Tel2 ?? string.Empty; }
    }

    /// <summary>
    /// E-Mailアドレス
    /// </summary>
    public string Mail
    {
        get { return PtInf.Mail ?? string.Empty; }
    }

    /// <summary>
    /// 世帯主名
    /// </summary>
    public string Setanusi
    {
        get { return PtInf.Setanusi ?? string.Empty; }
    }

    /// <summary>
    /// 続柄
    /// </summary>
    public string Zokugara
    {
        get { return PtInf.Zokugara ?? string.Empty; }
    }

    /// <summary>
    /// 職業
    /// </summary>
    public string Job
    {
        get { return PtInf.Job ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先名称
    /// </summary>
    public string RenrakuName
    {
        get { return PtInf.RenrakuName ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先郵便番号
    /// </summary>
    public string RenrakuPost
    {
        get { return PtInf.RenrakuPost ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先住所１
    /// </summary>
    public string RenrakuAddress1
    {
        get { return PtInf.RenrakuAddress1 ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先住所２
    /// </summary>
    public string RenrakuAddress2
    {
        get { return PtInf.RenrakuAddress2 ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先電話番号
    /// </summary>
    public string RenrakuTel
    {
        get { return PtInf.RenrakuTel ?? string.Empty; }
    }

    /// <summary>
    /// 連絡先電話番号
    /// </summary>
    public string RenrakuMemo
    {
        get { return PtInf.RenrakuMemo ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先名称
    /// </summary>
    public string OfficeName
    {
        get { return PtInf.OfficeName ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先郵便番号
    /// </summary>
    public string OfficePost
    {
        get { return PtInf.OfficePost ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先住所１
    /// </summary>
    public string OfficeAddress1
    {
        get { return PtInf.OfficeAddress1 ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先住所２
    /// </summary>
    public string OfficeAddress2
    {
        get { return PtInf.OfficeAddress2 ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先電話番号
    /// </summary>
    public string OfficeTel
    {
        get { return PtInf.OfficeTel ?? string.Empty; }
    }

    /// <summary>
    /// 勤務先備考
    /// </summary>
    public string OfficeMemo
    {
        get { return PtInf.OfficeMemo ?? string.Empty; }
    }

    /// <summary>
    /// 領収証明細発行区分
    ///  0:不要 
    ///  1:要
    /// </summary>
    public int IsRyosyoDetail
    {
        get { return PtInf.IsRyosyoDetail; }
    }

    /// <summary>
    /// 主治医コード
    /// </summary>
    public int PrimaryDoctor
    {
        get { return PtInf.PrimaryDoctor; }
    }

    /// <summary>
    /// テスト患者区分
    ///  1:テスト患者
    /// </summary>
    public int IsTester
    {
        get { return PtInf.IsTester; }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除
    /// </summary>
    public int IsDelete
    {
        get { return PtInf.IsDelete; }
    }

    /// <summary>
    /// MAIN_HOKEN_PID
    /// </summary>
    public int MainHokenPid
    {
        get { return PtInf.MainHokenPid; }
    }

}
