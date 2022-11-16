using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using UseCase.ApprovalInfo.GetApprovalInfList;

namespace EmrCloudApi.Tenant.Presenters.ApprovalInfo
{
    public class GetApprovalInfListPresenter : IGetApprovalInfListOutputPort
    {
        public Response<GetApprovalInfListResponse> Result { get; private set; } = new Response<GetApprovalInfListResponse>();
        public void Complete(GetApprovalInfListOutputData outputData)
        {
            Result = new Response<GetApprovalInfListResponse>()
            {
                Data = new GetApprovalInfListResponse()
                {
                    ApprovalInfList = outputData.ApprovalInfList
                },
                Status = (byte)outputData.Status
            };
        }
    }
}
