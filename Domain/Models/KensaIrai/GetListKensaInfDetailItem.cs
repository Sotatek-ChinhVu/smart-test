namespace Domain.Models.KensaIrai
{
    public class ListKensaInfDetailItem
    {
        public ListKensaInfDetailItem(
        long seqNo,
        string kensaName,
        string kensaItemCd,
        string resultVal,
        string resultType,
        string abnormalKbn,
        string cmtCd1,
        string cmtCd2,
        string cmt1,
        string cmt2,
        string std,
        string stdLow,
        string stdHig,
        string unit,
        string nyubi,
        string yoketu,
        string bilirubin,
        int sikyuKbn,
        int tosekiKbn
        )
        {
            SeqNo = seqNo;
            KensaName = kensaName;
            KensaItemCd = kensaItemCd;
            ResultVal = resultVal;
            ResultType = resultType;
            AbnormalKbn = abnormalKbn;
            CmtCd1 = cmtCd1;
            CmtCd2 = cmtCd2;
            Cmt1 = cmt1;
            Cmt2 = cmt2;
            Std = std;
            StdLow = stdLow;
            StdHig = stdHig;
            Unit = unit;
            Nyubi = nyubi;
            Yoketu = yoketu;
            Bilirubin = bilirubin;
            SikyuKbn = sikyuKbn;
            TosekiKbn = tosekiKbn;
        }

        public long SeqNo { get; private set; }

        public string KensaName { get; private set; }

        public string KensaItemCd { get; private set; }

        public string ResultVal { get; private set; }

        public string ResultType { get; private set; }

        public string AbnormalKbn { get; private set; }

        public string CmtCd1 { get; private set; }

        public string CmtCd2 { get; private set; }

        public string Cmt1 { get; private set; }

        public string Cmt2 { get; private set; }

        public string Std { get; private set; }

        public string StdLow { get; private set; }

        public string StdHig { get; private set; }

        public string Unit { get; private set; }

        public string Nyubi { get; private set; }

        public string Yoketu { get; private set; }

        public string Bilirubin { get; private set; }

        public int SikyuKbn { get; private set; }

        public int TosekiKbn { get; private set; }
    }
}