using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public class MonshinInforModel
    {
        public MonshinInforModel(int hpId, long ptId, long raiinNo, int sinDate, string text)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            Text = text;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
