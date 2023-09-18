﻿using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.GetList;

public class GetInsuranceListInputData : IInputData<GetInsuranceListByIdOutputData>
{
    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public byte SortType { get; private set; }


    public GetInsuranceListInputData(int hpId, long ptId, int sinDate, byte sortType)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        SortType = sortType;
    }
}