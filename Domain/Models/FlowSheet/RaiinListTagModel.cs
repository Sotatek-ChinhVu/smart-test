using Domain.Constant;
using Domain.Enum;
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
        public bool IsAddNew { get; private set; }
        public int TagNo { get; private set; }
        public StarSourceType SourceType { get; private set; }
        public string StarResource
        {
            get
            {
                if (TagNo == 0 && SourceType == StarSourceType.FlowSheet)
                {
                    return string.Empty;
                }
                if (FlowSheetConst.StarDictionary.ContainsKey(TagNo))
                {
                    return FlowSheetConst.StarDictionary[TagNo];
                }
                if (SourceType == StarSourceType.FlowSheet)
                {
                    return string.Empty;
                }
                return FlowSheetConst.StarDictionary[0];
            }
        }
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
