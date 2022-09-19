using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public class MonshinDto
    {
        public MonshinDto(int hpId, long ptId, long raiinNo, int sinDate, string text, long seqNo,
            string rtext, int getKbn, int createId, string? createMachine)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            Text = text;
            SeqNo = seqNo;
            Rtext = rtext;
            GetKbn = getKbn;
            CreateId = createId;
            CreateMachine = createMachine;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public string Text { get; set; } = string.Empty;
        public long SeqNo { get; set; }
        public string Rtext { get; set; } = string.Empty;
        public int GetKbn { get; set; }
        public int IsDeleted { get; set; }
        public int CreateId { get; set; }
        public string? CreateMachine { get; set; } = string.Empty;
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
