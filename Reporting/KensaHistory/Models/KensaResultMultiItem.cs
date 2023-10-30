using Domain.Models.KensaIrai;

namespace Reporting.KensaHistory.Models
{
    public class KensaResultMultiItem
    {
        public KensaResultMultiItem(string resultValue, string abnormalKbn, string resultType)
        {
            ResultValue = resultValue;
            AbnormalKbn = abnormalKbn;
            ResultType = resultType;
        }

        public KensaResultMultiItem ChangeResultVal(string resultValue)
        {
            ResultValue = resultValue;
            return this;
        }

        public string ResultValue { get; private set; }

        public string AbnormalKbn { get; private set; }

        public string ResultType {  get; private set; }
    }
}
