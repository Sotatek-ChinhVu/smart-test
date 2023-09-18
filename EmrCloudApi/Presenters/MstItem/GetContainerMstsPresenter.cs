using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetContainerMsts;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetContainerMstsPresenter : IGetContainerMstsOutputPort
    {
        public Response<GetContainerMstsResponse> Result { get; private set; } = default!;

        public void Complete(GetContainerMstsOutputData outputData)
        {
            Result = new Response<GetContainerMstsResponse>()
            {
                Data = new GetContainerMstsResponse(outputData.ContainerMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetContainerMstsStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetContainerMstsStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
