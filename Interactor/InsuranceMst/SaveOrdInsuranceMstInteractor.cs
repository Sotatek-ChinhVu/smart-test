using Domain.Models.InsuranceMst;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.InsuranceMst.SaveOrdInsuranceMst;

namespace Interactor.InsuranceMst
{
    public class SaveOrdInsuranceMstInteractor : ISaveOrdInsuranceMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveOrdInsuranceMstInteractor(ITenantProvider tenantProvider, IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveOrdInsuranceMstOutputData Handle(SaveOrdInsuranceMstInputData inputData)
        {
            if (inputData.HpId < 0)
                return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.InvalidHpId);

            if (inputData.UserId < 0)
                return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.InvalidUserId);
            try
            {
                bool result = _insuranceMstReponsitory.SaveOrdInsuranceMst(inputData.Insurances, inputData.HpId, inputData.UserId);
                if (result)
                    return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.Successful);
                else
                    return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
