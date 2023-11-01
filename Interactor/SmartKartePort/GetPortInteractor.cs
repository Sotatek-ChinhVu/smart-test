using Domain.Models.SmartKartePort;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SmartKartePort.GetPort;

namespace Interactor.SmartKartePort
{
    public class GetPortInteractor : IGetPortInputPort
    {
        private readonly ISmartKartePortRepository _smartKartePortRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        public GetPortInteractor(ISmartKartePortRepository smartKartePortRepository, ITenantProvider tenantProvider)
        {
            _smartKartePortRepository = smartKartePortRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public GetPortOutputData Handle(GetPortInputData input)
        {
            try
            {
                var data = _smartKartePortRepository.GetSignalRPort(input.MachineName, input.Ip);
                if (data?.PortNumber > 0)
                {
                    return new GetPortOutputData(data, GetPortStatus.Success);
                }
                else
                {
                    return new GetPortOutputData(new SmartKarteAppSignalRPortModel(), GetPortStatus.Nodata);
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
