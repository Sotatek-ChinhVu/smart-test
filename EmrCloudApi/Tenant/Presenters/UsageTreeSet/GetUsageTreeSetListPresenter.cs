using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UsageTreeSetResponse;
using UseCase.UsageTreeSet.GetTree;

namespace EmrCloudApi.Tenant.Presenters.UsageTreeSet
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

                case GetUsageTreeStatus.InvalidUsageKbn:
                    Result.Message = ResponseMessage.InvalidUsageKbn;
                    break;

                case GetUsageTreeStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}