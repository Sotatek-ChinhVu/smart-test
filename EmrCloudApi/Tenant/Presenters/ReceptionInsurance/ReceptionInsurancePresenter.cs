using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ReceptionInsurance;
using UseCase.ReceptionInsurance.Get;

namespace EmrCloudApi.Tenant.Presenters.ReceptionInsurance
{
    public class ReceptionInsurancePresenter : IGetReceptionInsuranceOutputPort
    {
        public Response<ReceptionInsuranceResponse> Result { get; private set; } = default!;

        public void Complete(GetReceptionInsuranceOutputData outputData)
        {
            Result = new Response<ReceptionInsuranceResponse>
            {
                Data = new ReceptionInsuranceResponse(outputData.ListReceptionInsurance),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case GetReceptionInsuranceStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetReceptionInsuranceStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetReceptionInsuranceStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetReceptionInsuranceStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                default:
                    break;
            }

        }
    }
}
