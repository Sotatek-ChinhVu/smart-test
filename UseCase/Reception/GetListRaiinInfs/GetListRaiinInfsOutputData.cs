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
    public GetListRaiinInfsOutputData(GetListRaiinInfsStatus status, List<ReceptionModel> raiinInfs)
    {
        Status = status;
        RaiinInfs = raiinInfs;
    }

    public GetListRaiinInfsStatus Status { get; private set; }
    public List<ReceptionModel> RaiinInfs { get; private set; }
}

