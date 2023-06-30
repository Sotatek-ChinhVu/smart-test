using EmrCloudApi.Constants;
using EmrCloudApi.Responses.User;
using EmrCloudApi.Responses;
using UseCase.User.GetListJobMst;

namespace EmrCloudApi.Presenters.User
{
    public class GetListJobMstPresenter : IGetListJobMstOutputPort
    {
        public Response<GetListJobMstResponse> Result { get; private set; } = new Response<GetListJobMstResponse>();

        public void Complete(GetListJobMstOutputData output)
        {
            Result.Data = new GetListJobMstResponse(output.JobMsts);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(GetListJobMstStatus status) => status switch
        {
            GetListJobMstStatus.Successful => ResponseMessage.Success,
            GetListJobMstStatus.NoData => ResponseMessage.NoData,
            GetListJobMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}
