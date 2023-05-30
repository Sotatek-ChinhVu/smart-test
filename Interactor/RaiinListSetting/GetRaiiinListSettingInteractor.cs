using Domain.Models.Document;
using Domain.Models.RaiinListSetting;
using UseCase.RaiinListSetting.GetRaiiinListSetting;

namespace Interactor.RaiinListSetting
{
    public class GetRaiiinListSettingInteractor : IGetRaiiinListSettingInputPort
    {
        private readonly IRaiinListSettingRepository _raiinListSettingRepository;

        public GetRaiiinListSettingInteractor(IRaiinListSettingRepository raiinListSettingRepository)
        {
            _raiinListSettingRepository = raiinListSettingRepository;
        }

        public GetRaiiinListSettingOutputData Handle(GetRaiiinListSettingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.InvalidHpId, new());
                }

                var data = _raiinListSettingRepository.GetRaiiinListSetting(inputData.HpId);

                if (data.Any())
                {
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.Successful, data);
                }
                else
                {
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.NoData, data);
                }
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
            }
        }
    }
}
