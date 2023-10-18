using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MstItem.SaveCompareTenMst;

namespace Interactor.MstItem
{
    public class SaveCompareTenMstInteractor : ISaveCompareTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveCompareTenMstInteractor(ITenantProvider tenantProvider, IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveCompareTenMstOutputData Handle(SaveCompareTenMstInputData inputData)
        {
            try
            {
                if (inputData.ListData.Count <= 0)
                {
                    return new SaveCompareTenMstOutputData(false, SaveCompareTenMstStatus.ListDataEmpty);
                }

                if (inputData.UserId <= 0)
                {
                    return new SaveCompareTenMstOutputData(false, SaveCompareTenMstStatus.InvalidUserId);
                }

                var result = _mstItemRepository.SaveCompareTenMst(inputData.ListData, inputData.Comparison, inputData.UserId);
                return new SaveCompareTenMstOutputData(result, result ? SaveCompareTenMstStatus.Success : SaveCompareTenMstStatus.Faild);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
