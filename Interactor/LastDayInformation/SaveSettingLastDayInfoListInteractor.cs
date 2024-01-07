using Domain.Models.TodayOdr;
using UseCase.LastDayInformation.SaveSettingLastDayInfoList;

namespace Interactor.LastDayInformation
{
    public class SaveSettingLastDayInfoListInteractor : ISaveSettingLastDayInfoListInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public SaveSettingLastDayInfoListInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public SaveSettingLastDayInfoListOutputData Handle(SaveSettingLastDayInfoListInputData inputData)
        {
            try
            {
                bool result = _todayOdrRepository.SaveSettingLastDayInfo(inputData.HpId, inputData.UserId, inputData.OdrDateInfModels);
                if (result)
                {
                    return new SaveSettingLastDayInfoListOutputData(SaveSettingLastDayInfoListStatus.Successed);
                }
                else
                {
                    return new SaveSettingLastDayInfoListOutputData(SaveSettingLastDayInfoListStatus.Failed);
                }
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
