using Domain.Models.RaiinListMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class FlowSheetModel
    {
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

        public FlowSheetModel(int sinDate, string fullLineOfKarte, long raiinNo, int syosaisinKbn, int status, bool _isContainsFile, int tagNo, string cmt, List<RaiinListInfModel> infs, bool isNextOrder, bool isToDayOdr)
        {
            SinDate = sinDate;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            Status = status;
            TagNo = tagNo;
            Comment = cmt;
            RaiinListInfs = infs;
            IsContainsFile = _isContainsFile;
            IsNextOrder = isNextOrder;
            IsToDayOdr = isToDayOdr;
        }
    }
}
