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
            Result.Data = new GetTrialAccountingMeiHoGaiResponse(outputData.SinMeis, outputData.SinHos, outputData.SinGais, outputData.AccountingInf, outputData.HokenPatternRate);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetTrialAccountingMeiHoGaiStatus.Successed => ResponseMessage.Success,
            GetTrialAccountingMeiHoGaiStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
