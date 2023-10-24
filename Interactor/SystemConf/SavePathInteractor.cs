using Domain.Models.SystemConf;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SystemConf.SavePath;

namespace Interactor.SystemConf
{
    public class SavePathInteractor : ISavePathInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SavePathInteractor(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository)
        {
            _systemConfRepository = systemConfRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SavePathOutputData Handle(SavePathInputData inputData)
        {
            try
            {
                var result = _systemConfRepository.SavePathConfOnline(inputData.HpId, inputData.UserId, inputData.SystemConfListXmlPathModels);
                if (result)
                {
                    return new SavePathOutputData(SavePathStatus.Successed);
                }

                return new SavePathOutputData(SavePathStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
