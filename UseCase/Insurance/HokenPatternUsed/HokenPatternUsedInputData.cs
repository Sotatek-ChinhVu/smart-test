using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.HokenPatternUsed
{
    public class HokenPatternUsedInputData : IInputData<HokenPatternUsedOutputData>
    {
        public HokenPatternUsedInputData(int hpId, long ptId, int hokenPid)
        {
            HpId = hpId;
            PtId = ptId;
            HokenPid = hokenPid;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenPid { get; private set; }
    }
}
