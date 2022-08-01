using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinListTagModel
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public bool IsAddNew { get; set; }
        public RaiinListTagModel(int hpId, long ptId, long raiinNo, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }
        public RaiinListTagModel(RaiinListTag model)
        {
            HpId = model.HpId;
            PtId = model.PtId;
            RaiinNo = model.RaiinNo;
            SinDate = model.SinDate;
            IsAddNew = false;
        }

        public RaiinListTagModel(int hpId, long ptId, long raiinNo, int sinDate, bool isAddNew)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            IsAddNew = isAddNew;
        }
    }
}
