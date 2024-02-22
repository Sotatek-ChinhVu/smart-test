using Domain.Models.ListSetMst;
using Entity.Tenant;
using UseCase.MstItem.GetTreeListSet;

namespace Interactor.MstItem
{
    public class GetTreeListSetInteractor : IGetTreeListSetInputPort
    {
        private readonly IListSetMstRepository _treeListSetRepository;
        public GetTreeListSetInteractor(IListSetMstRepository treeListSetRepository)
        {
            _treeListSetRepository = treeListSetRepository;
        }
        public GetTreeListSetOutputData Handle(GetTreeListSetInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new GetTreeListSetOutputData(new List<ListSetMstModel>(), GetTreeListSetStatus.InvalidHpId);

                if (inputData.SinDate < 0)
                    return new GetTreeListSetOutputData(new List<ListSetMstModel>(), GetTreeListSetStatus.InvalidSinDate);

                if (inputData.SetKbn < 0)
                    return new GetTreeListSetOutputData(new List<ListSetMstModel>(), GetTreeListSetStatus.InvalidKouiKbn);

                int generationId = _treeListSetRepository.GetGenerationId(inputData.HpId, inputData.SinDate);

                List<ListSetMstModel> result = new List<ListSetMstModel>();
                List<ListSetMstModel> rootSubs = new List<ListSetMstModel>();
                ListSetMstModel rootLevel = new ListSetMstModel(0, "共通", 0);
                rootSubs.AddRange(GetListTreeSet(inputData.SetKbn, generationId, inputData));
                if (!rootSubs.Any())
                {
                    result.Add(rootLevel);
                    return new GetTreeListSetOutputData(result, GetTreeListSetStatus.DataNotFound);
                }
                rootLevel.Childrens = rootSubs;
                result.Add(rootLevel);
                return new GetTreeListSetOutputData(result, GetTreeListSetStatus.Successed);
            }
            finally
            {
                _treeListSetRepository.ReleaseResource();
            }
        }
        private List<ListSetMstModel> GetListTreeSet(int setKbn, int generationId, GetTreeListSetInputData inputData)
        {
            var lst = new List<ListSetMstModel>();
            lst = _treeListSetRepository.GetListSetMst(inputData.HpId, setKbn, generationId);

            var lstUsageLevel1 = lst.Where(
                item => item.Level1 > 0 &&
                item.Level2 == 0 &&
                item.Level3 == 0 &&
                item.Level4 == 0 &&
                item.Level5 == 0
            ).OrderBy(item => item.Level1)
           .ThenBy(item => item.Level2)
           .ThenBy(item => item.Level3)
           .ThenBy(item => item.Level4)
           .ThenBy(item => item.Level5)
           .ToList();

            List<ListSetMstModel> result = new List<ListSetMstModel>();
            foreach (var item in lstUsageLevel1)
            {
                ListSetMstModel model = item;
                model.Level = 1;
                LoadSubLevel(2, model, lst);
                result.Add(model);
            }

            return result;
        }
        private void LoadSubLevel(int level, ListSetMstModel usageModel, List<ListSetMstModel> listSetMst)
        {
            if (level == 1 || level > 5) return;
            List<ListSetMstModel> subLevel = new List<ListSetMstModel>();
            switch (level)
            {
                case 2:
                    subLevel = listSetMst.Where(
                           item => item.Level1 == usageModel.Level1 &&
                           item.Level2 > 0 &&
                           item.Level3 == 0 &&
                           item.Level4 == 0 &&
                           item.Level5 == 0
                    ).OrderBy(item => item.Level1)
                    .ThenBy(item => item.Level2)
                    .ThenBy(item => item.Level3)
                    .ThenBy(item => item.Level4)
                    .ThenBy(item => item.Level5).ToList();
                    break;

                case 3:
                    subLevel = listSetMst.Where(
                           item => item.Level1 == usageModel.Level1 &&
                           item.Level2 == usageModel.Level2 &&
                           item.Level3 > 0 &&
                           item.Level4 == 0 &&
                           item.Level5 == 0
                    ).OrderBy(item => item.Level1)
                    .ThenBy(item => item.Level2)
                    .ThenBy(item => item.Level3)
                    .ThenBy(item => item.Level4)
                    .ThenBy(item => item.Level5).ToList();
                    break;

                case 4:
                    subLevel = listSetMst.Where(
                           item => item.Level1 == usageModel.Level1 &&
                           item.Level2 == usageModel.Level2 &&
                           item.Level3 == usageModel.Level3 &&
                           item.Level4 > 0 &&
                           item.Level5 == 0
                    ).OrderBy(item => item.Level1)
                    .ThenBy(item => item.Level2)
                    .ThenBy(item => item.Level3)
                    .ThenBy(item => item.Level4)
                    .ThenBy(item => item.Level5).ToList();
                    break;

                case 5:
                    subLevel = listSetMst.Where(
                           item => item.Level1 == usageModel.Level1 &&
                           item.Level2 == usageModel.Level2 &&
                           item.Level3 == usageModel.Level3 &&
                           item.Level4 == usageModel.Level4 &&
                           item.Level5 > 0
                    ).OrderBy(item => item.Level1)
                    .ThenBy(item => item.Level2)
                    .ThenBy(item => item.Level3)
                    .ThenBy(item => item.Level4)
                    .ThenBy(item => item.Level5).ToList();
                    break;
            }

            if (subLevel == null || subLevel.Count == 0) return;

            var usageListSubLevel = new List<ListSetMstModel>();
            foreach (var item in subLevel)
            {
                ListSetMstModel usageModelSubLevel = item;
                usageModelSubLevel.Level = level;
                LoadSubLevel(level + 1, usageModelSubLevel, listSetMst);
                usageListSubLevel.Add(usageModelSubLevel);
            }
            usageModel.Childrens = usageListSubLevel;
        }
    }
}
