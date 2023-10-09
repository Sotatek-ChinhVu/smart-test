namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailModel
    {
        public ListKensaInfDetailModel(List<KensaInfDetailColModel> kensaInfDetailCol, List<Dictionary<long, List<ListKensaInfDetailItem>>> kensaInfDetailData)
        {
            KensaInfDetailCol = kensaInfDetailCol;
            KensaInfDetailData = kensaInfDetailData;
        }

        public ListKensaInfDetailModel()
        {
        }

        public List<KensaInfDetailColModel> KensaInfDetailCol { get; private set; }

        public List<Dictionary<long, List<ListKensaInfDetailItem>>> KensaInfDetailData { get; private set; }

        public class KensaInfDetailColModel
        {
            public KensaInfDetailColModel(long iraiCd, int iraiDate)
            {
                IraiCd = iraiCd;
                IraiDate = iraiDate;
            }
            public long IraiCd { get; private set; }
            public int IraiDate { get; private set; }
        }
    }
}
