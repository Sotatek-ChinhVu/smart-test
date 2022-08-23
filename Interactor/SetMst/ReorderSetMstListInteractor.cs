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
            if (reorderSetMstInputData.SetMstLists != null)
            {
                List<SetMstModel> listSetMst = new();
                int level1 = 1;
                foreach (var itemLevel1 in reorderSetMstInputData.SetMstLists)
                {
                    // set level1 for SetMstModel
                    listSetMst.Add(convertInputDataToModel(itemLevel1, level1, 0, 0));

                    // if item of level1 has childrens, set level2 for SetMstModel
                    if (itemLevel1.Childrens != null)
                    {
                        int level2 = 1;
                        foreach (var itemLevel2 in itemLevel1.Childrens)
                        {
                            listSetMst.Add(convertInputDataToModel(itemLevel2, level1, level2, 0));

                            // if item of level2 has childrens, set level3 for SetMstModel
                            if (itemLevel2.Childrens != null)
                            {
                                int level3 = 1;
                                foreach (var itemLevel3 in itemLevel2.Childrens)
                                {
                                    listSetMst.Add(convertInputDataToModel(itemLevel3, level1, level2, 3));
                                    level3++;

                                    // if item of level3 has childrens, return failed
                                    if (itemLevel3.Childrens?.Count() > 0)
                                    {
                                        return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidLevel);
                                    }
                                }
                            }
                            level2++;
                        }
                    }
                    level1++;
                }
                if (_setMstRepository.ReorderSetMst(_userId, listSetMst))
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

    private SetMstModel convertInputDataToModel(ReorderSetMstInputItem inputItem, int level1, int level2, int level3)
    {
        return new SetMstModel(
                _hpId,
                inputItem.SetCd,
                inputItem.SetKbn,
                inputItem.SetKbnEdaNo,
                inputItem.GenerationId,
                level1,
                level2,
                level3,
                inputItem.SetName,
                inputItem.WeightKbn,
                inputItem.Color,
                0,
                0
            );
    }
}
