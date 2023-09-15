using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenOfHRTItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenOfHRTItemPresenter : IGetTenOfHRTItemOutputPort
    {
        public Response<GetTenOfHRTItemResponse> Result { get; private set; } = default!;

        public void Complete(GetTenOfHRTItemOutputData outputData)
        {
            Result = new Response<GetTenOfHRTItemResponse>()
            {
                Data = new GetTenOfHRTItemResponse(outputData.TenOfHRTItem),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenOfHRTItemStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
