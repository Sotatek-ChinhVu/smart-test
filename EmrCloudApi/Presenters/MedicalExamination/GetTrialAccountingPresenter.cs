using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.TrailAccounting;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetTrialAccountingPresenter : IGetTrialAccountingOutputPort
    {
        public Response<GetTrialAccountingResponse> Result { get; private set; } = new();
        public void Complete(GetTrialAccountingOutputData outputData)
        {
            Result.Data = new GetTrialAccountingResponse(outputData.HokenPatternRate, outputData.SinMeis, outputData.SinHos, outputData.SinGais, outputData.AccountingInf, outputData.WarningMemos);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetTrialAccountingStatus.Successed => ResponseMessage.Success,
            GetTrialAccountingStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
