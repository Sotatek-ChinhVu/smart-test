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
        public MonshinInforModel(int hpId, long ptId, long raiinNo, int sinDate, string text, int getKbn, int isDeleted, DateTime createDate, DateTime updateDate)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            Text = text;
            GetKbn = getKbn;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            UpdateDate = updateDate;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public string Text { get; set; } = string.Empty;
        public int GetKbn { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; set; }
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string? UpdateMachine { get; private set; } = string.Empty;
    }
}
