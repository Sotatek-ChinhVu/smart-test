using Domain.Models.Online;
using Helper.Common;
using Helper.Constants;
using UseCase.Online.InsertOnlineConfirmation;

namespace Interactor.Online
{
    public class InsertOnlineConfirmationInteractor : IInsertOnlineConfirmationInputPort
    {
        private readonly IOnlineRepository _onlineRepository;

        public InsertOnlineConfirmationInteractor(IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
        }

        public InsertOnlineConfirmationOutputData Handle(InsertOnlineConfirmationInputData inputData)
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

            if (_onlineRepository.SaveOnlineConfirmation(inputData.UserId, qualificationInf, ModelStatus.Added))
            {
                message = "確認対象患者選択";
                return new InsertOnlineConfirmationOutputData(message, InsertOnlineConfirmationStatus.Successed); ;
            }

            return new InsertOnlineConfirmationOutputData(message, InsertOnlineConfirmationStatus.Failed);
        }
    }
}
