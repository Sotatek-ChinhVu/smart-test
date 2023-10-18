using Domain.Models.RaiinKubunMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.RaiinKubunMst.Save;

namespace Interactor.RaiinKubunMst
{
    public class SaveDataKubunSettingInteractor : ISaveDataKubunSettingInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveDataKubunSettingInteractor(ITenantProvider tenantProvider, IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveDataKubunSettingOutputData Handle(SaveDataKubunSettingInputData inputData)
        {
            try
            {
                if (inputData.RaiinKubunMstModels != null && inputData.RaiinKubunMstModels.Any())
                {
                    var result = _raiinKubunMstRepository.SaveDataKubunSetting(inputData.RaiinKubunMstModels, inputData.UserId, inputData.HpId);
                    return new SaveDataKubunSettingOutputData(result);
                }
                return new SaveDataKubunSettingOutputData(new List<string>() { KubunSettingConstant.Nodata });
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _raiinKubunMstRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
