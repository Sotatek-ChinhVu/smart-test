using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KohiHokenMst;
using UseCase.KohiHokenMst.Get;

namespace EmrCloudApi.Tenant.Presenters.KohiHokenMst
{
    public class GetKohiHokenMstPresenter : IGetKohiHokenMstOutputPort
    {
        public Response<GetKohiHokenMstResponse> Result { get; private set; } = default!;
        public void Complete(GetKohiHokenMstOutputData output)
        {
            Result = new Response<GetKohiHokenMstResponse>()
            {
                Data = new GetKohiHokenMstResponse(output.HokenMstModel),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case GetKohiHokenMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetKohiHokenMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetKohiHokenMstStatus.InvalidFutansyaNo:
                    Result.Message = ResponseMessage.InvalidFutansyaNo;
                    break;
                case GetKohiHokenMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
