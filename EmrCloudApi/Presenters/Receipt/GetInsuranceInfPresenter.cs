using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListReceInf;

namespace EmrCloudApi.Presenters.Receipt
{
    public class GetInsuranceInfPresenter : IGetInsuranceInfOutputPort
    {
        public Response<GetInsuranceInfResponse> Result { get; private set; } = new();

        public void Complete(GetInsuranceInfOutputData outputData)
        {
            Result.Data = new GetInsuranceInfResponse(outputData.InsuranceInfDtos);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetInsuranceInfStatus status) => status switch
        {
            GetInsuranceInfStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
