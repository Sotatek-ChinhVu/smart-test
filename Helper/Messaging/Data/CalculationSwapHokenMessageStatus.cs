namespace Helper.Messaging.Data
{
    public class CalculationSwapHokenMessageStatus
    {
        public CalculationSwapHokenMessageStatus(string displayText, int percent, bool complete, bool completeSuccess)
        {
            DisplayText = displayText;
            Percent = percent;
            Complete = complete;
            CompleteSuccess = completeSuccess;
        }

        public string DisplayText { get; private set; }

        public int Percent { get; private set; }

        public bool Complete { get; private set; }

        /// <summary>
        /// true when done last flusAsync
        /// </summary>
        public bool CompleteSuccess { get; private set; }
    }
}
