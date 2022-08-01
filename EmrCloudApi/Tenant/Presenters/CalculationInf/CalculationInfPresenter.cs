using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.CalculationInf;
using UseCase.CalculationInf;

namespace EmrCloudApi.Tenant.Presenters.CalculationInf
{
    public class CalculationInfPresenter : ICalculationInfOutputPort
    {
        public Response<CalculationInfResponse> Result { get; private set; } = default!;

        public void Complete(CalculationInfOutputData outputData)
        {
            Result = new Response<CalculationInfResponse>
            {
                Data = new CalculationInfResponse(outputData.ListCalculation),
                Status = (byte)outputData.Status
            };

            switch (outputData.Status)
            {
                case CalculationInfStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case CalculationInfStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case CalculationInfStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case CalculationInfStatus.DataNotExist:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }

        }
    }
}
