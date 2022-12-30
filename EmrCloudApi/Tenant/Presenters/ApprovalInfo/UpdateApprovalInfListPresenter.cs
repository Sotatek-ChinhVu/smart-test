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
                Data = new UpdateApprovalInfListResponse(outputData.Status == ApprovalInfConstant.Success),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }
        private static string GetMessage(ApprovalInfConstant status) => status switch
        {
            ApprovalInfConstant.Success => ResponseMessage.Success,
            ApprovalInfConstant.ApprovalInfoListInputNoData => ResponseMessage.ApprovalInfoListInputNoData,
            ApprovalInfConstant.Failed => ResponseMessage.Failed,
            ApprovalInfConstant.ApprovalInfoInvalidHpId => ResponseMessage.InvalidHpId,
            ApprovalInfConstant.ApprovalInfoInvalidId => ResponseMessage.InvalidId,
            ApprovalInfConstant.ApprovalInfoInvalidIsDeleted => ResponseMessage.InvalidIsDeleted,
            ApprovalInfConstant.ApprovalInfoInvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            ApprovalInfConstant.ApprovalInfoInvalidPtId => ResponseMessage.InvalidPtId,
            ApprovalInfConstant.ApprovalInfoInvalidSinDate => ResponseMessage.InvalidSinDate,
            ApprovalInfConstant.ApprovalInfListExistedInputData => ResponseMessage.ApprovalInfListExistedInputData,
            ApprovalInfConstant.ApprovalInfListInvalidNoId => ResponseMessage.ApprovalInfListInvalidNoExistedId,
            ApprovalInfConstant.ApprovalInfListInvalidNoRaiinNo => ResponseMessage.ApprovalInfListInvalidNoExistedRaiinNo,
            _ => string.Empty
        };
    }
}