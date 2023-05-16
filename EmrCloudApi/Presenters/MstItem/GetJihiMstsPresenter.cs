using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetJihiSbtMstList;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetJihiMstsPresenter : IGetJihiSbtMstListOutputPort
    {
        public Response<GetJihiMstsResponse> Result { get; private set; } = default!;

        public void Complete(GetJihiSbtMstListOutputData outputData)
        {
            Result = new Response<GetJihiMstsResponse>
            {
                Data = new GetJihiMstsResponse(outputData.JihiSbtMstModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetJihiSbtMstListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetJihiSbtMstListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
