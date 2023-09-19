using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenOfKNItem;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenOfKNItemPresenter : IGetTenOfKNItemOutputPort
    {
        public Response<GetTenOfKNItemResponse> Result { get; private set; } = default!;

        public void Complete(GetTenOfKNItemOutputData outputData)
        {
            Result = new Response<GetTenOfKNItemResponse>()
            {
                Data = new GetTenOfKNItemResponse(outputData.LatestSedai),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(GetTenOfKNItemStatus status) => status switch
        {
            GetTenOfKNItemStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
