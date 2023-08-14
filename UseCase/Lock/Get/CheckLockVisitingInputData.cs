using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class CheckLockVisitingInputData : IInputData<CheckLockVisitingOutputData>
    {
        public CheckLockVisitingInputData(int hpId, int userId, long ptId, int sinDate, string functionCode, string tabKey)
        {
            HpId = hpId;
            PtId = ptId;
            UserId = userId;
            SinDate = sinDate;
            FunctionCode = functionCode;
            TabKey = tabKey;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string FunctionCode { get; private set; }
        public string TabKey { get; private set; }
    }
}
