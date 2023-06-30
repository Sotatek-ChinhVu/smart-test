using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Remove
{
    public class RemoveLockInputData : IInputData<RemoveLockOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public string FunctionCode { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int UserId { get; private set; }

        public bool IsRemoveAllLock { get; private set; }

        public bool IsRemoveAllLockPtId { get; private set; }

        public RemoveLockInputData(int hpId, long ptId, string functionCode, int sinDate, long raiinNo, int userId, bool isRemoveAllLock, bool isRemoveAllLockPtId)
        {
            HpId = hpId;
            PtId = ptId;
            FunctionCode = functionCode;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            IsRemoveAllLock = isRemoveAllLock;
            IsRemoveAllLockPtId = isRemoveAllLockPtId;
        }
    }
}
