using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetAccountingSystemConf;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingConfigPresenter : IGetAccountingConfigOutputPort
    {
        public Response<GetAccountingConfigResponse> Result { get; private set; } = new();
        public void Complete(GetAccountingConfigOutputData outputData)
        {
            Result.Data = new GetAccountingConfigResponse(outputData.AccountingConfigDto);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetAccountingConfigStatus.Successed => ResponseMessage.Success,
            GetAccountingConfigStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
