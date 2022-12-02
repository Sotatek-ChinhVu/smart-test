using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using UseCase.NextOrder.Get;

namespace EmrCloudApi.Presenters.NextOrder
{
    public class GetNextOrderPresenter : IGetNextOrderOutputPort
    {
        public Response<GetNextOrderResponse> Result { get; private set; } = default!;

        public void Complete(GetNextOrderOutputData outputData)
        {
            Result = new Response<GetNextOrderResponse>()
            {
                Data = new GetNextOrderResponse(outputData.GroupHokenItems, outputData.KarteInf, outputData.ByomeiItems),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetNextOrderStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetNextOrderStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetNextOrderStatus.InvalidRsvkrtNo:
                    Result.Message = ResponseMessage.InvalidRsvkrtNo;
                    break;
                case GetNextOrderStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetNextOrderStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case GetNextOrderStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetNextOrderStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetNextOrderStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }

        }
    }
}
