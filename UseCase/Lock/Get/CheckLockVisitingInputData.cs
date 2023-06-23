using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class CheckLockVisitingInputData : IInputData<CheckLockVisitingOutputData>
    {
        public CheckLockVisitingInputData(int hpId, int userId, long ptId, int sinDate, string functionCode, string token)
        {
            HpId = hpId;
            PtId = ptId;
            UserId = userId;
            SinDate = sinDate;
            FunctionCode = functionCode;
            Token = token;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public string FunctionCode { get; private set; }
        public string Token { get; private set; }
    }
}
