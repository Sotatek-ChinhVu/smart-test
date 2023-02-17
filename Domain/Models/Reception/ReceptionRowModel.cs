using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.Reception;

public class ReceptionRowModel
{
    public ReceptionRowModel(long raiinNo, long ptId, long parentRaiinNo, int uketukeNo, bool hasLockInf, int raiinStatus, int isDeleted,
        long ptNum, string kanaName, string name, int sex, int birthday, string yoyakuTime,
        string rsvFrameName, int uketukeSbtId, string uketukeTime, string sinStartTime,
        string sinEndTime, string kaikeiTime, string raiinCmt, string ptComment,
        int tantoId, int kaId, int lastVisitDate, int firstVisitDate, string sname, string raiinRemark,
        int confirmationState, string confirmationResult, List<int> grpIds, List<DynamicCell> dynamicCells, int sinDate,
        int hokenPid, int hokenStartDate, int hokenEndDate, int hokenSbtCd, int hokenKbn,
        int kohi1HokenSbtKbn, string kohi1Houbetu, int kohi2HokenSbtKbn, string kohi2Houbetu,
        int kohi3HokenSbtKbn, string kohi3Houbetu, int kohi4HokenSbtKbn, string kohi4Houbetu,
        UserConfCommon.DateTimeFormart dateTimeFormart = UserConfCommon.DateTimeFormart.JapaneseCalendar)
    {
        IsDeleted = isDeleted;
        RaiinNo = raiinNo;
        PtId = ptId;
        SinDate = sinDate;
        SameVisit = parentRaiinNo == 0 ? string.Empty : parentRaiinNo.ToString();
        UketukeNo = uketukeNo;
        Status = hasLockInf ? RaiinState.Examining : raiinStatus;
        OriginalStatus = raiinStatus;
        PtNum = ptNum;
        KanaName = kanaName;
        Name = name;
        Sex = GetGenderText(sex);
        Birthday = GetFormattedBirthDay(birthday, dateTimeFormart);
        Age = CIUtil.SDateToDecodeAge(birthday.AsString(), sinDate.AsString());
        YoyakuTime = yoyakuTime;
        ReservationName = rsvFrameName;
        UketukeSbtId = uketukeSbtId;
        UketukeTime = uketukeTime;
        SinStartTime = sinStartTime;
        SinEndTime = sinEndTime;
        KaikeiTime = kaikeiTime;
        RaiinCmt = raiinCmt;
        PtComment = ptComment;
        HokenPatternName = GetHokenName(hokenPid, hokenStartDate, hokenEndDate, hokenSbtCd, hokenKbn,
            kohi1HokenSbtKbn, kohi1Houbetu, kohi2HokenSbtKbn, kohi2Houbetu,
            kohi3HokenSbtKbn, kohi3Houbetu, kohi4HokenSbtKbn, kohi4Houbetu);
        TantoId = tantoId;
        KaId = kaId;
        LastVisitDate = CIUtil.SDateToShowWDate2(lastVisitDate);
        FirstVisitDate = CIUtil.SDateToShowWDate2(firstVisitDate);
        Sname = sname;
        RaiinRemark = raiinRemark;
        ConfirmationState = GetConfirmationStateText(confirmationState);
        ConfirmationResult = confirmationResult;
        GrpIdToDynamicCell = grpIds.ToDictionary(
            grpId => grpId,
            grpId => dynamicCells.FirstOrDefault(c => c.GrpId == grpId, new DynamicCell(grpId)));
        HokenPid = hokenPid;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }
    
    public int IsDeleted { get; private set; }
    
    public long RaiinNo { get; private set; }
    
    // 順番
    public int UketukeNo { get; private set; }
    // 同一来院
    public string SameVisit { get; private set; }
    // 状態
    public int Status { get; private set; }
    public int OriginalStatus { get; private set; }
    // 患者番号
    public long PtNum { get; private set; }
    // カナ氏名
    public string KanaName { get; private set; }
    // 氏名
    public string Name { get; private set; }
    // 性
    public string Sex { get; private set; }
    // 生年月日
    public string Birthday { get; private set; }
    // 年齢
    public string Age { get; private set; }
    // 読
    // Patients have same KanaName will be counted as duplicate.
    // Note: This property will be affected in case of real-time update so it will be calculated by front-end.
    public bool IsNameDuplicate { get; set; }
    public string NameDuplicateState => IsNameDuplicate ? "●" : string.Empty;
    // 予約時間
    public string YoyakuTime { get; private set; }
    // 予約名
    public string ReservationName { get; private set; }
    // 受付種別
    public int UketukeSbtId { get; private set; }
    // 受付時間
    public string UketukeTime { get; private set; }
    // 診察開始
    public string SinStartTime { get; private set; }
    // 診察終了
    public string SinEndTime { get; private set; }
    // 精算時間
    public string KaikeiTime { get; private set; }
    // 来院コメント
    public string RaiinCmt { get; private set; }
    // 患者コメント
    public string PtComment { get; private set; }
    // 保険
    public string HokenPatternName { get; private set; }
    // 担当医
    public int TantoId { get; private set; }
    // 診療科
    public int KaId { get; private set; }
    // 前回来院
    public string LastVisitDate { get; private set; }
    public string FirstVisitDate { get; private set; }
    // 主治医
    public string Sname { get; private set; }
    // 備考
    public string RaiinRemark { get; private set; }
    // 資格確認状況
    public string ConfirmationState { get; private set; }
    // 資格確認結果
    public string ConfirmationResult { get; private set; }
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

