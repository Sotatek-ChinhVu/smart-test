using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Check
{
    public class CheckLockInputData : IInputData<CheckLockOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public string FunctionCode { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int UserId { get; private set; }

        public string TabKey { get; private set; }

        public CheckLockInputData(int hpId, long ptId, string functionCode, int sinDate, long raiinNo, int userId, string tabKey)
        {
            HpId = hpId;
            PtId = ptId;
            FunctionCode = functionCode;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            TabKey = tabKey;
        }
    }
}
