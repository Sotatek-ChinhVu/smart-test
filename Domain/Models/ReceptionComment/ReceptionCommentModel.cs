using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionComment
{
    public class ReceptionCommentModel
    {
        public ReceptionCommentModel(int hpId, long ptId, long raiinNo, string text)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            Text = text;
        }
        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}

