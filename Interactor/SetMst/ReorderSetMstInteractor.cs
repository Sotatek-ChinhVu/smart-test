using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.ReorderSetMst;

namespace Interactor.SetMst;

public class ReorderSetMstInteractor : IReorderSetMstInputPort
{
    private readonly int _userId = TempIdentity.UserId;
    private readonly ISetMstRepository _setMstRepository;
    public ReorderSetMstInteractor(ISetMstRepository setMstRepository)
    {
        _setMstRepository = setMstRepository;
    }
    public ReorderSetMstOutputData Handle(ReorderSetMstInputData reorderSetMstInputData)
    {
        try
        {
            if (reorderSetMstInputData.DragSetCd != 0 && _setMstRepository.ReorderSetMst(_userId, reorderSetMstInputData.HpId, reorderSetMstInputData.DragSetCd, reorderSetMstInputData.DropSetCd))
            {
                return new ReorderSetMstOutputData(ReorderSetMstStatus.Successed);
            }
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
        catch
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
    }
}
