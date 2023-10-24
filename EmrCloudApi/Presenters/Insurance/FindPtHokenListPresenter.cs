using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.FindPtHokenList;

namespace EmrCloudApi.Presenters.Insurance
{
    public class FindPtHokenListPresenter
    {
        public Response<FindPtHokenListResponse> Result { get; private set; } = default!;
        public void Complete(FindPtHokenListOutputData output)
        {
            Result = new Response<FindPtHokenListResponse>()
            {
                Data = new FindPtHokenListResponse(output.Data),
                Status = (int)output.Status
            };

            switch (output.Status)
            {
                case FindPtHokenListStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case FindPtHokenListStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case FindPtHokenListStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case FindPtHokenListStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
            }
        }
    }
}
