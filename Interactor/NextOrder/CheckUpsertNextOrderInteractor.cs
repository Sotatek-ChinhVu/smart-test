using Domain.Models.NextOrder;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.NextOrder.Check;

namespace Interactor.NextOrder
{
    public class CheckUpsertNextOrderInteractor : ICheckUpsertNextOrderInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public CheckUpsertNextOrderInteractor(ITenantProvider tenantProvider, INextOrderRepository nextOrderRepository)
        {
            _nextOrderRepository = nextOrderRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public CheckUpsertNextOrderOutputData Handle(CheckUpsertNextOrderInputData inputData)
        {
            try
            {
                var existed = _nextOrderRepository.CheckUpsertNextOrder(inputData.HpId, inputData.PtId, inputData.RsvDate);

                if (existed)
                {
                    return new CheckUpsertNextOrderOutputData(CheckUpsertNextOrderStatus.InValid);
                }

                return new CheckUpsertNextOrderOutputData(CheckUpsertNextOrderStatus.Valid);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _nextOrderRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
