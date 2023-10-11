namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailModel
    {
        public ListKensaInfDetailModel(List<KensaInfDetailColModel> kensaInfDetailCol, List<object> kensaInfDetailData)
        {
            KensaInfDetailCol = kensaInfDetailCol;
            KensaInfDetailData = kensaInfDetailData;
        }

        public ListKensaInfDetailModel()
        {
        }

        public List<KensaInfDetailColModel> KensaInfDetailCol { get; private set; }

        public List<object> KensaInfDetailData { get; private set; }

        public class KensaInfDetailColModel
        {
            public KensaInfDetailColModel(long iraiCd, int iraiDate, string nyubi, string yoketu, int bilirubin, int sikyuKbn, int tosekiKbn)
            {
                IraiCd = iraiCd;
                IraiDate = iraiDate;
                Nyubi = nyubi;
                Yoketu = yoketu;
                Bilirubin = bilirubin;
                SikyuKbn = sikyuKbn;
                TosekiKbn = tosekiKbn;
            }

            public long IraiCd { get; private set; }

            public int IraiDate { get; private set; }

            public string Nyubi { get; private set; }

            public string Yoketu { get; private set; }

            public int Bilirubin { get; private set; }

            public int SikyuKbn { get; private set; }

            public int TosekiKbn { get; private set; }
        }
    }
}
