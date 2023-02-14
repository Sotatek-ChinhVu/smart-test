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
                Data = new GetAccountingHistoryOrderResponse(outputData.Total, outputData.HistoryOrderModels),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetAccountingHistoryOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidPtId;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSinDate;
                    break;
                case GetAccountingHistoryOrderStatus.NoData:
                    Result.Message = ResponseMessage.GetMedicalExaminationNoData;
                    break;
                case GetAccountingHistoryOrderStatus.Successed:
                    Result.Message = ResponseMessage.GetMedicalExaminationSuccessed;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidStartPage:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidStartIndex;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidPageSize:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidPageSize;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidDeleteCondition:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidDeleteCondition;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidFilterId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidFilterId;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidSearchType:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSearchType;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidSearchCategory:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSearchCategory;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidSearchText:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidSearchText;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidUserId:
                    Result.Message = ResponseMessage.GetMedicalExaminationInvalidUserId;
                    break;
                case GetAccountingHistoryOrderStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetAccountingHistoryOrderStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }

    }
}




