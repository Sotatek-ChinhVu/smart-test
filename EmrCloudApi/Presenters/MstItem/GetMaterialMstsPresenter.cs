using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetMaterialMsts;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetMaterialMstsPresenter : IGetMaterialMstsOutputPort
    {
        public Response<GetMaterialMstsResponse> Result { get; private set; } = default!;

        public void Complete(GetMaterialMstsOutputData outputData)
        {
            Result = new Response<GetMaterialMstsResponse>()
            {
                Data = new GetMaterialMstsResponse(outputData.GetContainerMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetMaterialMstsStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetMaterialMstsStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
