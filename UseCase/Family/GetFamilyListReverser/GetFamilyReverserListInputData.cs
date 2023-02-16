﻿using UseCase.Core.Sync.Core;

namespace UseCase.Family.GetFamilyReverserList;

public class GetFamilyReverserListInputData : IInputData<GetFamilyReverserListOutputData>
{
    public GetFamilyReverserListInputData(int hpId, long familyPtId, Dictionary<long, string> listPtInf)
    {
        HpId = hpId;
        FamilyPtId = familyPtId;
        ListPtInf = listPtInf;
    }

    public int HpId { get; private set; }

    public long FamilyPtId { get; private set; }

    public Dictionary<long, string> ListPtInf { get; private set; }
}
