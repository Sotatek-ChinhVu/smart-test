using UseCase.Core.Sync.Core;

namespace UseCase.SwapHoken.Save
{
    public class SaveSwapHokenInputData : IInputData<SaveSwapHokenOutputData>
    {
        public SaveSwapHokenInputData(int hpId, long ptId, int hokenIdBefore, string hokenBeforeName, int hokenIdAfter, string hokenAfterName, int hokenPidBefore, int hokenPidAfter, int startDate, int endDate, bool isHokenPatternUsed, bool confirmInvalidIsShowConversionCondition, bool confirmSwapHoken, int userId, bool isReCalculation, bool isReceCalculation, bool isReceCheckError)
        {
            HpId = hpId;
            PtId = ptId;
            HokenIdBefore = hokenIdBefore;
            HokenBeforeName = hokenBeforeName;
            HokenIdAfter = hokenIdAfter;
            HokenAfterName = hokenAfterName;
            HokenPidBefore = hokenPidBefore;
            HokenPidAfter = hokenPidAfter;
            StartDate = startDate;
            EndDate = endDate;
            IsHokenPatternUsed = isHokenPatternUsed;
            ConfirmInvalidIsShowConversionCondition = confirmInvalidIsShowConversionCondition;
            ConfirmSwapHoken = confirmSwapHoken;
            UserId = userId;
            IsReCalculation = isReCalculation;
            IsReceCalculation = isReceCalculation;
            IsReceCheckError = isReceCheckError;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenIdBefore { get; private set; }

        public string HokenBeforeName { get; private set; }

        public int HokenIdAfter { get; private set; }

        public string HokenAfterName { get; private set; }

        public int HokenPidBefore { get; private set; }

        public int HokenPidAfter { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public bool IsHokenPatternUsed { get; private set; }

        public bool ConfirmInvalidIsShowConversionCondition { get; private set; }

        public bool ConfirmSwapHoken { get; private set; }

        public int UserId { get; private set; }

        public void SetEndDate(int value) => this.EndDate = value;

        public bool IsReCalculation { get; private set; }

        public bool IsReceCalculation { get; private set; }

        public bool IsReceCheckError { get; private set; }
    }
}
