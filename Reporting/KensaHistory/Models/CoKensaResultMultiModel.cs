namespace Reporting.KensaHistory.Models
{
    public class CoKensaResultMultiModel
    {
        public CoKensaResultMultiModel(long iraiDate, string itemName, string unit, string standardValue, List<KensaResultMultiItem> kensaResultMultiItems, List<long> date, long seqParentNo, string rowSeqId)
        {
            IraiDate = iraiDate;
            ItemName = itemName;
            Unit = unit;
            StandardValue = standardValue;
            KensaResultMultiItems = kensaResultMultiItems;
            Date = date;
            SeqParentNo = seqParentNo;
            RowSeqId = rowSeqId;
        }

        public long IraiDate { get; private set; }

        public List<KensaResultMultiItem> KensaResultMultiItems { get; private set; }

        public string ItemName { get; private set; }

        public string Unit { get; private set; }

        public string StandardValue { get; private set; }

        public List<long> Date {  get; private set; }

        public long SeqParentNo {  get; private set; }

        public string RowSeqId {  get; private set; }
    }
}
