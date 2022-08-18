namespace Domain.Models.FlowSheet
{
    public class FlowSheetModel
    {
        public FlowSheetModel(long ptId, int sinDate, int tagNo, string fullLineOfKarte, long raiinNo, int syosaisinKbn, string comment, int status, bool isContainsFile, bool isNextOrder, bool isToDayOdr, int raiinListTagSeqNo, long raiinListCmtSeqNo, int cmtKbn, List<RaiinListInfModel> raiinListInfs)
        {
            PtId = ptId;
            SinDate = sinDate;
            TagNo = tagNo;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            Comment = comment;
            Status = status;
            IsContainsFile = isContainsFile;
            IsNextOrder = isNextOrder;
            IsToDayOdr = isToDayOdr;
            RaiinListTagSeqNo = raiinListTagSeqNo;
            RaiinListCmtSeqNo = raiinListCmtSeqNo;
            CmtKbn = cmtKbn;
            RaiinListInfs = raiinListInfs;
        }

        public long PtId { get; private set; }

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

        public int RaiinListTagSeqNo { get; private set; }

        public long RaiinListCmtSeqNo { get; private set; }

        public int CmtKbn { get; private set; }

        public List<RaiinListInfModel> RaiinListInfs { get; private set; }

    }
}
