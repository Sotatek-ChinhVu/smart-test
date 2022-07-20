using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientCommentInfo
{
    public class PatientCmtInfModel
    {
        public PatientCmtInfModel(int hpId, long ptId, int seqNo, string text, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            Text = text;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SeqNo { get; private set; }
        public string Text { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateMachine { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string UpdateMachine { get; private set; }
    }
}
