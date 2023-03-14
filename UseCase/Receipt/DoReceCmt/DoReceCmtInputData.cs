﻿using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.DoReceCmt;

public class DoReceCmtInputData : IInputData<DoReceCmtOutputData>
{
    public DoReceCmtInputData(int hpId, int sinYm, long ptId, int hokenId)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }
}
