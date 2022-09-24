namespace Domain.Models.FlowSheet
{
    public class FlowSheetModel
    {
        public FlowSheetModel(int sinDate, int tagNo, string fullLineOfKarte, long raiinNo, int syosaisinKbn, string comment, int status, bool isNextOrder, bool isToDayOdr, List<RaiinListInfModel> raiinListInfs, long ptId, int cmtKbn, long rainListCmtSeqNo, int rainListTagSeqNo)
        {
            SinDate = sinDate;
            TagNo = tagNo;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            Comment = comment;
            Status = status;
            IsContainsFile = raiinListInfs.Any(r => r.RaiinListKbn == 4); ;
            IsNextOrder = isNextOrder;
            IsToDayOdr = isToDayOdr;
            RaiinListInfs = raiinListInfs;
            PtId = ptId;
            CmtKbn = cmtKbn;
            RainListCmtSeqNo = rainListCmtSeqNo;
            RainListTagSeqNo = rainListTagSeqNo;
        }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public string FullLineOfKarte { get; private set; }

        public long RaiinNo { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public string Comment { get; private set; }

        public int Status { get; private set; }

        public bool IsContainsFile { get; private set; }

        public bool IsNextOrder { get; private set; }

        public bool IsToDayOdr { get; private set; }

        public List<RaiinListInfModel> RaiinListInfs { get; private set; }

        public long PtId { get; private set; }

        public int CmtKbn { get; private set; }

        public long RainListCmtSeqNo { get; private set; }

        public int RainListTagSeqNo { get; private set; }

    }
}
