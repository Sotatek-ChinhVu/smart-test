using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.HokenPatternUsed;

namespace EmrCloudApi.Presenters.Insurance
{
    public class CheckHokenPatternUsedPresenter
    {
        public Response<CheckHokenPatternUsedResponse> Result { get; private set; } = default!;
        public void Complete(HokenPatternUsedOutputData output)
        {
            Result = new Response<CheckHokenPatternUsedResponse>()
            {
                Data = new CheckHokenPatternUsedResponse(output.IsUsed, output.Status),
                Status = (int)output.Status
            };

            switch (output.Status)
            {
                case HokenPatternUsedStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case HokenPatternUsedStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case HokenPatternUsedStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case HokenPatternUsedStatus.InvalidHokenPid:
                    Result.Message = ResponseMessage.InvalidPatternHokenPid;
                    break;
                case HokenPatternUsedStatus.Exception:
                    Result.Message = ResponseMessage.ExceptionError;
                    break;
            }
        }
    }
}
