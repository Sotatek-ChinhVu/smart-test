using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.GetById;

namespace EmrCloudApi.Tenant.Presenters.Insurance
{
    public class GetInsuranceByIdPresenter : IGetInsuranceByIdOutputPort
    {
        public Response<GetInsuranceByIdResponse> Result { get; private set; } = default!;
        public void Complete(GetInsuranceByIdOutputData output)
        {
            Result = new Response<GetInsuranceByIdResponse>()
            {

                Data = new GetInsuranceByIdResponse()
                {
                    Data = output.Data
                },
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {
                case GetInsuranceByIdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.GetInsuranceByIdInvalidHpId;
                    break;
                case GetInsuranceByIdStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetInsuranceByIdInvalidPtId;
                    break;
                case GetInsuranceByIdStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetInsuranceByIdInvalidSinDate;
                    break;
                case GetInsuranceByIdStatus.InvalidHokenPid:
                    Result.Message = ResponseMessage.GetInsuranceByIdInvalidHokenPid;
                    break;
                case GetInsuranceByIdStatus.Successed:
                    Result.Message = ResponseMessage.GetInsuranceByIdSuccessed;
                    break;
            }
        }
    }
}
