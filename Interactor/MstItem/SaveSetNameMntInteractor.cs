using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MstItem.SaveSetNameMnt;

namespace Interactor.MstItem
{
    public class SaveSetNameMntInteractor : ISaveSetNameMntInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveSetNameMntInteractor(ITenantProvider tenantProvider, IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveSetNameMntOutputData Handle(SaveSetNameMntInputData inputData)
        {
            try
            {
                if (inputData.ListData.Count <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.ListDataEmpty);
                }
                if (inputData.Sindate <= 0)
                {
                    return new SaveSetNameMntOutputData(false, SaveSetNameMntStatus.InvalidSinDate);
                }

                var result = _mstItemRepository.SaveSetNameMnt(inputData.ListData, inputData.UserId, inputData.HpId, inputData.Sindate);
                return new SaveSetNameMntOutputData(result, result ? SaveSetNameMntStatus.Success : SaveSetNameMntStatus.Faild);
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
