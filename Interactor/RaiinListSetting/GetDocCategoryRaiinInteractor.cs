using Domain.Models.RaiinListSetting;
using UseCase.RaiinListSetting.GetDocCategory;

namespace Interactor.RaiinListSetting
{
    public class GetDocCategoryRaiinInteractor : IGetDocCategoryRaiinInputPort
    {
        private readonly IRaiinListSettingRepository _raiinListSettingRepository;
        public GetDocCategoryRaiinInteractor(IRaiinListSettingRepository raiinListSettingRepository)
        {
            _raiinListSettingRepository = raiinListSettingRepository;
        }

        public GetDocCategoryRaiinOutputData Handle(GetDocCategoryRaiinInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.InvalidHpId, new());
                }

                var data = _raiinListSettingRepository.GetDocCategoryCollection(inputData.HpId);

                if (data.Any())
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.Successful, data);
                }
                else
                {
                    return new GetDocCategoryRaiinOutputData(GetDocCategoryRaiinStatus.NoData, data);
                }
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
            }
        }
    }
}
