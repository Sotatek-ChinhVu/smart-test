using Domain.Models.SetMst;
using UseCase.SetMst.GetList;

namespace Interactor.SetMst.CommonSuperSet;

public class CommonSuperSet : ICommonSuperSet
{
    public List<GetSetMstListOutputItem> BuildTreeSetKbn(List<SetMstModel>? datas)
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
                item.IsDeleted,
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
                            c.IsDeleted,
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
                                    c.IsDeleted,
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
