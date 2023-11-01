using Domain.Models.SmartKartePort;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SmartKartePort.UpdatePort;

namespace Interactor.SmartKartePort
{
    public class UpdatePortInteractor : IUpdatePortInputPort
    {
        private readonly ISmartKartePortRepository _smartKartePortRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        public UpdatePortInteractor(ISmartKartePortRepository smartKartePortRepository, ITenantProvider tenantProvider)
        {
            _smartKartePortRepository = smartKartePortRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public UpdatePortOutputData Handle(UpdatePortInputData input)
        {
            try
            {
                var update = _smartKartePortRepository.UpdateSignalRPort(input.UserId, input.SignalRPortModel);
                if (update)
                {
                    return new UpdatePortOutputData(UpdatePortStatus.Success);
                }
                else
                {
                    return new UpdatePortOutputData(UpdatePortStatus.Faild);
                }

            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _smartKartePortRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
