﻿using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.SaveSetMst;

public class SaveSetMstInputData : IInputData<SaveSetMstOutputData>
{
    public SaveSetMstInputData(long ptId, long raiinNo, int sinDate, int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, string setName, int weightKbn, int color, int isDeleted, int hpId, int userId, bool isGroup = false)
    {
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        SetCd = setCd;
        SetKbn = setKbn;
        SetKbnEdaNo = setKbnEdaNo;
        GenerationId = generationId;
        Level1 = level1;
        Level2 = level2;
        Level3 = level3;
        SetName = setName;
        WeightKbn = weightKbn;
        Color = color;
        IsDeleted = isDeleted;
        IsGroup = isGroup;
        HpId = hpId;
        UserId = userId;
    }
    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public int SetCd { get; private set; }

    public int SetKbn { get; private set; }

    public int SetKbnEdaNo { get; private set; }

    public int GenerationId { get; private set; }

    public int Level1 { get; private set; }

    public int Level2 { get; private set; }

    public int Level3 { get; private set; }

    public string SetName { get; private set; }

    public int WeightKbn { get; private set; }

    public int Color { get; private set; }

    public int IsDeleted { get; private set; }

    public bool IsGroup { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
