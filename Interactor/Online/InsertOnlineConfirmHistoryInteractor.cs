using Domain.Models.Online;
using System.Xml;
using UseCase.Online.InsertOnlineConfirmHistory;

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
            var onlineModelList = inputData.OnlineList.Select(item => new OnlineConfirmationHistoryModel(
                                                                          item.PtId,
                                                                          item.OnlineConfirmationDate,
                                                                          item.ConfirmationType,
                                                                          item.InfoConsFlg,
                                                                          item.ConfirmationResult,
                                                                          item.PrescriptionIssueType,
                                                                          item.UketukeStatus))
                                                      .ToList();
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
