using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.ReceptionComment;

namespace EmrCloudApi.Tenant.Presenters.Reception
{
    public class GetReceptionCommentPresenter
    {
        public Response<GetReceptionCommentResponse> Result { get; private set; } = new Response<GetReceptionCommentResponse>();

        public void Complete(GetReceptionCommentOutputData output)
        {
            Result.Data = new GetReceptionCommentResponse(output.ReceptionModel);
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
