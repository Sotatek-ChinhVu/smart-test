using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.ReorderSetMstList;

namespace Interactor.SetMst;

public class ReorderSetMstListInteractor : IReorderSetMstInputPort
{
    private readonly int _userId = TempIdentity.UserId;
    private readonly ISetMstRepository _setMstRepository;
    public ReorderSetMstListInteractor(ISetMstRepository setMstRepository)
    {
        _setMstRepository = setMstRepository;
    }
    public ReorderSetMstOutputData Handle(ReorderSetMstInputData reorderSetMstInputData)
    {
        try
        {
            if (reorderSetMstInputData.DragSetCd != 0)
            {
                if (_setMstRepository.ReorderSetMst(_userId, reorderSetMstInputData.HpId, reorderSetMstInputData.DragSetCd, reorderSetMstInputData.DropSetCd))
                {
                    return new ReorderSetMstOutputData(ReorderSetMstStatus.Successed);
                }
            }
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
        catch
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
    }
}
