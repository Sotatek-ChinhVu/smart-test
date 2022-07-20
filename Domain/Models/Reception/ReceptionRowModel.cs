using Domain.Constant;

namespace Domain.Models.Reception;

public class ReceptionRowModel
{
    public ReceptionRowModel(long raiinNo, long parentRaiinNo, int uketukeNo, bool hasLockInf, int raiinStatus,
        long ptNum, string kanaName, string name, int sex, int birthday, string? yoyakuTime,
        string? rsvFrameName, string uketukeSbtName, string? uketukeTime, string sinStartTime,
        string? sinEndTime, string? kaikeiTime, string? raiinCmt, string? ptComment,
        string tantoName, string kaName, int lastVisitDate, string sname, string? raiinRemark,
        int confirmationState, string? confirmationResult, List<int> grpIds, List<DynamicCell> dynamicCells)
    {
        RaiinNo = raiinNo;
        SameVisit = parentRaiinNo == 0 ? string.Empty : parentRaiinNo.ToString();
        UketukeNo = uketukeNo;
        Status = hasLockInf ? RaiinState.Examining : raiinStatus;
        OriginalStatus = raiinStatus;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = sex;
        Birthday = birthday;
        Age = "TODO";
        YoyakuTime = yoyakuTime ?? string.Empty;
        ReservationName = rsvFrameName ?? string.Empty;
        UketukeSbtName = uketukeSbtName;
        UketukeTime = uketukeTime ?? string.Empty;
        SinStartTime = sinStartTime;
        SinEndTime = sinEndTime ?? string.Empty;
        KaikeiTime = kaikeiTime ?? string.Empty;
        RaiinCmt = raiinCmt ?? string.Empty;
        PtComment = ptComment ?? string.Empty;
        HokenPatternName = "TODO";
        TantoName = tantoName;
        KaName = kaName;
        LastVisitDate = lastVisitDate;
        Sname = sname;
        RaiinRemark = raiinRemark ?? string.Empty;
        ConfirmationState = confirmationState;
        ConfirmationResult = confirmationResult ?? string.Empty;
        GrpIdToDynamicCell = grpIds.ToDictionary(
            grpId => grpId,
            grpId => dynamicCells.FirstOrDefault(c => c.GrpId == grpId, new DynamicCell(grpId)));
    }

    public long RaiinNo { get; set; }
    // 順番
    public int UketukeNo { get; set; }
    // 同一来院
    public string SameVisit { get; set; } = string.Empty;
    // 状態
    public int Status { get; set; }
    public int OriginalStatus { get; set; }
    // 患者番号
    public long PtNum { get; set; }
    // カナ氏名
    public string KanaName { get; set; } = string.Empty;
    // 氏名
    public string Name { get; set; } = string.Empty;
    // 性
    public int Sex { get; set; }
    // 生年月日
    public int Birthday { get; set; }
    // 年齢
    public string Age { get; set; } = string.Empty;
    // 読
    public bool IsNameDuplicate { get; set; }
    // 予約時間
    public string YoyakuTime { get; set; } = string.Empty;
    // 予約名
    public string ReservationName { get; set; } = string.Empty;
    // 受付種別
    public string UketukeSbtName { get; set; } = string.Empty;
    // 受付時間
    public string UketukeTime { get; set; } = string.Empty;
    // 診察開始
    public string SinStartTime { get; set; } = string.Empty;
    // 診察終了
    public string SinEndTime { get; set; } = string.Empty;
    // 精算時間
    public string KaikeiTime { get; set; } = string.Empty;
    // 来院コメント
    public string RaiinCmt { get; set; } = string.Empty;
    // 患者コメント
    public string PtComment { get; set; } = string.Empty;
    // 保険
    public string HokenPatternName { get; set; } = string.Empty;
    // 担当医
    public string TantoName { get; set; } = string.Empty;
    // 診療科
    public string KaName { get; set; } = string.Empty;
    // 前回来院
    public int LastVisitDate { get; set; }
    // 主治医
    public string Sname { get; set; } = string.Empty;
    // 備考
    public string RaiinRemark { get; set; } = string.Empty;
    // 資格確認状況
    public int ConfirmationState { get; set; }
    // 資格確認結果
    public string ConfirmationResult { get; set; } = string.Empty;
    // Dynamic cells
    public Dictionary<int, DynamicCell> GrpIdToDynamicCell { get; set; }
}
