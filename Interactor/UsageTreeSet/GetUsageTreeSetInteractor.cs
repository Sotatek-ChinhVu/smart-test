using Domain.Models.UsageTreeSet;
using System.Collections.ObjectModel;
using UseCase.UsageTreeSet.GetTree;

namespace Interactor.UsageTreeSet
{
    public class GetUsageTreeSetInteractor : IGetUsageTreeSetInputPort
    {
        private readonly IUsageTreeSetRepository _usageTreeSetRepository;

        private readonly List<int> _usageDrug = new List<int>() { 21, 22, 23 };
        private readonly List<int> _usageInject = new List<int>() { 28, 31, 32, 33, 34 };//AIN-4152
        private readonly List<int> _listMedicalManagement = new List<int> { 11, 12, 13 };

        public GetUsageTreeSetInteractor(IUsageTreeSetRepository usageTreeSetRepository)
        {
            _usageTreeSetRepository = usageTreeSetRepository;
        }

        public GetUsageTreeSetOutputData Handle(GetUsageTreeSetInputData inputData)
        {
            if (inputData.HpId < 0)
                return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidHpId);

            if (inputData.SinDate < 0)
                return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidSinDate);

            if (inputData.SetUsageKbn < 0)
                return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidUsageKbn);

            int generationId = _usageTreeSetRepository.GetGenerationId(inputData.SinDate);

            var lstUsage = new List<ListSetMstModel>();

            if (inputData.SetUsageKbn == 0)
                lstUsage = _usageTreeSetRepository.GetAllTanSetInfs(inputData.HpId, generationId);
            else
            {
                if (_usageDrug.Contains(inputData.SetUsageKbn) || _usageInject.Contains(inputData.SetUsageKbn))
                {
                    if (_usageDrug.Contains(inputData.SetUsageKbn))
                        lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageDrug, generationId);
                    else
                        lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageInject, generationId);
                }
                else if (_listMedicalManagement.Contains(inputData.SetUsageKbn))
                    lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _listMedicalManagement, generationId);
                else
                    lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, inputData.SetUsageKbn, generationId);
            }
            
            var lstUsageLevel1 = lstUsage.Where(
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

            var usageList = new List<ListSetMstModel>();
            foreach (var item in lstUsageLevel1)
            {
                ListSetMstModel usageModel = item;
                usageModel.Level = 1;
                //Sub level
                LoadSubLevel(2, usageModel, lstUsage);

                usageList.Add(usageModel);
            }
            if (usageList.Count == 0)
                return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.DataNotFound);

            return new GetUsageTreeSetOutputData(usageList, GetUsageTreeStatus.Successed);
        }

        private void LoadSubLevel(int level, ListSetMstModel usageModel, List<ListSetMstModel> listSetMst)
        {
            if (level == 1 || level > 5) return;
            List<ListSetMstModel> subLevel = null;
            //
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

            //Sub level

            if (subLevel == null || subLevel.Count == 0) return;

            var usageListSubLevel = new List<ListSetMstModel>();
            foreach (var item in subLevel)
            {
                ListSetMstModel usageModelSubLevel = item;

                usageModelSubLevel.Level = level;

                LoadSubLevel(level + 1, usageModelSubLevel, listSetMst);

                usageListSubLevel.Add(usageModelSubLevel);
            }

            //
            var usageItems = new ObservableCollection<ListSetMstModel>(usageListSubLevel);

            usageModel.Childrens = usageItems;
        }
    }
}