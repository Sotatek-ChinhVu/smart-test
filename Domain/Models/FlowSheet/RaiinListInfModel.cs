using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinListInfModel
    {
        private RaiinListInfModel item;

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int GrpId { get; private set; }
        public int KbnCd { get; private set; }
        public int RaiinListKbn { get; private set; }
        public int SortNo { get; private set; }
        public bool IsContainsFile { get; set; } = false;
        public RaiinListInfModel(int hpId, long ptId, long raiinNo, int sinDate, int grpId, int kbnCd, int raiinListKbn, int sortNo)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            GrpId = grpId;
            KbnCd = kbnCd;
            RaiinListKbn = raiinListKbn;
            SortNo = sortNo;
        }
        public RaiinListInfModel(RaiinListInfModel item)
        {
            this.item = item;
        }
    }
}
