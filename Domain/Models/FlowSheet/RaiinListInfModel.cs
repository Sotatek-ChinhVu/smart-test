namespace Domain.Models.FlowSheet
{
    public class RaiinListInfModel
    {
        public long RaiinNo { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public string KbnName { get; private set; }

        public string KbnColorCode { get; private set; }

        public int RaiinListKbn { get; private set; }

        public RaiinListInfModel(long raiinNo, int grpId, int kbnCd, int raiinListKbn, string kbnName, string kbnColorCode)
        {
            RaiinNo = raiinNo;
            GrpId = grpId;
            KbnCd = kbnCd;
            RaiinListKbn = raiinListKbn;
            KbnName = kbnName;
            KbnColorCode = kbnColorCode;
        }
    }
}
