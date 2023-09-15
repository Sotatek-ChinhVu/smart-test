using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenOfIGEItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenOfIGEItemPresenter : IGetTenOfIGEItemOutputPort
    {
        public Response<GetTenOfIGEItemResponse> Result { get; private set; } = default!;

        public void Complete(GetTenOfIGEItemOutputData outputData)
        {
            Result = new Response<GetTenOfIGEItemResponse>()
            {
                Data = new GetTenOfIGEItemResponse(outputData.TenOfIGEItem),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenOfIGEItemStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTenOfIGEItemStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
