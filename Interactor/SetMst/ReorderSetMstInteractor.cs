using Domain.Models.SetMst;
using Interactor.SetMst.CommonSuperSet;
using UseCase.SetMst.ReorderSetMst;

namespace Interactor.SetMst;

public class ReorderSetMstInteractor : IReorderSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    private readonly ICommonSuperSet _commonSuperSet;
    public ReorderSetMstInteractor(ISetMstRepository setMstRepository, ICommonSuperSet commonSuperSet)
    {
        _setMstRepository = setMstRepository;
        _commonSuperSet = commonSuperSet;
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
            var result = _setMstRepository.ReorderSetMst(reorderSetMstInputData.UserId, reorderSetMstInputData.HpId, reorderSetMstInputData.DragSetCd, reorderSetMstInputData.DropSetCd);
            if (result.status)
            {
                var data = _commonSuperSet.BuildTreeSetKbn(result.setMstModels);
                return new ReorderSetMstOutputData(data, ReorderSetMstStatus.Successed);
            }
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidLevel);
        }
        catch
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
        finally
        {
            _setMstRepository.ReleaseResource();
        }
    }
}
