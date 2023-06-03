using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.GetList;
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
            var result = _setMstRepository.ReorderSetMst(reorderSetMstInputData.UserId, reorderSetMstInputData.HpId, reorderSetMstInputData.DragSetCd, reorderSetMstInputData.DropSetCd);
            if (result.status)
            {
                return new ReorderSetMstOutputData(BuildTreeSetKbn(result.setMstModels), ReorderSetMstStatus.Successed);
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

    private List<GetSetMstListOutputItem> BuildTreeSetKbn(List<SetMstModel>? datas)
    {
        List<GetSetMstListOutputItem> result = new();
        var topNodes = datas?.Where(c => c.Level2 == 0 && c.Level3 == 0);
        if (topNodes?.Any() != true) { return result; }
        var obj = new object();

        Parallel.ForEach(topNodes, item =>
        {
            var node = new GetSetMstListOutputItem(
                item.HpId,
                item.SetCd,
                item.SetKbn,
                item.SetKbnEdaNo,
                item.GenerationId,
                item.Level1,
                item.Level2,
                item.Level3,
                item.SetName,
                item.WeightKbn,
                item.Color,
                item.IsGroup,
                datas?.Where(c => c.Level1 == item.Level1 && c.Level2 != 0 && c.Level3 == 0)?
                        .Select(c => new GetSetMstListOutputItem(
                            c.HpId,
                            c.SetCd,
                            c.SetKbn,
                            c.SetKbnEdaNo,
                            c.GenerationId,
                            c.Level1,
                            c.Level2,
                            c.Level3,
                            c.SetName,
                            c.WeightKbn,
                            c.Color,
                            c.IsGroup,
                            datas.Where(m => m.Level3 != 0 && m.Level1 == item.Level1 && m.Level2 == c.Level2)?
                                .Select(c => new GetSetMstListOutputItem(
                                    c.HpId,
                                    c.SetCd,
                                    c.SetKbn,
                                    c.SetKbnEdaNo,
                                    c.GenerationId,
                                    c.Level1,
                                    c.Level2,
                                    c.Level3,
                                    c.SetName,
                                    c.WeightKbn,
                                    c.Color,
                                    c.IsGroup,
                                    new List<GetSetMstListOutputItem>() ?? new List<GetSetMstListOutputItem>()
                                )).OrderBy(s => s.Level1).ThenBy(s => s.Level2).ThenBy(s => s.Level3).ToList() ?? new List<GetSetMstListOutputItem>()
                        )).OrderBy(s => s.Level1).ThenBy(s => s.Level2).ThenBy(s => s.Level3).ToList() ?? new List<GetSetMstListOutputItem>()
                );

            lock (obj)
            {
                result.Add(node);
            }
        });
        return result.OrderBy(s => s.Level1)
      .ThenBy(s => s.Level2)
      .ThenBy(s => s.Level3).ToList();
    }
}
