using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceptionVisiting;
using UseCase.ReceptionVisiting.Get;

namespace EmrCloudApi.Presenters.Reception
{
    public class GetReceptionVisitingPresenter : IGetReceptionVisitingOutputPort
    {
        public Response<GetReceptionVisitingResponse> Result { get; private set; } = new Response<GetReceptionVisitingResponse>();

        public void Complete(GetReceptionVisitingOutputData output)
        {
            Result.Data = new GetReceptionVisitingResponse(output.ReceptionModel);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetReceptionVisitingStatus status) => status switch
        {
            GetReceptionVisitingStatus.Success => ResponseMessage.Success,
            GetReceptionVisitingStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetReceptionVisitingStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetReceptionVisitingStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty

        };
    }
}

