using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ChartApproval;
using UseCase.ChartApproval.GetApprovalInfList;

namespace EmrCloudApi.Presenters.ChartApproval
{
    public class GetApprovalInfListPresenter : IGetApprovalInfListOutputPort
    {
        public Response<GetApprovalInfListResponse> Result { get; private set; } = new Response<GetApprovalInfListResponse>();

        public void Complete(GetApprovalInfListOutputData outputData)
        {
            Result.Data = new GetApprovalInfListResponse(outputData.ApprovalInfList, outputData.Message, outputData.MessageType);
            Result.Message = string.IsNullOrEmpty(outputData.Message) ? GetMessage(outputData.Status) : outputData.Message;
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetApprovalInfListStatus status) => status switch
        {
            GetApprovalInfListStatus.Success => ResponseMessage.Success,
            GetApprovalInfListStatus.NoData => ResponseMessage.NoData,
            GetApprovalInfListStatus.InvalidKaId => ResponseMessage.InvalidKaId,
            GetApprovalInfListStatus.InvalidTantoId => ResponseMessage.InvalidTantoId,
            GetApprovalInfListStatus.InvalidStartDate => ResponseMessage.InvalidStartDate,
            _ => string.Empty
        };
    }
}