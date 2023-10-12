using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaInfDetail
{
    public class UpdateKensaInfDetailOutputData : IOutputData
    {
        public UpdateKensaInfDetailStatus Status { get; private set; }
        public bool CheckResult { get; private set; }

        public UpdateKensaInfDetailOutputData(bool checkResult, UpdateKensaInfDetailStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }
    }

    public enum UpdateKensaInfDetailStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidDataUpdate = 4,

    }
}
