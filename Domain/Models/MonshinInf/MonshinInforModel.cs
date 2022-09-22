using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; set; } = string.Empty;
    }
}
