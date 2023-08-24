﻿using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetYoyakuRaiinInf;

public class GetYoyakuRaiinInfInputData : IInputData<GetYoyakuRaiinInfOutputData>
{
    public GetYoyakuRaiinInfInputData(int hpId, int sinDate, long ptId)
    {
        HpId = hpId;
        SinDate = sinDate;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }
}