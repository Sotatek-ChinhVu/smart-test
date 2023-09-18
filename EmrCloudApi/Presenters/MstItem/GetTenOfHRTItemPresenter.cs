using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenOfItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenOfHRTItemPresenter : IGetTenOfItemOutputPort
    {
        public Response<GetTenOfHRTItemResponse> Result { get; private set; } = default!;

        public void Complete(GetTenOfItemOutputData outputData)
        {
            Result = new Response<GetTenOfHRTItemResponse>()
            {
                Data = new GetTenOfHRTItemResponse(outputData.TenOfItem),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenOfItemStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
