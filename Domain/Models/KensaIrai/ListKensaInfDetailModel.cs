namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailModel
    {
        public ListKensaInfDetailModel(long ptId, long iraiCd, long raiinNo, int iraiDate, List<ListKensaInfDetailItem> kensaInfDetailItem)
        {
            PtId = ptId;
            IraiCd = iraiCd;
            RaiinNo = raiinNo;
            IraiDate = iraiDate;
            KensaInfDetailItem = kensaInfDetailItem;
        }

        public long PtId { get; private set; }

        public long IraiCd { get; private set; }

        public long RaiinNo { get; private set; }

        public int IraiDate { get; private set; }

        public List<ListKensaInfDetailItem> KensaInfDetailItem { get; private set; }
    }
}
