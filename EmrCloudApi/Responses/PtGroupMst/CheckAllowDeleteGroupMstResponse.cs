using UseCase.PtGroupMst.CheckAllowDelete;

namespace EmrCloudApi.Responses.PtGroupMst
{
    public class CheckAllowDeleteGroupMstResponse
    {
        public CheckAllowDeleteGroupMstResponse(CheckAllowDeleteGroupMstStatus status)
        {
            Status = status;
        }

        public CheckAllowDeleteGroupMstStatus Status { get; private set; }
    }
}
