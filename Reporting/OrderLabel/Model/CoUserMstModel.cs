using Entity.Tenant;

namespace Reporting.OrderLabel.Model;

public class CoUserMstModel
{
    public UserMst UserMst { get; }

    public CoUserMstModel(UserMst userMst)
    {
        UserMst = userMst;
    }

    /// <summary>
    /// ユーザーマスタ
    ///  ユーザー権限は別テーブルで管理予定
    /// </summary>
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get { return UserMst.HpId; }
    }

    /// <summary>
    /// ユーザーID
    /// </summary>
    public int UserId
    {
        get { return UserMst.UserId; }
    }

    /// <summary>
    /// 医師区分
    ///  JOB_MST.JOB_CD 
    /// </summary>
    public int JobCd
    {
        get { return UserMst.JobCd; }
    }

    /// <summary>
    /// 管理者区分
    ///  0:一般 
    ///  1:管理者 
    ///  9:システム管理者
    /// </summary>
    public int ManagerKbn
    {
        get { return UserMst.ManagerKbn; }
    }

    /// <summary>
    /// 診療科ID
    ///  KA_MST.KA_ID  
    /// </summary>
    public int KaId
    {
        get { return UserMst.KaId; }
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName
    {
        get { return UserMst.KanaName ?? string.Empty; }
    }

    /// <summary>
    /// 氏名
    /// </summary>
    public string Name
    {
        get { return UserMst.Name ?? string.Empty; }
    }

    /// <summary>
    /// 略氏名
    /// </summary>
    public string Sname
    {
        get { return UserMst.Sname ?? string.Empty; }
    }

    /// <summary>
    /// 保険医氏名
    /// </summary>
    public string DrName
    {
        get { return UserMst.DrName ?? string.Empty; }
    }

    /// <summary>
    /// 麻薬使用者免許No.
    /// </summary>
    public string MayakuLicenseNo
    {
        get { return UserMst.MayakuLicenseNo ?? string.Empty; }
    }

    /// <summary>
    /// 在籍開始日
    /// </summary>
    public int StartDate
    {
        get { return UserMst.StartDate; }
    }

    /// <summary>
    /// 在籍終了日
    /// </summary>
    public int EndDate
    {
        get { return UserMst.EndDate; }
    }

    /// <summary>
    /// 並び順
    ///  担当医メニューの表示順などに使用     
    /// </summary>
    public int SortNo
    {
        get { return UserMst.SortNo; }
    }

    /// <summary>
    /// 連携コード１
    /// </summary>
    public string RenkeiCd1
    {
        get { return UserMst.RenkeiCd1 ?? string.Empty; }
    }

    /// <summary>
    /// 削除区分
    ///  1:削除
    /// </summary>
    public int IsDeleted
    {
        get { return UserMst.IsDeleted; }
    }

    /// <summary>
    /// 作成日時 
    /// </summary>
    public DateTime CreateDate
    {
        get { return UserMst.CreateDate; }
    }

    /// <summary>
    /// 作成者  
    /// </summary>
    public int CreateId
    {
        get { return UserMst.CreateId; }
    }

    /// <summary>
    /// 作成端末   
    /// </summary>
    public string CreateMachine
    {
        get { return UserMst.CreateMachine ?? string.Empty; }
    }

    /// <summary>
    /// 更新日時   
    /// </summary>
    public DateTime UpdateDate
    {
        get { return UserMst.UpdateDate; }
    }

    /// <summary>
    /// 更新者   
    /// </summary>
    public int UpdateId
    {
        get { return UserMst.UpdateId; }
    }

    /// <summary>
    /// 更新端末   
    /// </summary>
    public string UpdateMachine
    {
        get { return UserMst.UpdateMachine ?? string.Empty; }
    }

    /// <summary>
    /// 連番
    /// </summary>
    public long Id
    {
        get { return UserMst.Id; }
    }
}
