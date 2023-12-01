namespace Reporting.KensaHistory.Models
{
    public class KensaResultMultiItem
    {
        public KensaResultMultiItem(long iraiDate, string resultValue, string abnormalKbn, string resultType)
        {
            IraiDate = iraiDate;
            ResultValue = resultValue;
            AbnormalKbn = abnormalKbn;
            ResultType = resultType;
        }

        public KensaResultMultiItem ChangeResultVal(string resultValue)
        {
            ResultValue = resultValue;
            return this;
        }

        public long IraiDate { get; private set; }

        public string ResultValue { get; private set; }

        public string AbnormalKbn { get; private set; }

        public string ResultType { get; private set; }
    }
}
