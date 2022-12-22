using Domain.Models.UsageTreeSet;
using Helper.Common;
using UseCase.UsageTreeSet.GetTree;

namespace Interactor.UsageTreeSet
{
    public class GetUsageTreeSetInteractor : IGetUsageTreeSetInputPort
    {
        private readonly IUsageTreeSetRepository _usageTreeSetRepository;

        private readonly List<int> _listDug = new List<int> { 20, 21, 22, 23 };
        private readonly List<int> _listInject = new List<int> { 28, 30, 31, 32, 33, 34 };
        private readonly List<int> _usageDrug = new List<int>() { 21, 22, 23 };
        private readonly List<int> _usageInject = new List<int>() { 28, 31, 32, 33, 34 };
        private readonly List<int> _listMedicalManagement = new List<int> { 11, 12, 13 };

        public GetUsageTreeSetInteractor(IUsageTreeSetRepository usageTreeSetRepository)
        {
            _usageTreeSetRepository = usageTreeSetRepository;
        }

        public GetUsageTreeSetOutputData Handle(GetUsageTreeSetInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidHpId);

                if (inputData.SinDate < 0)
                    return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidSinDate);

                if (inputData.KouiKbn < 0)
                    return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.InvalidKouiKbn);

                int setUsageKbn = 0;
                int setDrugKbn = 0;

                if (_listDug.Contains(inputData.KouiKbn))
                {
                    setDrugKbn = 20;
                    setUsageKbn = 21;
                }
                else if (_listInject.Contains(inputData.KouiKbn))
                {
                    setDrugKbn = 31;
                    setUsageKbn = 30;
                }
                else
                    setDrugKbn = OdrUtil.GetGroupKoui(inputData.KouiKbn);

                int generationId = _usageTreeSetRepository.GetGenerationId(inputData.SinDate);

                List<ListSetMstModel> result = new List<ListSetMstModel>();
                if (setDrugKbn != 0)
                    result.AddRange(GetListTreeSet(setDrugKbn, generationId, inputData));

                if (setUsageKbn != 0)
                    result.AddRange(GetListUsageTreeSet(setUsageKbn, generationId, inputData));

                if (!result.Any())
                    return new GetUsageTreeSetOutputData(new List<ListSetMstModel>(), GetUsageTreeStatus.DataNotFound);

                return new GetUsageTreeSetOutputData(result, GetUsageTreeStatus.Successed);
            }
            finally
            {
                _usageTreeSetRepository.ReleaseResource();
            }
        }

        private List<ListSetMstModel> GetListTreeSet(int setDrugKbn, int generationId, GetUsageTreeSetInputData inputData)
        {
            var lstDrug = new List<ListSetMstModel>();
            if (_usageDrug.Contains(setDrugKbn) || _usageInject.Contains(setDrugKbn))
            {
                if (_usageDrug.Contains(setDrugKbn))
                    lstDrug = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageDrug, generationId, inputData.SinDate);
                else
                    lstDrug = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageInject, generationId, inputData.SinDate);
            }
            else if (_listMedicalManagement.Contains(setDrugKbn))
                lstDrug = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _listMedicalManagement, generationId, inputData.SinDate);
            else
                lstDrug = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, setDrugKbn, generationId, inputData.SinDate);

            var lstUsageLevel1 = lstDrug.Where(
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
                LoadSubLevel(2, model, lstDrug);
                result.Add(model);
            }

            return result;
        }

        private List<ListSetMstModel> GetListUsageTreeSet(int setUsageKbn, int generationId, GetUsageTreeSetInputData inputData)
        {
            var lstUsage = new List<ListSetMstModel>();
            if (_usageDrug.Contains(setUsageKbn) || _usageInject.Contains(setUsageKbn))
            {
                if (_usageDrug.Contains(setUsageKbn))
                    lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageDrug, generationId, inputData.SinDate);
                else
                    lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, _usageInject, generationId, inputData.SinDate);
            }
            else
                lstUsage = _usageTreeSetRepository.GetTanSetInfs(inputData.HpId, setUsageKbn, generationId, inputData.SinDate);

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

            List<ListSetMstModel> result = new List<ListSetMstModel>();
            foreach (var item in lstUsageLevel1)
            {
                ListSetMstModel model = item;
                model.Level = 1;
                LoadSubLevel(2, model, lstUsage);
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