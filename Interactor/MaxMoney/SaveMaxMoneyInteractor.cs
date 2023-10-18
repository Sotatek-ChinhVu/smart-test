using Domain.Models.MaxMoney;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MaxMoney.SaveMaxMoney;

namespace Interactor.MaxMoney
{
    public class SaveMaxMoneyInteractor : ISaveMaxMoneyInputPort
    {
        private readonly IMaxmoneyReposiory _maxmoneyReposiory;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveMaxMoneyInteractor(ITenantProvider tenantProvider, IMaxmoneyReposiory maxmoneyReposiory)
        {
            _maxmoneyReposiory = maxmoneyReposiory;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveMaxMoneyOutputData Handle(SaveMaxMoneyInputData inputData)
        {
            try
            {
                if (inputData.PtId < 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidPtId);

                if (inputData.HpId < 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidHpId);

                if (inputData.KohiId <= 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidKohiId);


                bool reuslt = _maxmoneyReposiory.SaveMaxMoney(inputData.ListLimits, inputData.HpId, inputData.PtId, inputData.KohiId, inputData.SinYM, inputData.UserId);
                if (reuslt)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.Successful);

                else return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _maxmoneyReposiory.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
