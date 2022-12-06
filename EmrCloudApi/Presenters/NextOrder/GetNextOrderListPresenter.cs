using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using UseCase.NextOrder.GetList;

namespace EmrCloudApi.Presenters.NextOrder
{
    public class GetNextOrderListPresenter : IGetNextOrderListOutputPort
    {
        public Response<GetNextOrderListResponse> Result { get; private set; } = default!;

        public void Complete(GetNextOrderListOutputData outputData)
        {
            Result = new Response<GetNextOrderListResponse>()
            {
                Data = new GetNextOrderListResponse(outputData.NextOrders),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetNextOrderListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetNextOrderListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetNextOrderListStatus.InvalidRsvkrtKbn:
                    Result.Message = ResponseMessage.InvalidRsvkrtKbn;
                    break;
                case GetNextOrderListStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetNextOrderListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetNextOrderListStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }

        }
    }
}
