using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.ReorderSetMstList;

namespace Interactor.SetMst;

public class ReorderSetMstListInteractor : IReorderSetMstInputPort
{
    private readonly int _hpId = TempIdentity.HpId;
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
            if (reorderSetMstInputData.DragSetMstItem != null && reorderSetMstInputData.DropSetMstItem != null)
            {
                var setMstModelDragItem = ConvertInputDataToModel(reorderSetMstInputData.DragSetMstItem);
                var setMstModelDropItem = ConvertInputDataToModel(reorderSetMstInputData.DropSetMstItem);
                if (_setMstRepository.ReorderSetMst(_userId, setMstModelDragItem, setMstModelDropItem))
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

    private SetMstModel ConvertInputDataToModel(ReorderSetMstInputItem inputItem)
    {
        return new SetMstModel(
                _hpId,
                inputItem.SetCd,
                inputItem.SetKbn,
                inputItem.SetKbnEdaNo,
                inputItem.GenerationId,
                inputItem.Level1,
                inputItem.Level2,
                inputItem.Level3,
                String.Empty,
                0,
                0,
                0,
                0
            );
    }
}
