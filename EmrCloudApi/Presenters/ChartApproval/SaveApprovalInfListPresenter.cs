using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ChartApproval;
using UseCase.ChartApproval.SaveApprovalInfList;

namespace EmrCloudApi.Presenters.ChartApproval
{
    public class SaveApprovalInfListPresenter : ISaveApprovalInfListOutputPort
    {
        public Response<SaveApprovalInfListResponse> Result { get; private set; } = default!;

        public void Complete(SaveApprovalInfListOutputData outputData)
        {
            Result = new Response<SaveApprovalInfListResponse>()
            {
                Data = new SaveApprovalInfListResponse(outputData.Status),
                Status = (int)outputData.Status,
                Message = GetMessage(outputData.Status)
            };
        }

        private string GetMessage(SaveApprovalInfStatus status) => status switch
        {
            SaveApprovalInfStatus.Success => ResponseMessage.Success,
            SaveApprovalInfStatus.Failed => ResponseMessage.Failed,
            SaveApprovalInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveApprovalInfStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            SaveApprovalInfStatus.InvalidInputListApporoval => ResponseMessage.InvalidInputListApporoval,
            _ => string.Empty
        };
    }
}