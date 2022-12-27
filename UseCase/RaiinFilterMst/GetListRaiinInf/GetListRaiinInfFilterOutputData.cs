using Domain.Models.FlowSheet;
using Domain.Models.RaiinFilterMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.GetListRaiinInf;

public class GetListRaiinInfFilterOutputData : IOutputData
{
    public GetListRaiinInfFilterOutputData(GetListRaiinInfFilterStatus status, List<RaiinFilterMstModel> raiinInfFilters)
    {
        Status = status;
        RaiinInfFilters = raiinInfFilters;
    }

    public GetListRaiinInfFilterStatus Status { get; private set; }
    public List<RaiinFilterMstModel> RaiinInfFilters { get; private set; }
}
