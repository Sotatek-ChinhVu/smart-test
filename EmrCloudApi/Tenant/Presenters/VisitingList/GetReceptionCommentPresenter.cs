using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.VisitingList;
using UseCase.VisitingList.ReceptionComment;

namespace EmrCloudApi.Tenant.Presenters.VisitingList
{
    public class GetReceptionCommentPresenter
    {
        public Response<GetReceptionCommentResponse> Result { get; private set; } = new Response<GetReceptionCommentResponse>();

        public void Complete(GetReceptionCommentOutputData output)
        {
            Result.Data = new GetReceptionCommentResponse(output.ReceptionComments);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetReceptionCommentStatus status) => status switch
        {
            GetReceptionCommentStatus.Success => ResponseMessage.Success,
            GetReceptionCommentStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetReceptionCommentStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty

        };
    }
}
