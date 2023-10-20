using Domain.Models.RaiinListSetting;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.RaiinListSetting.SaveRaiinListSetting;

namespace Interactor.RaiinListSetting
{
    public class SaveRaiinListSettingInteractor : ISaveRaiinListSettingInputPort
    {
        private readonly IRaiinListSettingRepository _raiinListSettingRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveRaiinListSettingInteractor(ITenantProvider tenantProvider, IRaiinListSettingRepository raiinListSettingRepository)
        {
            _raiinListSettingRepository = raiinListSettingRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _raiinListSettingRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
