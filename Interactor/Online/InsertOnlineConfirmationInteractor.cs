using Domain.Models.Online;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Online.InsertOnlineConfirmation;

namespace Interactor.Online
{
    public class InsertOnlineConfirmationInteractor : IInsertOnlineConfirmationInputPort
    {
        private readonly IOnlineRepository _onlineRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public InsertOnlineConfirmationInteractor(ITenantProvider tenantProvider, IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public InsertOnlineConfirmationOutputData Handle(InsertOnlineConfirmationInputData inputData)
        {
            try
            {
                var message = string.Empty;
                //check ArbitraryFileIdentifier
                if (inputData.QCBIResponse.MessageHeader.ArbitraryFileIdentifier != inputData.ArbitraryFileIdentifier)
                    return new InsertOnlineConfirmationOutputData(message, InsertOnlineConfirmationStatus.InvalidArbitraryFileIdentifier);

                var receptionDateTime = CIUtil.StrDateToDate(inputData.QCBIResponse.MessageBody.ReceptionDateTime);
                var qualificationInf = new QualificationInfModel(
                                                inputData.QCBIResponse.MessageBody.ReceptionNumber,
                                                receptionDateTime,
                                                inputData.SinDate,
                                                "0",
                                                string.Empty
                                                );

                if (!_onlineRepository.SaveOnlineConfirmation(inputData.UserId, qualificationInf, ModelStatus.Added))
                {
                    message = "確認対象患者選択";
                    return new InsertOnlineConfirmationOutputData(message, InsertOnlineConfirmationStatus.Failed); ;
                }

                return new InsertOnlineConfirmationOutputData(message, InsertOnlineConfirmationStatus.Successed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _onlineRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
