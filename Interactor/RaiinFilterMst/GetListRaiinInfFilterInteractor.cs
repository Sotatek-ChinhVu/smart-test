using Domain.Models.RaiinFilterMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.RaiinFilterMst.GetListRaiinInf;

namespace Interactor.RaiinFilterMst;

public class GetListRaiinInfFilterInteractor : IGetListRaiinInfFilterInputPort
{
    private readonly IRaiinFilterMstRepository _raiinFilterMstRepository;

    public GetListRaiinInfFilterInteractor(IRaiinFilterMstRepository raiinFilterMstRepository)
    {
        _raiinFilterMstRepository = raiinFilterMstRepository;
    }
    public GetListRaiinInfFilterOutputData Handle(GetListRaiinInfFilterInputData inputData)
    {
        var raiinFilters = _raiinFilterMstRepository.GetList();
        var status = raiinFilters.Any() ? GetListRaiinInfFilterStatus.Success : GetListRaiinInfFilterStatus.NoData;
        return new GetListRaiinInfFilterOutputData(status, raiinFilters);
    }
}
