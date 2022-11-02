using System.Xml.Linq;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenInputData : IInputData<SaveSwapHokenOutputData>
    {
        public SaveSwapHokenInputData(int hpId, long ptId, int hokenIdBefore, int hokenIdAfter, int hokenPidBefore, int hokenPidAfter, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            HokenIdBefore = hokenIdBefore;
            HokenIdAfter = hokenIdAfter;
            HokenPidBefore = hokenPidBefore;
            HokenPidAfter = hokenPidAfter;
            StartDate = startDate;
            EndDate = endDate == 0 ? 99999999 : endDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int HokenIdBefore { get; private set; }
        public int HokenIdAfter { get; private set; }
        public int HokenPidBefore { get; private set; }
        public int HokenPidAfter { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
    }
}
