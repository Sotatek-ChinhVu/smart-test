﻿using Domain.Common;
using Domain.Constant;
using Helper.Common;
using Helper.Extendsions;

namespace Domain.Models.Reception;

public class ReceptionRowModel
{
    public ReceptionRowModel(long raiinNo, long parentRaiinNo, int uketukeNo, bool hasLockInf, int raiinStatus,
        long ptNum, string kanaName, string name, int sex, int birthday, string? yoyakuTime,
        string? rsvFrameName, string? uketukeSbtName, string? uketukeTime, string sinStartTime,
        string? sinEndTime, string? kaikeiTime, string? raiinCmt, string? ptComment,
        string? tantoName, string? kaName, int lastVisitDate, string? sname, string? raiinRemark,
        int confirmationState, string? confirmationResult, List<int> grpIds, List<DynamicCell> dynamicCells,
        int sinDate, UserConfCommon.DateTimeFormart dateTimeFormart = UserConfCommon.DateTimeFormart.JapaneseCalendar)
    {
        RaiinNo = raiinNo;
        SameVisit = parentRaiinNo == 0 ? string.Empty : parentRaiinNo.ToString();
        UketukeNo = uketukeNo;
        Status = RaiinState.VisitStatus[hasLockInf ? RaiinState.Examining : raiinStatus];
        OriginalStatus = RaiinState.VisitStatus[raiinStatus];
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = GetGenderText(sex);
        Birthday = GetFormattedBirthDay(birthday, dateTimeFormart);
        Age = CIUtil.SDateToDecodeAge(birthday.AsString(), sinDate.AsString());
        YoyakuTime = yoyakuTime ?? string.Empty;
        ReservationName = rsvFrameName ?? string.Empty;
        UketukeSbtName = uketukeSbtName ?? string.Empty;
        UketukeTime = uketukeTime ?? string.Empty;
        SinStartTime = sinStartTime;
        SinEndTime = sinEndTime ?? string.Empty;
        KaikeiTime = kaikeiTime ?? string.Empty;
        RaiinCmt = raiinCmt ?? string.Empty;
        PtComment = ptComment ?? string.Empty;
        HokenPatternName = "TODO";
        TantoName = tantoName ?? string.Empty;
        KaName = kaName ?? string.Empty;
        LastVisitDate = CIUtil.SDateToShowSDate(lastVisitDate);
        Sname = sname ?? string.Empty;
        RaiinRemark = raiinRemark ?? string.Empty;
        ConfirmationState = GetConfirmationStateText(confirmationState);
        ConfirmationResult = confirmationResult ?? string.Empty;
        GrpIdToDynamicCell = grpIds.ToDictionary(
            grpId => grpId,
            grpId => dynamicCells.FirstOrDefault(c => c.GrpId == grpId, new DynamicCell(grpId)));
    }

    public long RaiinNo { get; private set; }
    // 順番
    public int UketukeNo { get; private set; }
    // 同一来院
    public string SameVisit { get; private set; } = string.Empty;
    // 状態
    public string Status { get; private set; }
    public string OriginalStatus { get; private set; }
    // 患者番号
    public long PtNum { get; private set; }
    // カナ氏名
    public string KanaName { get; private set; } = string.Empty;
    // 氏名
    public string Name { get; private set; } = string.Empty;
    // 性
    public string Sex { get; private set; }
    // 生年月日
    public string Birthday { get; private set; }
    // 年齢
    public string Age { get; private set; } = string.Empty;
    // 読
    public bool IsNameDuplicate { get; private set; }
    // 予約時間
    public string YoyakuTime { get; private set; } = string.Empty;
    // 予約名
    public string ReservationName { get; private set; } = string.Empty;
    // 受付種別
    public string UketukeSbtName { get; private set; } = string.Empty;
    // 受付時間
    public string UketukeTime { get; private set; } = string.Empty;
    // 診察開始
    public string SinStartTime { get; private set; } = string.Empty;
    // 診察終了
    public string SinEndTime { get; private set; } = string.Empty;
    // 精算時間
    public string KaikeiTime { get; private set; } = string.Empty;
    // 来院コメント
    public string RaiinCmt { get; private set; } = string.Empty;
    // 患者コメント
    public string PtComment { get; private set; } = string.Empty;
    // 保険
    public string HokenPatternName { get; private set; } = string.Empty;
    // 担当医
    public string TantoName { get; private set; } = string.Empty;
    // 診療科
    public string KaName { get; private set; } = string.Empty;
    // 前回来院
    public string LastVisitDate { get; private set; }
    // 主治医
    public string Sname { get; private set; } = string.Empty;
    // 備考
    public string RaiinRemark { get; private set; } = string.Empty;
    // 資格確認状況
    public string ConfirmationState { get; private set; }
    // 資格確認結果
    public string ConfirmationResult { get; private set; } = string.Empty;
    // Dynamic cells
    public Dictionary<int, DynamicCell> GrpIdToDynamicCell { get; private set; }

    private string GetGenderText(int sex) => sex switch
    {
        1 => "男",
        2 => "女",
        _ => string.Empty
    };

    private string GetFormattedBirthDay(int birthDay, UserConfCommon.DateTimeFormart dateTimeFormart)
    {
        if (birthDay <= 0)
        {
            return string.Empty;
        }
        switch (dateTimeFormart)
        {
            case UserConfCommon.DateTimeFormart.JapaneseCalendar:
                return CIUtil.SDateToShowWDate2(birthDay);
            case UserConfCommon.DateTimeFormart.WesternCalendar:
                return CIUtil.SDateToShowSWDate(birthDay, -1, 0, 1);
            case UserConfCommon.DateTimeFormart.JapAndWestCalendar:
                return CIUtil.SDateToShowWSDate(birthDay);
            default:
                return CIUtil.SDateToShowWDate2(birthDay);
        }
    }

    private string GetConfirmationStateText(int confirmationState) => confirmationState switch
    {
        1 => "有効",
        2 => "無効",
        3 => "無効（新しい資格あり）",
        4 => "該当資格なし",
        5 => "複数該当",
        97 => "確認エラー",
        98 => "確認中",
        99 => "確認完了",
        _ => string.Empty
    };
}
