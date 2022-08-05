﻿using Domain.Models.RaiinListMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class FlowSheetModel
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public RaiinListTagModel RaiinListTag { get; private set; }
        public string FullLineOfKarte { get; private set; }
        public long RaiinNo { get; private set; }
        public int SyosaisinKbn { get; private set; }
        public RaiinListCmtModel RaiinListCmt { get; private set; }
        public bool IsNextOrder { get; private set; }
        public bool IsTodayOrder { get; private set; }
        public int Status { get; private set; }
        // Raiin List Detail && RaiinListInf (for dynamic column)
        public List<RaiinListInfModel> RaiinListInfs { get; private set; }
        public FlowSheetModel(int hpId, long ptId, int sinDate, string fullLineOfKarte, long raiinNo, int syosaisinKbn, bool isNextOrder, bool isTodayOrder, int status, RaiinListTagModel tag, RaiinListCmtModel cmt, List<RaiinListInfModel> infs)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            FullLineOfKarte = fullLineOfKarte ?? string.Empty;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            Status = status;
            IsNextOrder = isNextOrder;
            IsTodayOrder = isTodayOrder;
            RaiinListTag = tag;
            RaiinListCmt = cmt;
            RaiinListInfs = infs;
        }
        public FlowSheetModel(FlowSheetModel model, RaiinListTagModel tag, RaiinListCmtModel cmt, List<RaiinListInfModel> inf)
        {
            HpId = model.HpId;
            PtId = model.PtId;
            SinDate = model.SinDate;
            FullLineOfKarte = model.FullLineOfKarte;
            RaiinNo = model.RaiinNo;
            SyosaisinKbn= model.SyosaisinKbn;
            Status = model.Status;
            IsNextOrder = model.IsNextOrder;
            IsTodayOrder = model.IsTodayOrder;
            RaiinListTag = tag;
            RaiinListCmt = cmt;
            RaiinListInfs = inf;
        }

        public FlowSheetModel(int hpId, long ptId, int sinDate, string fullLineOfKarte, long raiinNo, int syosaisinKbn, bool isNextOrder, bool isTodayOrder, int status)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            FullLineOfKarte = fullLineOfKarte;
            RaiinNo = raiinNo;
            SyosaisinKbn = syosaisinKbn;
            IsNextOrder = isNextOrder;
            IsTodayOrder = isTodayOrder;
            Status = status;
            RaiinListTag = new();
            RaiinListCmt = new();
            RaiinListInfs = new();
        }
    }
}
