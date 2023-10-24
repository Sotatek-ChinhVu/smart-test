using Newtonsoft.Json;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Add
{
    public class AddLockInputData : IInputData<AddLockOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public string FunctionCode { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int UserId { get; private set; }

        public string TabKey { get; private set; }

        public string LoginKey { get; private set; }

        [JsonConstructor]
        public AddLockInputData(int hpId, long ptId, string functionCode, int sinDate, long raiinNo, int userId, string tabKey, string loginKey)
        {
            HpId = hpId;
            PtId = ptId;
            FunctionCode = functionCode;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            TabKey = tabKey;
            LoginKey = loginKey;
        }

        public AddLockInputData()
        {
            FunctionCode = string.Empty;
            TabKey = string.Empty;
            LoginKey = string.Empty;
        }
    }
}
