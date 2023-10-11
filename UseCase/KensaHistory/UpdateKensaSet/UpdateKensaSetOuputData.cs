using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaSet
{
    public class UpdateKensaSetOuputData : IOutputData
    {
        public UpdateKensaSetStatus Status { get; private set; }
        public bool CheckResult { get; set; }

        public UpdateKensaSetOuputData(bool checkResult, UpdateKensaSetStatus status)
        {
            Status = status;
            CheckResult = checkResult;
        }
    }

    public enum UpdateKensaSetStatus : byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidUserId = 3,
        InvalidDataUpdate = 4,
        Failed = 5
    }
}
