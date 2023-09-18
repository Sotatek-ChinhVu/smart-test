using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTreeByomeiSet;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTreeByomeiSetPresenter : IGetTreeByomeiSetOutputPort
    {
        public Response<GetTreeByomeiSetResponse> Result { get; private set; } = new Response<GetTreeByomeiSetResponse>();

        public void Complete(GetTreeByomeiSetOutputData outputData)
        {
            Result = new Response<GetTreeByomeiSetResponse>()
            {
                Data = new GetTreeByomeiSetResponse(outputData.Datas),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTreeByomeiSetStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetTreeByomeiSetStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetTreeByomeiSetStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetTreeByomeiSetStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
            }
        }
    }
}
