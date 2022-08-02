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

                Data = new GetInsuranceMstResponse()
                {
                    InsuranceMst = output.InsuranceMstData
                },
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case GetInsuranceMstStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidPtId;
                    break;
                case GetInsuranceMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidHpId;
                    break;
                case GetInsuranceMstStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidSinDate;
                    break;
                case GetInsuranceMstStatus.InvalidHokenId:
                    Result.Message = ResponseMessage.GetInsuranceListInvalidSinDate;
                    break;
                case GetInsuranceMstStatus.Successed:
                    Result.Message = ResponseMessage.GetInsuranceListSuccessed;
                    break;
            }
        }
    }
}
