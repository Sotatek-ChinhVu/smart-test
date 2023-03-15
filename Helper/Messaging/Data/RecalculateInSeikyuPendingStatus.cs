namespace Helper.Messaging.Data
{
    public class RecalculateInSeikyuPendingStatus
    {
        public RecalculateInSeikyuPendingStatus(string displayText, int percent)
        {
            DisplayText = displayText;
            Percent = percent;
        }

        public string DisplayText { get; private set; }

        /// <summary>
        /// Percent Text
        /// </summary>
        public int Percent { get; private set; }
    }
}
