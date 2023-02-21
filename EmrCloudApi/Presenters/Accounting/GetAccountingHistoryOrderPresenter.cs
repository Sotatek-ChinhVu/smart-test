using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetHistoryOrder;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingHistoryOrderPresenter : IGetAccountingHistoryOrderOutputPort
    {
        public Response<GetAccountingHistoryOrderResponse> Result { get; private set; } = new();
        public void Complete(GetAccountingHistoryOrderOutputData outputData)
        {
            Result = new Response<GetAccountingHistoryOrderResponse>()
            {
                Data = new GetAccountingHistoryOrderResponse(outputData.HistoryOrderDtos),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetAccountingHistoryOrderStatus.NoData:
                    Result.Message = ResponseMessage.GetMedicalExaminationNoData;
                    break;
                case GetAccountingHistoryOrderStatus.Successed:
                    Result.Message = ResponseMessage.GetMedicalExaminationSuccessed;
                    break;
                case GetAccountingHistoryOrderStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}