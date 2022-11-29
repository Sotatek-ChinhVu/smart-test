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
            UpdateApprovalInfListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            UpdateApprovalInfListStatus.InvalidId => ResponseMessage.InvalidHpId,
            UpdateApprovalInfListStatus.InvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            UpdateApprovalInfListStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            UpdateApprovalInfListStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
            UpdateApprovalInfListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            UpdateApprovalInfListStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            UpdateApprovalInfListStatus.InvalidCreateMachine => ResponseMessage.InvalidCreateMachine,
            UpdateApprovalInfListStatus.InvalidCreateId => ResponseMessage.InvalidCreateId,
            UpdateApprovalInfListStatus.InvalidUpdateId => ResponseMessage.InvalidUpdateId,
            UpdateApprovalInfListStatus.InvalidUpdateMachine => ResponseMessage.InvalidUpdateMachine,
            UpdateApprovalInfListStatus.ApprovalInfListExistedInputData => ResponseMessage.ApprovalInfListExistedInputData,
            UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId => ResponseMessage.ApprovalInfListInvalidNoExistedId,
            UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo => ResponseMessage.ApprovalInfListInvalidNoExistedRaiinNo,
            _ => string.Empty
        };
    }
}
