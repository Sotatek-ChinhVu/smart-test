namespace EmrCloudApi.Requests.SwapHoken
{
    public class SaveSwapHokenRequest
    {
        public SaveSwapHokenRequest(long ptId, int hokenIdBefore, string hokenNameBefore, int hokenIdAfter, string hokenNameAfter, int hokenPidBefore, int hokenPidAfter, int startDate, int endDate, bool isHokenPatternUsed, bool confirmInvalidIsShowConversionCondition, bool confirmSwapHoken, bool isReCalculation, bool isReceCalculation, bool isReceCheckError)
        {
            PtId = ptId;
            HokenIdBefore = hokenIdBefore;
            HokenNameBefore = hokenNameBefore;
            HokenIdAfter = hokenIdAfter;
            HokenNameAfter = hokenNameAfter;
            HokenPidBefore = hokenPidBefore;
            HokenPidAfter = hokenPidAfter;
            StartDate = startDate;
            EndDate = endDate;
            IsHokenPatternUsed = isHokenPatternUsed;
            ConfirmInvalidIsShowConversionCondition = confirmInvalidIsShowConversionCondition;
            ConfirmSwapHoken = confirmSwapHoken;
            IsReCalculation = isReCalculation;
            IsReceCalculation = isReceCalculation;
            IsReceCheckError = isReceCheckError;
        }

        public long PtId { get; private set; }

        public int HokenIdBefore { get; private set; }

        public string HokenNameBefore { get; private set; }

        public int HokenIdAfter { get; private set; }

        public string HokenNameAfter { get; private set; }

        public int HokenPidBefore { get; private set; }

        public int HokenPidAfter { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public bool IsHokenPatternUsed { get; private set; }

        public bool ConfirmInvalidIsShowConversionCondition { get; private set; }

        public bool ConfirmSwapHoken { get; private set; }

        #region caculate module client
        public bool IsReCalculation { get; private set; }

        public bool IsReceCalculation { get; private set; }

        public bool IsReceCheckError { get; private set; }
        #endregion

    }
}