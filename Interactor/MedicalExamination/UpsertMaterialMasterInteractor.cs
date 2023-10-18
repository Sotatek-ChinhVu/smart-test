using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.UpsertMaterialMaster;

namespace Interactor.MedicalExamination
{
    public class UpsertMaterialMasterInteractor : IUpsertMaterialMasterInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        public UpsertMaterialMasterInteractor(ITenantProvider tenantProvider, IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }
        public UpsertMaterialMasterOutputData Handle(UpsertMaterialMasterInputData inputData)
        {
            try
            {
                if (_mstItemRepository.UpsertMaterialMaster(inputData.HpId, inputData.UserId, inputData.MaterialMasters))
                {
                    return new UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus.Success);
                }
                return new UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus.Failed);
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
