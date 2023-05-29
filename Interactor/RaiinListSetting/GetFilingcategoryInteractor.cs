using Domain.Models.RaiinListSetting;
using UseCase.RaiinListSetting.GetFilingcategory;

namespace Interactor.RaiinListSetting
{
    public class GetFilingcategoryInteractor : IGetFilingcategoryInputPort
    {
        private readonly IRaiinListSettingRepository _raiinListSettingRepository;

        public GetFilingcategoryInteractor(IRaiinListSettingRepository raiinListSettingRepository)
        {
            _raiinListSettingRepository = raiinListSettingRepository;
        }

        public GetFilingcategoryOutputData Handle(GetFilingcategoryInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetFilingcategoryOutputData(GetFilingcategoryStatus.InvalidHpId, new());
                }

                var data = _raiinListSettingRepository.GetFilingcategoryCollection(inputData.HpId);

                if (data.Any())
                {
                    return new GetFilingcategoryOutputData(GetFilingcategoryStatus.Successful, data);
                }
                else
                {
                    return new GetFilingcategoryOutputData(GetFilingcategoryStatus.NoData, data);
                }
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
            }
        }
    }
}
