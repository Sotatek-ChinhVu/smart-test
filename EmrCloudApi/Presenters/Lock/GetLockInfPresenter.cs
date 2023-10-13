using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Lock;
using UseCase.Lock.GetLockInf;

namespace EmrCloudApi.Presenters.Lock
{
    public class GetLockInfPresenter : IGetLockInfOutputPort
    {
        public Response<GetLockInfResponse> Result { get; private set; } = default!;

        public void Complete(GetLockInfOutputData outputData)
        {
            Result = new Response<GetLockInfResponse>()
            {
                Data = new GetLockInfResponse(outputData.LockInfs, outputData.UserLocks),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetLockInfStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetLockInfStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
