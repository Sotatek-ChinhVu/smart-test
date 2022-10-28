namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class HokenPartternDto
    {
        public HokenPartternDto(int hpId, long ptId, int hokenPid, long seqNo, int hokenKbn, int hokenSbtCd, int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string hokenMemo, int startDate, int endDate)
        {
            HpId = hpId;
            PtId = ptId;
            HokenPid = hokenPid;
            SeqNo = seqNo;
            HokenKbn = hokenKbn;
            HokenSbtCd = hokenSbtCd;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            HokenMemo = hokenMemo;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenPid { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenKbn { get; private set; }

        public int HokenSbtCd { get; private set; }

        public int HokenId { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }

        public string HokenMemo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}