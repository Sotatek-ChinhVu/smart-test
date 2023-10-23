namespace Reporting.KensaHistory.Models
{
    public class KensaResultMultiItem
    {
        public KensaResultMultiItem(string resultValue, string abnormalKbn)
        {
            ResultValue = resultValue;
            AbnormalKbn = abnormalKbn;
        }

        public string ResultValue { get; private set; }

        public string AbnormalKbn { get; private set; }
    }
}
