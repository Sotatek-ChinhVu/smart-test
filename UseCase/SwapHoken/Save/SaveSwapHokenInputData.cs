using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenInputData : IInputData<SaveSwapHokenOutputData>
    {
        public SaveSwapHokenInputData(int hpId, long ptId, int hokenIdBefore, int hokenIdAfter, int hokenPidBefore, int hokenPidAfter, int startDate, int endDate, bool isReCalculation, bool isReceCalculation, bool isReceCheckError)
        {
            HpId = hpId;
            PtId = ptId;
            HokenIdBefore = hokenIdBefore;
            HokenIdAfter = hokenIdAfter;
            HokenPidBefore = hokenPidBefore;
            HokenPidAfter = hokenPidAfter;
            StartDate = startDate;
            EndDate = endDate;
            IsReCalculation = isReCalculation;
            IsReceCalculation = isReceCalculation;
            IsReceCheckError = isReceCheckError;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int HokenIdBefore { get; private set; }
        public int HokenIdAfter { get; private set; }
        public int HokenPidBefore { get; private set; }
        public int HokenPidAfter { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; set; }
        public bool IsReCalculation { get; private set; }
        public bool IsReceCalculation { get; private set; }
        public bool IsReceCheckError { get; private set; }
    }
}
