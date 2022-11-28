using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using EmrCloudApi.Tenant.Responses.ApprovalInfo;
using UseCase.ApprovalInfo.UpdateApprovalInfList;

namespace EmrCloudApi.Tenant.Presenters.ApprovalInfo 
{
    public class UpdateApprovalInfListPresenter : IUpdateApprovalInfListOutputPort
    {
        public Response<UpdateApprovalInfListResponse> Result { get; private set; } = default!;
        public void Complete(UpdateApprovalInfListOutputData outputData)
        {
            Result = new Response<UpdateApprovalInfListResponse>()
            {
                Data = new UpdateApprovalInfListResponse(outputData.Status == UpdateApprovalInfListStatus.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private static string GetMessage(UpdateApprovalInfListStatus status) => status switch
        {
            UpdateApprovalInfListStatus.Success => ResponseMessage.Success,
            UpdateApprovalInfListStatus.ApprovalInfoListInputNoData => ResponseMessage.ApprovalInfoListInputNoData,
            UpdateApprovalInfListStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
