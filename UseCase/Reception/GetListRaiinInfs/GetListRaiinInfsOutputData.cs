﻿using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInfs;

public class GetListRaiinInfsOutputData : IOutputData
{
    public GetListRaiinInfsOutputData(List<GetListRaiinInfsInputItem> raiinInfs, GetListRaiinInfsStatus status)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfsStatus Status { get; private set; }
    public List<GetListRaiinInfsInputItem> RaiinInfs { get; private set; }
}