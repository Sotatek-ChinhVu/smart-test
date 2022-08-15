using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.GetHeaderSummaryInfo;
using UseCase.HeaderSumaryInfo.Get;

namespace EmrCloudApi.Tenant.Presenters.HeaderSumaryInfo
{
    public class GetHeaderSumaryInfoPresenter : IGetHeaderSummaryInfoOutputPort
    {
        public Response<GetHeaderSummaryInfoResponse> Result { get; private set; } = default!;

        public void Complete(GetHeaderSumaryInfoOutputData outputData)
        {
            Result = new Response<GetHeaderSummaryInfoResponse>()
            {
                Data = new GetHeaderSummaryInfoResponse(null, null, null),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetHeaderSumaryInfoStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case GetHeaderSumaryInfoStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetHeaderSumaryInfoStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetHeaderSumaryInfoStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetHeaderSumaryInfoStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
                case GetHeaderSumaryInfoStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    Result.Data = new GetHeaderSummaryInfoResponse(outputData.Header1Info, outputData.Header2Info, outputData.Notification);
                    break;
            }

        }
    }
}
