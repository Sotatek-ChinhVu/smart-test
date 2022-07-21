using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using UseCase.RaiinKubunMst.GetList;

namespace EmrCloudApi.Tenant.Presenters.RaiinKubun
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
                Message = ResponseMessage.GetRaiinKubunMstListSuccessed,
                Status = 1
            };
        }
    }
}
