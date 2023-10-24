using EmrCloudApi.Constants;
using EmrCloudApi.Responses.AccountDue;
using EmrCloudApi.Responses;
using UseCase.Online.InsertOnlineConfirmHistory;
using EmrCloudApi.Responses.Online;

namespace EmrCloudApi.Presenters.Online;

public class InsertOnlineConfirmHistoryPresenter : IInsertOnlineConfirmHistoryOutputPort
{
    public Response<InsertOnlineConfirmHistoryResponse> Result { get; private set; } = new();

    public void Complete(InsertOnlineConfirmHistoryOutputData output)
    {
        Result.Data = new InsertOnlineConfirmHistoryResponse(output.IdList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(InsertOnlineConfirmHistoryStatus status) => status switch
    {
        InsertOnlineConfirmHistoryStatus.Successed => ResponseMessage.Success,
        InsertOnlineConfirmHistoryStatus.Failed => ResponseMessage.Failed,
        InsertOnlineConfirmHistoryStatus.InvalidConfirmationResult => ResponseMessage.InvalidConfirmationResult,
        InsertOnlineConfirmHistoryStatus.InvalidOnlineConfirmationDate => ResponseMessage.InvalidOnlineConfirmationDate,
        _ => string.Empty
    };
}
