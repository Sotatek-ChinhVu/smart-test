using Domain.Models.RaiinListSetting;
using UseCase.RaiinListSetting.SaveRaiinListSetting;

namespace Interactor.RaiinListSetting
{
    public class SaveRaiinListSettingInteractor : ISaveRaiinListSettingInputPort
    {
        private readonly IRaiinListSettingRepository _raiinListSettingRepository;

        public SaveRaiinListSettingInteractor(IRaiinListSettingRepository raiinListSettingRepository)
        {
            _raiinListSettingRepository = raiinListSettingRepository;
        }

        public SaveRaiinListSettingOutputData Handle(SaveRaiinListSettingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0) return new SaveRaiinListSettingOutputData(SaveRaiinListSettingStatus.InvalidHpId);
                if (inputData.UserId <= 0) return new SaveRaiinListSettingOutputData(SaveRaiinListSettingStatus.InvalidUserId);

                bool result = _raiinListSettingRepository.SaveRaiinListSetting(inputData.HpId, inputData.RaiinListSettings, inputData.UserId);

                if (result) return new SaveRaiinListSettingOutputData(SaveRaiinListSettingStatus.Successful);
                else return new SaveRaiinListSettingOutputData(SaveRaiinListSettingStatus.Failed);
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
            }
        }
    }
}
