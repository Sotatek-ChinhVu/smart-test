using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
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
            UpdateApprovalInfListStatus.ApprovalInfoInvalidHpId => ResponseMessage.InvalidHpId,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidId => ResponseMessage.InvalidHpId,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidSeqNo => ResponseMessage.InvalidSeqNo,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidPtId => ResponseMessage.InvalidPtId,
            UpdateApprovalInfListStatus.ApprovalInfoInvalidSinDate => ResponseMessage.InvalidSinDate,
            UpdateApprovalInfListStatus.ApprovalInfListExistedInputData => ResponseMessage.ApprovalInfListExistedInputData,
            UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedId => ResponseMessage.ApprovalInfListInvalidNoExistedId,
            UpdateApprovalInfListStatus.ApprovalInfListInvalidNoExistedRaiinNo => ResponseMessage.ApprovalInfListInvalidNoExistedRaiinNo,
            _ => string.Empty
        };
    }
}