    private string GetHokenName(int hokenPid, int hokenStartDate, int hokenEndDate, int hokenSbtCd, int hokenKbn,
        int kohi1HokenSbtKbn, string kohi1Houbetu, int kohi2HokenSbtKbn, string kohi2Houbetu,
        int kohi3HokenSbtKbn, string kohi3Houbetu, int kohi4HokenSbtKbn, string kohi4Houbetu)
    {
        if (hokenPid == CommonConstants.InvalidId)
        {
            return string.Empty;
        }

        string hokenName = hokenPid.ToString().PadLeft(3, '0') + ". ";
        if (IsExpirated())
        {
            hokenName = "×" + hokenName;
        }

        string prefix = string.Empty;
        string postfix = string.Empty;
        if (hokenSbtCd == 0)
        {
            switch (hokenKbn)
            {
                case 0:
                    hokenName += "自費";
                    break;
                case 11:
                    hokenName += "労災（短期給付）";
                    break;
                case 12:
                    hokenName += "労災（傷病年金）";
                    break;
                case 13:
                    hokenName += "労災（アフターケア）";
                    break;
                case 14:
                    hokenName += "自賠責";
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (hokenSbtCd < 0)
            {
                return hokenName;
            }

            string subHokenSbtCd = hokenSbtCd.ToString().PadRight(3, '0');
            int firstNum = Int32.Parse(subHokenSbtCd[0].ToString());
            int secondNum = Int32.Parse(subHokenSbtCd[1].ToString());
            int thirNum = Int32.Parse(subHokenSbtCd[2].ToString());
            switch (firstNum)
            {
                case 1:
                    hokenName += "社保";
                    break;
                case 2:
                    hokenName += "国保";
                    break;
                case 3:
                    hokenName += "後期";
                    break;
                case 4:
                    hokenName += "退職";
                    break;
                case 5:
                    hokenName += "公費";
                    break;
            }

            if (secondNum > 0)
            {
                if (thirNum == 1)
                {
                    prefix += "単独";
                }
                else
                {
                    prefix += thirNum + "併";
                }

                if (kohi1HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix))
                    {
                        postfix += "+";
                    }
                    if (kohi1HokenSbtKbn != 2)
                    {
                        postfix += kohi1Houbetu;
                    }
                    else
                    {
                        postfix += "マル長";
                    }
                }
                if (kohi2HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix))
                    {
                        postfix += "+";
                    }
                    if (kohi2HokenSbtKbn != 2)
                    {
                        postfix += kohi2Houbetu;
                    }
                    else
                    {
                        postfix += "マル長";
                    }
                }
                if (kohi3HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix))
                    {
                        postfix += "+";
                    }
                    if (kohi3HokenSbtKbn != 2)
                    {
                        postfix += kohi3Houbetu;
                    }
                    else
                    {
                        postfix += "マル長";
                    }
                }
                if (kohi4HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix))
                    {
                        postfix += "+";
                    }
                    if (kohi4HokenSbtKbn != 2)
                    {
                        postfix += kohi4Houbetu;
                    }
                    else
                    {
                        postfix += "マル長";
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(postfix))
        {
            hokenName = hokenName + prefix + "(" + postfix + ")";
        }
        else
        {
            hokenName = hokenName + prefix;
        }

        string sBuff = "";
        if (hokenStartDate > 0)
        {
            sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate(hokenStartDate));
        }
        else
        {
            sBuff = string.Format("{0, -11}", " ");
        }

        sBuff += " ～ ";

        if (hokenEndDate > 0 && hokenEndDate < 99999999)
        {
            sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate(hokenEndDate));
        }
        else
        {
            sBuff += string.Format("{0, -11}", " ");
        }

        return hokenName + " " + sBuff;

        bool IsExpirated()
        {
            return !(hokenStartDate <= SinDate && hokenEndDate >= SinDate);
        }
    }
    public int HokenPid { get; private set; }

}
