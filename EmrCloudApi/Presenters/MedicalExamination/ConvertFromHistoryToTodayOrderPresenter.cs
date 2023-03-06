using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.ConvertFromHistoryTodayOrder;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ConvertFromHistoryToTodayOrderPresenter : IConvertFromHistoryTodayOrderOutputPort
    {
        public Response<ConvertFromHistoryToTodayOrderResponse> Result { get; private set; } = default!;

        public void Complete(ConvertFromHistoryTodayOrderOutputData outputData)
        {
            Result = new Response<ConvertFromHistoryToTodayOrderResponse>()
            {
                Data = new ConvertFromHistoryToTodayOrderResponse(outputData.OdrInfItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ConvertFromHistoryTodayOrderStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ConvertFromHistoryTodayOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ConvertFromHistoryTodayOrderStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case ConvertFromHistoryTodayOrderStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case ConvertFromHistoryTodayOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ConvertFromHistoryTodayOrderStatus.InputNoData:
                    Result.Message = ResponseMessage.InputNoData;
                    break;
                case ConvertFromHistoryTodayOrderStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
