using EmrCloudApi.Constants;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses;
using UseCase.InsuranceMst.GetHokenSyaMst;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class GetHokenSyaMstPresenter : IGetHokenSyaMstOutputPort
    {
        public Response<GetHokenSyaMstResponse> Result { get; private set; } = default!;

        public void Complete(GetHokenSyaMstOutputData outputData)
        {
            Result = new Response<GetHokenSyaMstResponse>()
            {

                Data = new GetHokenSyaMstResponse(new HokenSyaMstDto(outputData.Data)),
                Status = (byte)outputData.Status,
            };
            switch (outputData.Status)
            {

                case GetHokenSyaMstStatus.InvalidHokenKbn:
                    Result.Message = ResponseMessage.InvalidHokenKbn;
                    break;
                case GetHokenSyaMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetHokenSyaMstStatus.InvalidHokenSyaNo:
                    Result.Message = ResponseMessage.InvalidHokenSyaNo;
                    break;
                case GetHokenSyaMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetHokenSyaMstStatus.Exception:
                    Result.Message = ResponseMessage.ExceptionError;
                    break;
            }
        }
    }
}
