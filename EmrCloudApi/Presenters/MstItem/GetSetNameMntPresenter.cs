using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetSetNameMnt;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetSetNameMntPresenter
    {
        public Response<GetSetNameMntResponse> Result { get; private set; } = default!;

        public void Complete(GetSetNameMntOutPutData outputData)
        {
            Result = new Response<GetSetNameMntResponse>()
            {
                Data = new GetSetNameMntResponse(outputData.SetNameMnts),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetNameMntStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSetNameMntStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetSetNameMntStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
