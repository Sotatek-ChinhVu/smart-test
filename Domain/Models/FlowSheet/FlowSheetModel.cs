﻿namespace Domain.Models.FlowSheet
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

        public FlowSheetModel(int sinDate, long ptId, long raiinNo, int syosaisinKbn, int status)
        {
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtId = ptId;
            SyosaisinKbn = syosaisinKbn;
            Status = status;
            FullLineOfKarte = string.Empty;
            Comment = string.Empty;
            RaiinListInfs = new List<RaiinListInfModel>();
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

    }
}
