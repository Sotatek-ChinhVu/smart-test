using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses.MaxMoney;
using EmrCloudApi.Tenant.Responses;
using UseCase.MaxMoney.SaveMaxMoney;

namespace EmrCloudApi.Tenant.Presenters.MaxMoney
{
    public class SaveMaxMoneyPresenter : ISaveMaxMoneyOutputPort
    {
        public Response<SaveMaxMoneyResponse> Result { get; private set; } = new Response<SaveMaxMoneyResponse>();
        public void Complete(SaveMaxMoneyOutputData outputData)
        {
            Result.Data = new SaveMaxMoneyResponse()
            {
                State = outputData.Status
            };
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveMaxMoneyStatus status) => status switch
        {
            SaveMaxMoneyStatus.Successful => ResponseMessage.Success,
            SaveMaxMoneyStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveMaxMoneyStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            SaveMaxMoneyStatus.InvalidKohiId => ResponseMessage.InvalidKohiId,
            SaveMaxMoneyStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
