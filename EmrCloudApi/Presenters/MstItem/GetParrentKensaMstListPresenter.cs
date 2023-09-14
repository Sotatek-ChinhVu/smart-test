using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetParrentKensaMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetParrentKensaMstListPresenter : IGetParrentKensaMstOutputPort
    {
        public Response<GetParrentKensaMstListResponse> Result { get; private set; } = default!;

        public void Complete(GetParrentKensaMstOutputData outputData)
        {
            Result = new Response<GetParrentKensaMstListResponse>()
            {
                Data = new GetParrentKensaMstListResponse(outputData.KensaMsts),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetParrentKensaMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetParrentKensaMstStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
