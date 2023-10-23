namespace Domain.Models.FlowSheet
{
    public class FlowSheetModel
    {
        public FlowSheetModel(int sinDate, int tagNo, string fullLineOfKarte, long raiinNo, int syosaisinKbn, string comment, int status, bool isNextOrder, bool isToDayOdr, List<RaiinListInfModel> raiinListInfs, long ptId, bool isNotSaved)
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
            IsNotSaved = isNotSaved;
        }

        public FlowSheetModel(int sinDate, long ptId, long raiinNo, string uketukeTime, int syosaisinKbn, int status, bool isNextOrder)
        {
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UketukeTime = uketukeTime;
            PtId = ptId;
            SyosaisinKbn = syosaisinKbn;
            Status = status;
            FullLineOfKarte = string.Empty;
            Comment = string.Empty;
            RaiinListInfs = new List<RaiinListInfModel>();
            IsNextOrder = isNextOrder;
            IsToDayOdr = !isNextOrder;
        }

        public FlowSheetModel(int sinDate, int tagNo, string fullLineOfKarte, long raiinNo, string uketukeTime, int syosaisinKbn, string comment, int status, bool isNextOrder, bool isToDayOdr, List<RaiinListInfModel> raiinListInfs, long ptId, bool isNotSaved)
        {
            SinDate = sinDate;
            TagNo = tagNo;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            UketukeTime = uketukeTime;
            SyosaisinKbn = syosaisinKbn;
            Comment = comment;
            Status = status;
            IsContainsFile = raiinListInfs.Any(r => r.RaiinListKbn == 4); ;
            IsNextOrder = isNextOrder;
            IsToDayOdr = isToDayOdr;
            RaiinListInfs = raiinListInfs;
            PtId = ptId;
            IsNotSaved = isNotSaved;
        }

        public bool IsNext
        {
            get
            {
                return SyosaisinKbn < 0;
            }
        }

        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public string FullLineOfKarte { get; private set; }

        public string UketukeTime { get; private set; }

        public long RaiinNo { get; private set; }

        public int SyosaisinKbn { get; private set; }

        public string Comment { get; private set; }

        public int Status { get; private set; }

        public bool IsContainsFile { get; private set; }

        public bool IsNextOrder { get; private set; }

        public bool IsToDayOdr { get; private set; }

        public List<RaiinListInfModel> RaiinListInfs { get; private set; }

        public long PtId { get; private set; }

        public bool IsNotSaved { get; private set; }

        public FlowSheetModel ChangeFlowSheet(int tagNo, string karteContent, string comment, long ptId)
        {
            TagNo = tagNo;
            FullLineOfKarte = karteContent;
            Comment = comment;
            PtId = ptId;
            IsNotSaved = false;
            RaiinListInfs = new();
            return this;
        }

    }
}
