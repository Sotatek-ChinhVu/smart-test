using UseCase.ChartApproval.CheckSaveLogOut;

namespace EmrCloudApi.Responses.ChartApproval
{
    public class CheckSaveLogOutResponse
    {
        public CheckSaveLogOutResponse(CheckSaveLogOutStatus status)
        {
            Status = status;
        }

        public CheckSaveLogOutStatus Status { get; private set; }
    }
}
