using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ReceptionInsurance;
using UseCase.Insurance.ValidPatternExpirated;

namespace EmrCloudApi.Tenant.Presenters.ReceptionInsurance
{
    public class ValidPatternExpiratedPresenter : IValidPatternExpiratedOutputPort
    {
        public Response<ValidPatternExpiratedResponse> Result { get; private set; } = default!;

        public void Complete(ValidPatternExpiratedOutputData outputData)
        {
            Result = new Response<ValidPatternExpiratedResponse>
            {
                Data = new ValidPatternExpiratedResponse(outputData.Result, outputData.Message),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ValidPatternExpiratedStatus.ValidPatternExpiratedSuccess:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
