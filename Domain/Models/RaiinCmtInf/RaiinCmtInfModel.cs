using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RaiinCmtInf
{
    public class RaiinCmtInfModel
    {
        public RaiinCmtInfModel(int hpId, long ptId, int sinDate, long raiinNo, int cmtKbn, long seqNo, string text, int isDelete, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            CmtKbn = cmtKbn;
            SeqNo = seqNo;
            Text = text;
            IsDelete = isDelete;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public int CmtKbn { get; set; }
        public long SeqNo { get; set; }
        public string Text { get; set; }
        public int IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string CreateMachine { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
        public string UpdateMachine { get; set; }
    }
}
