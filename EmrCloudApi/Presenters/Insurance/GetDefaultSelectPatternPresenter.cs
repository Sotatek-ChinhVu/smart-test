using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceList;
using UseCase.Insurance.GetDefaultSelectPattern;

namespace EmrCloudApi.Presenters.InsuranceList
{
    public class GetDefaultSelectPatternPresenter : IGetDefaultSelectPatternOutputPort
    {
        public Response<GetDefaultSelectPatternResponse> Result { get; private set; } = default!;
        public void Complete(GetDefaultSelectPatternOutputData output)
        {
            Result = new Response<GetDefaultSelectPatternResponse>()
            {

                Data = new GetDefaultSelectPatternResponse(output.HokenPid),
                Status = (byte)output.Status,
            };
            switch (output.Status)
            {

                case GetDefaultSelectPatternStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case GetDefaultSelectPatternStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetDefaultSelectPatternStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetDefaultSelectPatternStatus.InvalidHistoryPid:
                    Result.Message = ResponseMessage.InvalidHistoryPid;
                    break;
                case GetDefaultSelectPatternStatus.InvalidSelectedHokenPid:
                    Result.Message = ResponseMessage.InvalidSelectedHokenPid;
                    break;
                case GetDefaultSelectPatternStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetDefaultSelectPatternStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}