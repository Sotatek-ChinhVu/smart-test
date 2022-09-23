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

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; private set; }
        public string Rtext { get; private set; } = string.Empty;
        public int GetKbn { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string? CreateMachine { get; private set; } = string.Empty;
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string? UpdateMachine { get; private set; } = string.Empty;
    }
}
