using UseCase.MstItem.CheckIsTenMstUsed;

namespace EmrCloudApi.Responses.MstItem
{
    public class CheckIsTenMstUsedResponse
    {
        public CheckIsTenMstUsedResponse(CheckIsTenMstUsedStatus status)
        {
            Status = status;
        }

        public CheckIsTenMstUsedStatus Status { get; private set; }
    }
}
