using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceMst;
using UseCase.InsuranceMst.Get;

namespace EmrCloudApi.Tenant.Presenters.InsuranceMst
{
    public class GetInsuranceMstPresenter : IGetInsuranceMstOutputPort
    {
        public Response<GetInsuranceMstResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceMstOutputData output)
        {
            Result = new Response<GetInsuranceMstResponse>()
            {

                Data = new GetInsuranceMstResponse(output.InsuranceMstData),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case GetInsuranceMstStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetInsuranceMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetInsuranceMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetInsuranceMstStatus.InvalidHokenId:
                    Result.Message = ResponseMessage.InvalidHokenId;
                    break;
                case GetInsuranceMstStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
