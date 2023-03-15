using Entity.Tenant;
using Helper.Common;

namespace Reporting.Receipt.Models;

public class PtInfModel
{
    public PtInf PtInf { get; } = null;
    private int _ageKbn;
    private int _ageYear, _ageMonth, _ageDay;
    private bool _isStudent;
    private bool _isElder;

    public PtInfModel(PtInf ptInf, int sinDate)
    {
        PtInf = ptInf;

        _ageKbn = 9;

        _ageYear = 0;
        _ageMonth = 0;
        _ageDay = 0;

        CIUtil.SDateToDecodeAge(PtInf.Birthday, sinDate, ref _ageYear, ref _ageMonth, ref _ageDay);

        if ((_ageYear == 0) && (_ageMonth == 0) && (_ageDay < 28))
        {
            _ageKbn = 0;
        }
        else if ((_ageYear == 0) && (_ageMonth < 12))
        {
            // 乳児
            _ageKbn = 1;
        }
        else if (_ageYear < 3)
        {
            // 幼児
            _ageKbn = 2;
        }
        else if (_ageYear < 6)
        {
            // 幼児
            _ageKbn = 3;
        }

        _isStudent = CIUtil.IsStudent(PtInf.Birthday, sinDate);

        _isElder = CIUtil.AgeChk(PtInf.Birthday, sinDate, 70);
    }

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
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get { return PtInf.PtNum; }
    }
    ///// <summary>
    ///// 連番
    ///// </summary>
    //public long SeqNo
    //{
    //    get { return PtInf.SeqNo; }
    //    set
    //    {
    //        if (PtInf.SeqNo == value) return;
    //        PtInf.SeqNo = value;
    //        RaisePropertyChanged(() => SeqNo);
    //    }
    //}

    ///// <summary>
    ///// 患者番号
    /////  医療機関が患者特定するための番号
    ///// </summary>
    //public long PtNum
    //{
    //    get { return PtInf.PtNum; }
    //    set
    //    {
    //        if (PtInf.PtNum == value) return;
    //        PtInf.PtNum = value;
    //        RaisePropertyChanged(() => PtNum);
    //    }
    //}

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
    /// 年齢区分
    /// 0: 新生児（生後28日未満）
    /// 1: 乳児（1歳未満）
    /// 2: 乳幼児（3歳未満）
    /// 3: 幼児（6歳未満）
    /// 9: 6歳以上
    /// </summary>
    public int AgeKbn()
    {
        return _ageKbn;
    }

    /// <summary>
    /// 年齢（年）
    /// </summary>
    public int Age
    {
        get { return _ageYear; }
    }

    /// <summary>
    /// 年齢（月）
    /// </summary>
    public int AgeMonth
    {
        get { return _ageMonth; }
    }

    /// <summary>
    /// 年齢（日）
    /// </summary>
    public int AgeDay
    {
        get { return _ageDay; }
    }

    /// <summary>
    /// 幼児（6歳未満）かどうか判定
    /// </summary>
    /// <returns>true: 幼児（6歳未満）</returns>
    public bool IsYoJi
    {
        get { return AgeKbn() < 9; }
    }

    /// <summary>
    /// 乳幼児（3歳未満）かどうか判定
    /// </summary>
    /// <returns>true: 乳幼児（3歳未満）</returns>
    public bool IsNyuyoJi
    {
        get { return AgeKbn() < 3; }
    }

    /// <summary>
    /// 乳児（1歳未満）かどうか判定
    /// </summary>
    /// <returns>true: 乳児（1歳未満）</returns>
    public bool IsNyuJi
    {
        get { return AgeKbn() < 2; }
    }

    /// <summary>
    /// 新生児（28日未満）かどうか判定
    /// </summary>
    /// <returns>true: 新生児（28日未満）</returns>
    public bool IsSinseiJi
    {
        get { return AgeKbn() < 1; }
    }

    /// <summary>
    /// 就学しているかどうか
    /// false: 未就学
    /// </summary>
    public bool IsStudent
    {
        get { return _isStudent; }
    }

    public bool IsElder
    {
        get { return _isElder; }
    }
}
