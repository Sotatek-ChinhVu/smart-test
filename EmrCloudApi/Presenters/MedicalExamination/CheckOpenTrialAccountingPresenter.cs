using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class CheckOpenTrialAccountingPresenter : ICheckOpenTrialAccountingOutputPort
    {
        public Response<CheckOpenTrialAccountingResponse> Result { get; private set; } = new();


        public void Complete(CheckOpenTrialAccountingOutputData outputData)
        {
            Result.Data = new CheckOpenTrialAccountingResponse(outputData.IsHokenPatternSelect, outputData.Type, outputData.ItemName, outputData.LastDaySanteiRiha, outputData.RihaItemName, outputData.SystemSetting, outputData.IsExistYoboItemOnly);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            CheckOpenTrialAccountingStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
