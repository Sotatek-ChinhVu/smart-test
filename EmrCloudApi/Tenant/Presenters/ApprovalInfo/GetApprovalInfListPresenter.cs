using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using UseCase.ApprovalInfo.GetApprovalInfList;

namespace EmrCloudApi.Tenant.Presenters.ApprovalInfo;

public class GetApprovalInfListPresenter : IGetApprovalInfListOutputPort
{
    public Response<GetApprovalInfListResponse> Result { get; private set; } = new Response<GetApprovalInfListResponse>();
    public void Complete(GetApprovalInfListOutputData outputData)
    {
        Result.Data = new GetApprovalInfListResponse(outputData.ApprovalInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private string GetMessage(GetApprovalInfListStatus status) => status switch
    {
        GetApprovalInfListStatus.Success => ResponseMessage.Success,
        GetApprovalInfListStatus.Failed => ResponseMessage.Failed,
        GetApprovalInfListStatus.InvalidKaId => ResponseMessage.InvalidKaId,
        GetApprovalInfListStatus.InvalidTantoId => ResponseMessage.InvalidTantoId,
        GetApprovalInfListStatus.InvalidStarDate => ResponseMessage.InvalidStarDate,
        GetApprovalInfListStatus.InvalidEndDate => ResponseMessage.InvalidEndDate,
        _ => string.Empty
    };
}
