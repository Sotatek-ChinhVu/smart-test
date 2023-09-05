using Domain.Models.Online;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using UseCase.Online;
using UseCase.Online.InsertOnlineConfirmHistory;
using UseCase.Online.QualificationConfirmation;

namespace Interactor.Online;

public class InsertOnlineConfirmHistoryInteractor : IInsertOnlineConfirmHistoryInputPort
{
    private readonly IOnlineRepository _onlineRepository;

    public InsertOnlineConfirmHistoryInteractor(IOnlineRepository onlineRepository)
    {
        _onlineRepository = onlineRepository;
    }

    public InsertOnlineConfirmHistoryOutputData Handle(InsertOnlineConfirmHistoryInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult != InsertOnlineConfirmHistoryStatus.ValidateSuccess)
            {
                return new InsertOnlineConfirmHistoryOutputData(validateResult);
            }

            List<OnlineConfirmationHistoryModel> onlineModelList = new();
            foreach (var item in inputData.OnlineList)
            {
                var xmlObject = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(item.ConfirmationResult)) as QCXmlMsgResponse;
                if (xmlObject != null)
                {
                    var onlineConfirmationDate = xmlObject.MessageHeader.ProcessExecutionTime;
                    onlineModelList.Add(new OnlineConfirmationHistoryModel(0,
                                                                           item.PtId,
                                                                           TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(onlineConfirmationDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)),
                                                                           item.ConfirmationType,
                                                                           item.InfoConsFlg,
                                                                           item.ConfirmationResult,
                                                                           item.PrescriptionIssueType,
                                                                           item.UketukeStatus));
                }
            }
            if (_onlineRepository.InsertOnlineConfirmHistory(inputData.UserId, onlineModelList))
            {
                return new InsertOnlineConfirmHistoryOutputData(InsertOnlineConfirmHistoryStatus.Successed);
            }
            return new InsertOnlineConfirmHistoryOutputData(InsertOnlineConfirmHistoryStatus.Failed);
        }
        finally
        {
            _onlineRepository.ReleaseResource();
        }
    }

    private InsertOnlineConfirmHistoryStatus ValidateData(InsertOnlineConfirmHistoryInputData inputData)
    {
        foreach (var item in inputData.OnlineList)
        {
            if (string.IsNullOrEmpty(item.ConfirmationResult))
            {
                return InsertOnlineConfirmHistoryStatus.InvalidConfirmationResult;
            }
            try
            {
                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml(item.ConfirmationResult);
            }
            catch
            {
                return InsertOnlineConfirmHistoryStatus.InvalidConfirmationResult;
            }
        }
        return InsertOnlineConfirmHistoryStatus.ValidateSuccess;
    }
}
