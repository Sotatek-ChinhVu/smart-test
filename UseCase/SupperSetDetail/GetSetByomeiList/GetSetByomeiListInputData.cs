﻿using UseCase.Core.Sync.Core;

namespace UseCase.SupperSetDetail.GetSetByomeiList;

public class GetSetByomeiListInputData : IInputData<GetSetByomeiListOutputData>
{
    public GetSetByomeiListInputData(int hpId, int setCd)
    {
        HpId = hpId;
        SetCd = setCd;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
}
