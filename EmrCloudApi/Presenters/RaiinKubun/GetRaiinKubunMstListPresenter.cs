using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.GetList;

namespace EmrCloudApi.Presenters.RaiinKubun
{
    public class GetRaiinKubunMstListPresenter : IGetRaiinKubunMstListOutputPort
    {
        public Response<GetRaiinKubunMstListResponse> Result { get; private set; } = default!;
        public void Complete(GetRaiinKubunMstListOutputData outputData)
        {
            Result = new Response<GetRaiinKubunMstListResponse>()
            {
                Data = new GetRaiinKubunMstListResponse()
                {
                    RaiinKubunMstList = outputData.RaiinKubunList
                },
                Message = ResponseMessage.Success,
                Status = 1
            };
        }
    }
}
