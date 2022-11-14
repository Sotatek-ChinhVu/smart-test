using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.ReorderSetMst;

namespace Interactor.SetMst;

public class ReorderSetMstInteractor : IReorderSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    public ReorderSetMstInteractor(ISetMstRepository setMstRepository)
    {
        _setMstRepository = setMstRepository;
    }
    public ReorderSetMstOutputData Handle(ReorderSetMstInputData reorderSetMstInputData)
    {
        if (reorderSetMstInputData.HpId <= 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidHpId);
        }
        else if (reorderSetMstInputData.DragSetCd <= 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidDragSetCd);
        }
        else if (reorderSetMstInputData.DropSetCd < 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidDropSetCd);
        }
        try
        {
            if (_setMstRepository.ReorderSetMst(reorderSetMstInputData.UserId, reorderSetMstInputData.HpId, reorderSetMstInputData.DragSetCd, reorderSetMstInputData.DropSetCd))
            {
                return new ReorderSetMstOutputData(ReorderSetMstStatus.Successed);
            }
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidLevel);
        }
        catch
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
    }
}
