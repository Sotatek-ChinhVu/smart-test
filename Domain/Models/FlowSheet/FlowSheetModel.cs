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

        public RaiinListCmtModel RaiinListCmt { get; private set; }

        public int Status { get; private set; }

        public bool IsContainsFile { get; private set; }

        // Raiin List Detail && RaiinListInf (for dynamic column)
        public List<RaiinListInfModel> RaiinListInfs { get; private set; }

        public FlowSheetModel(int sinDate, string fullLineOfKarte, long raiinNo, int syosaisinKbn, int status, bool _isContainsFile, int tagNo, RaiinListCmtModel cmt, List<RaiinListInfModel> infs)
        {
            SinDate = sinDate;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            Status = status;
            TagNo = tagNo;
            RaiinListCmt = cmt;
            RaiinListInfs = infs;
            IsContainsFile = _isContainsFile;
        }
            IsToDayOdr = isToDayOdr;
        }
    }
}
