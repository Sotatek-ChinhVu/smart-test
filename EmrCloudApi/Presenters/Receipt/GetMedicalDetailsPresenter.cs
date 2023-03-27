using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.MedicalDetail;

namespace EmrCloudApi.Presenters.Receipt
{
    public class GetMedicalDetailsPresenter : IGetMedicalDetailsOutputPort
    {
        public Response<GetMedicalDetailsResponse> Result { get; private set; } = new();
        public void Complete(GetMedicalDetailsOutputData outputData)
        {
            Result.Data = new GetMedicalDetailsResponse(outputData.SinMeiModels, outputData.Holidays);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetMedicalDetailsStatus status) => status switch
        {
            GetMedicalDetailsStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
