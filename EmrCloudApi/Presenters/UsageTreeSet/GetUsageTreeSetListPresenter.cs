using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UsageTreeSetResponse;
using UseCase.UsageTreeSet.GetTree;

namespace EmrCloudApi.Presenters.UsageTreeSet
{
    public class GetUsageTreeSetListPresenter : IGetUsageTreeSetOutputPort
    {
        public Response<GetUsageTreeSetListResponse> Result { get; private set; } = default!;

        public void Complete(GetUsageTreeSetOutputData outputData)
        {
            Result = new Response<GetUsageTreeSetListResponse>
            {
                Data = new GetUsageTreeSetListResponse()
                {
                    Data = outputData.ListUsageTreeSet
                },
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetUsageTreeStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;

                case GetUsageTreeStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;

                case GetUsageTreeStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;

                case GetUsageTreeStatus.InvalidKouiKbn:
                    Result.Message = ResponseMessage.InvalidUsageKbn;
                    break;

                case GetUsageTreeStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}