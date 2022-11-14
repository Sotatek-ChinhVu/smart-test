using Domain.Models.RaiinFilterMst;
using UseCase.RaiinFilterMst.SaveList;

namespace Interactor.RaiinFilterMst;

public class SaveRaiinFilterMstListInteractor : ISaveRaiinFilterMstListInputPort
{
    private readonly IRaiinFilterMstRepository _raiinFilterMstRepository;

    public SaveRaiinFilterMstListInteractor(IRaiinFilterMstRepository raiinFilterMstRepository)
    {
        _raiinFilterMstRepository = raiinFilterMstRepository;
    }

    public SaveRaiinFilterMstListOutputData Handle(SaveRaiinFilterMstListInputData input)
    {
        _raiinFilterMstRepository.SaveList(input.FilterMsts, input.HpId, input.UserId);
        return new SaveRaiinFilterMstListOutputData(SaveRaiinFilterMstListStatus.Success);
    }
}
