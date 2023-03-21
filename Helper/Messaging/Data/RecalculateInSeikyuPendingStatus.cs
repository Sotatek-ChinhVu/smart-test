namespace Helper.Messaging.Data
{
    public class RecalculateInSeikyuPendingStatus
    {
        public RecalculateInSeikyuPendingStatus(string displayText, int percent, bool complete, bool completeSuccess)
        {
            DisplayText = displayText;
            Percent = percent;
            Complete = complete;
            CompleteSuccess = completeSuccess;
        }

        public string DisplayText { get; private set; }

        /// <summary>
        /// Percent Text
        /// </summary>
        public int Percent { get; private set; }

        /// <summary>
        /// Complete API
        /// </summary>
        public bool Complete { get; private set; }

        /// <summary>
        /// Complete API success
        /// </summary>
        public bool CompleteSuccess { get; private set; }
    }
}
