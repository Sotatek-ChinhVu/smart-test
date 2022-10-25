using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using UseCase.AccountDue.GetAccountDueList;

namespace EmrCloudApi.Tenant.Presenters.AccountDue;

public class GetAccountDueListPresenter : IGetAccountDueListOutputPort
{
    public Response<GetAccountDueListResponse> Result { get; private set; } = new();

    public void Complete(GetAccountDueListOutputData output)
    {
        Result.Data = new GetAccountDueListResponse(output.AccountDueModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetAccountDueListStatus status) => status switch
    {
        GetAccountDueListStatus.Successed => ResponseMessage.Success,
        GetAccountDueListStatus.Failed => ResponseMessage.Failed,
        GetAccountDueListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetAccountDueListStatus.InvalidPtId => ResponseMessage.PtInfNotFould,
        GetAccountDueListStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        GetAccountDueListStatus.InvalidpageSize => ResponseMessage.InvalidPageSize,
        GetAccountDueListStatus.InvalidpageIndex => ResponseMessage.InvalidPageIndex,
        _ => string.Empty
    };
}
