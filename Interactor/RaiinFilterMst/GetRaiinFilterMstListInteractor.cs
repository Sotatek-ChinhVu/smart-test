using Domain.Models.RaiinFilterMst;
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
        var filterMsts = _raiinFilterMstRepository.GetList();
        return new GetRaiinFilterMstListOutputData(GetRaiinFilterMstListStatus.Success, filterMsts);
    }
}
