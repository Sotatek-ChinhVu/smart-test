using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.ReceptionComment;

namespace EmrCloudApi.Presenters.Reception
{
    public class GetReceptionCommentPresenter
    {
        public Response<GetReceptionCommentResponse> Result { get; private set; } = new Response<GetReceptionCommentResponse>();

        public void Complete(GetReceptionCommentOutputData output)
        {
            Result.Data = new GetReceptionCommentResponse(output.ReceptionDto);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetReceptionCommentStatus status) => status switch
        {
            GetReceptionCommentStatus.Success => ResponseMessage.Success,
            GetReceptionCommentStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetReceptionCommentStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetReceptionCommentStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty

        };
    }
}
