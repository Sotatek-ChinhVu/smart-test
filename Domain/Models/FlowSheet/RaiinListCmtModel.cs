using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinListCmtModel
    {
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int CmtKbn { get; private set; }
        public string Text { get; private set; }
        public bool IsAddNew { get; private set; }
        public RaiinListCmtModel(int hpId, long ptId, long raiinNo, int sinDate, int cmtKbn, string text)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            CmtKbn = cmtKbn;
            Text = text ?? string.Empty;
        }
        public RaiinListCmtModel(RaiinListCmt model)
        {
            HpId = model.HpId;
            PtId = model.PtId;
            RaiinNo = model.RaiinNo;
            SinDate = model.SinDate;
            CmtKbn = model.CmtKbn;
            Text = model.Text;
            IsAddNew = false;
        }
        public RaiinListCmtModel(int hpId, long ptId, long raiinNo, int sinDate, int cmtKbn, string text, bool isAddNew)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            CmtKbn = cmtKbn;
            Text = text ?? string.Empty;
            IsAddNew = isAddNew;
        }

        public RaiinListCmtModel()
        {
        }
    }
}
