using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MaxMoney;
using EmrCloudApi.Responses;
using UseCase.MaxMoney.GetMaxMoneyByPtId;

namespace EmrCloudApi.Presenters.MaxMoney
{
    public class GetMaxMoneyByPtIdPresenter : IGetMaxMoneyByPtIdOutputPort
    {
        public Response<GetMaxMoneyByPtIdResponse> Result { get; private set; } = new Response<GetMaxMoneyByPtIdResponse>();
        public void Complete(GetMaxMoneyByPtIdOutputData outputData)
        {
            Result.Data = new GetMaxMoneyByPtIdResponse(outputData.Datas);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetMaxMoneyByPtIdStatus status) => status switch
        {
            GetMaxMoneyByPtIdStatus.Successed => ResponseMessage.Success,
            GetMaxMoneyByPtIdStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetMaxMoneyByPtIdStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            GetMaxMoneyByPtIdStatus.DataNotFound => ResponseMessage.NotFound,
            _ => string.Empty
        };
    }
}
