using UseCase.Core.Sync.Core;

namespace UseCase.ListSetMst.UpdateListSetMst
{
    public class UpdateListSetMstOutputData : IOutputData
    {
        public UpdateListSetMstStatus Status { get; private set; }
        public bool CheckResult { get; set; }

        public UpdateListSetMstOutputData(bool checkResult, UpdateListSetMstStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }
    }

    public enum UpdateListSetMstStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidDataUpdate = 4,
        Failed = 5
    }
}
