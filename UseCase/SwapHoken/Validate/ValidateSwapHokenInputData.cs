using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Validate
{
    public class ValidateSwapHokenInputData : IInputData<ValidateSwapHokenOutputData>
    {
        public ValidateSwapHokenInputData(int hpId, long ptId, int hokenPid, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            HokenPid = hokenPid;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenPid { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}
