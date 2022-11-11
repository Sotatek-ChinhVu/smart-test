using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MaxMoney;
using UseCase.MaxMoney.GetMaxMoney;

namespace EmrCloudApi.Tenant.Presenters.MaxMoney
{
    public class GetMaxMoneyPresenter : IGetMaxMoneyOutputPort
    {
        public Response<GetMaxMoneyResponse> Result { get; private set; } = new Response<GetMaxMoneyResponse>();
        public void Complete(GetMaxMoneyOutputData outputData)
        {
            Result.Data = new GetMaxMoneyResponse()
            {
                Data = outputData.Data
            };
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetMaxMoneyStatus status) => status switch
        {
            GetMaxMoneyStatus.Successed => ResponseMessage.Success,
            GetMaxMoneyStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetMaxMoneyStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            GetMaxMoneyStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
            GetMaxMoneyStatus.InvalidKohiId => ResponseMessage.InvalidKohiId,
            _ => string.Empty
        };
    }
}
