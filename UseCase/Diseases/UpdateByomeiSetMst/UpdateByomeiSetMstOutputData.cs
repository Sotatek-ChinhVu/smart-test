using UseCase.Core.Sync.Core;

namespace UseCase.ByomeiSetMst.UpdateByomeiSetMst
{
    public class UpdateByomeiSetMstOutputData : IOutputData
    {
        public UpdateByomeiSetMstStatus Status { get; private set; }
        public bool CheckResult { get; set; }

        public UpdateByomeiSetMstOutputData(bool checkResult, UpdateByomeiSetMstStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }
    }

    public enum UpdateByomeiSetMstStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidDataUpdate = 4,
        Failed = 5
    }
}

