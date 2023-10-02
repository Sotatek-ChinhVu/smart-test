using Domain.Models.Document;
using Domain.Models.RaiinListSetting;
using Helper.Extension;
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
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.InvalidHpId, new(), 0, 0);
                }

                var data = _raiinListSettingRepository.GetRaiiinListSetting(inputData.HpId);

                if (data.raiinListMsts.Any())
                {
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.Successful, data.raiinListMsts, data.grpIdMax, data.sortNoMax);
                }
                else
                {
                    return new GetRaiiinListSettingOutputData(GetRaiiinListSettingStatus.NoData, new(), 0, 0);
                }
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
            }
        }
    }
}
