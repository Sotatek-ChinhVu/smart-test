using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetRenkeiMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetRenkeiMstPresenter : IGetRenkeiMstOutputPort
    {
        public Response<GetRenkeiMstResponse> Result { get; private set; } = default!;

        public void Complete(GetRenkeiMstOutputData outputData)
        {
            Result = new Response<GetRenkeiMstResponse>()
            {
                Data = new GetRenkeiMstResponse(outputData.RenkeiMst),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetRenkeiMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetRenkeiMstStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetRenkeiMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetRenkeiMstStatus.InvalidRenkeiId:
                    Result.Message = ResponseMessage.InvalidRenkeiId;
                    break;
            }
        }
    }
}
