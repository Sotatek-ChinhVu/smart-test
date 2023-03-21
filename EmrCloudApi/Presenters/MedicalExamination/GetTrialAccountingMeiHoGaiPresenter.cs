using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.TrailAccounting;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiPresenter : IGetTrialAccountingMeiHoGaiOutputPort
    {
        public Response<GetTrialAccountingMeiHoGaiResponse> Result { get; private set; } = new();
        public void Complete(GetTrialAccountingMeiHoGaiOutputData outputData)
        {
            Result.Data = new GetTrialAccountingMeiHoGaiResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetTrialAccountingMeiHoGaiStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
