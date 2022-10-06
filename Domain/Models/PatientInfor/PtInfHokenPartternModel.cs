namespace Domain.Models.PatientInfor
{
    public class PtInfHokenPartternModel
    {
        public PtInfHokenPartternModel(long seqNo, int hokenPid, int hokenKbn, int hokenSbtCd, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int startDate, int endDate, string hokenMemo, PtInfPtHokenModel hokenInf, PtInfPtKohiModel kohi1, PtInfPtKohiModel kohi2, PtInfPtKohiModel kohi3, PtInfPtKohiModel kohi4)
        {
            SeqNo = seqNo;
            HokenPid = hokenPid;
            HokenKbn = hokenKbn;
            HokenSbtCd = hokenSbtCd;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            StartDate = startDate;
            EndDate = endDate;
            HokenMemo = hokenMemo;
            HokenInf = hokenInf;
            Kohi1 = kohi1;
            Kohi2 = kohi2;
            Kohi3 = kohi3;
            Kohi4 = kohi4;
        }

        public long SeqNo { get; private set; }
        public int HokenPid { get; private set; }
        public int HokenKbn { get; private set; }
        public int HokenSbtCd { get; private set; }
        public int HokenId { get; private set; }
        public int Kohi1Id { get; private set; }
        public int Kohi2Id { get; private set; }
        public int Kohi3Id { get; private set; }
        public int Kohi4Id { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string HokenMemo { get; private set; }
        public PtInfPtHokenModel HokenInf { get; private set; }
        public PtInfPtKohiModel Kohi1 { get; private set; }
        public PtInfPtKohiModel Kohi2 { get; private set; }
        public PtInfPtKohiModel Kohi3 { get; private set; }
        public PtInfPtKohiModel Kohi4 { get; private set; }
    }
}
