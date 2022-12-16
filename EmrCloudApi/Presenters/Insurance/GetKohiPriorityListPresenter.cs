using EmrCloudApi.Responses.Insurance;
using EmrCloudApi.Responses;
using UseCase.Insurance.GetKohiPriorityList;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.Insurance
{
    public class GetKohiPriorityListPresenter : IGetKohiPriorityListOutputPort
    {
        public Response<GetKohiPriorityListResponse> Result { get; private set; } = default!;
        public void Complete(GetKohiPriorityListOutputData output)
        {
            Result = new Response<GetKohiPriorityListResponse>()
            {
                Data = new GetKohiPriorityListResponse(output.Data),
                Status = (byte)output.Status
            };

            switch (output.Status)
            {
                case GetKohiPriorityListStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetKohiPriorityListStatus.Exception:
                    Result.Message = ResponseMessage.ExceptionError;
                    break;
                case GetKohiPriorityListStatus.DataNotFound:
                    Result.Message = ResponseMessage.NotFound;
                    break;
            }
        }
    }
}
