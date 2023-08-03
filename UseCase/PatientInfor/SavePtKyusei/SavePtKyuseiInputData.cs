﻿using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SavePtKyusei;

public class SavePtKyuseiInputData : IInputData<SavePtKyuseiOutputData>
{
    public SavePtKyuseiInputData(int hpId, int userId, List<PtKyuseiItem> ptKyuseiList)
    {
        HpId = hpId;
        UserId = userId;
        PtKyuseiList = ptKyuseiList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<PtKyuseiItem> PtKyuseiList { get; private set; }
}
