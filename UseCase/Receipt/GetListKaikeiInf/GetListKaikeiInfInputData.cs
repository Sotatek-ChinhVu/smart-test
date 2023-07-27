﻿using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListKaikeiInf;

public class GetListKaikeiInfInputData : IInputData<GetListKaikeiInfOutputData>
{
    public GetListKaikeiInfInputData(int hpId, int sinYm, long ptId)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }
}
