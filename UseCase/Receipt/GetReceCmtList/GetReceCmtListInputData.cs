﻿using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceCmt;

public class GetReceCmtListInputData : IInputData<GetReceCmtListOutputData>
{
    public GetReceCmtListInputData(int hpId, int sinYm, long ptId, int hokenId, int sinDate)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
        HokenId = hokenId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }

    public int SinDate { get; private set; }
}
