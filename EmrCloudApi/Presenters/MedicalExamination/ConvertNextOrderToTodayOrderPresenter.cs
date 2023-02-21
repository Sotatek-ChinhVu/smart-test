using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.ConvertNextOrderToTodayOdr;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class ConvertNextOrderToTodayOrderPresenter : IConvertNextOrderToTodayOrdOutputPort
    {
        public Response<ConvertNextOrderToTodayOrderResponse> Result { get; private set; } = default!;

        public void Complete(ConvertNextOrderToTodayOrdOutputData outputData)
        {
            Result = new Response<ConvertNextOrderToTodayOrderResponse>()
            {
                Data = new ConvertNextOrderToTodayOrderResponse(outputData.OdrInfs),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case ConvertNextOrderToTodayOrdStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case ConvertNextOrderToTodayOrdStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case ConvertNextOrderToTodayOrdStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case ConvertNextOrderToTodayOrdStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case ConvertNextOrderToTodayOrdStatus.InvalidOrderInfs:
                    Result.Message = ResponseMessage.InvalidOrderInfs;
                    break;
                case ConvertNextOrderToTodayOrdStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case ConvertNextOrderToTodayOrdStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
