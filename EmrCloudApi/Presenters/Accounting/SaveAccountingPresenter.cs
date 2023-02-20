using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.SaveAccounting;

namespace EmrCloudApi.Presenters.Accounting
{
    public class SaveAccountingPresenter
    {
        public Response<SaveAccountingResponse> Result { get; private set; } = new();
        public void Complete(SaveAccountingOutputData outputData)
        {
            Result.Data = new SaveAccountingResponse(outputData.Status == SaveAccountingStatus.Success);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(SaveAccountingStatus status) => status switch
        {
            SaveAccountingStatus.Success => ResponseMessage.Success,
            SaveAccountingStatus.Failed => ResponseMessage.Failed,
            SaveAccountingStatus.InputDataNull => ResponseMessage.InputDataNull,
            _ => string.Empty
        };
    }
}
