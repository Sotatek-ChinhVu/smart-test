﻿using Domain.Models.RaiinFilterMst;
using UseCase.RaiinFilterMst.GetList;

namespace Interactor.RaiinFilterMst;

public class GetRaiinFilterMstListInteractor : IGetRaiinFilterMstListInputPort
{
    private readonly IRaiinFilterMstRepository _raiinFilterMstRepository;

    public GetRaiinFilterMstListInteractor(IRaiinFilterMstRepository raiinFilterMstRepository)
    {
        _raiinFilterMstRepository = raiinFilterMstRepository;
    }

    public GetRaiinFilterMstListOutputData Handle(GetRaiinFilterMstListInputData inputData)
    {
        try
        {
            var filterMsts = _raiinFilterMstRepository.GetList(inputData.HpId);
            var status = filterMsts.Any() ? GetRaiinFilterMstListStatus.Success : GetRaiinFilterMstListStatus.NoData;
            return new GetRaiinFilterMstListOutputData(status, filterMsts);
        }
        finally
        {
            _raiinFilterMstRepository.ReleaseResource();
        }
    }
}
