using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetKensaCenterMsts;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetKensaCenterMstsPresenter : IGetKensaCenterMstsOutputPort
    {
        public Response<GetKensaCenterMstsResponse> Result { get; private set; } = default!;

        public void Complete(GetKensaCenterMstsOutputData outputData)
        {
            Result = new Response<GetKensaCenterMstsResponse>()
            {
                Data = new GetKensaCenterMstsResponse(outputData.KensaCenterMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetKensaCenterMstsStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetKensaCenterMstsStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
