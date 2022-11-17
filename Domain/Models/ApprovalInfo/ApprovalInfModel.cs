using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Models.ApprovalInfo
{
    public class ApprovalInfModel
    {
        public ApprovalInfModel(int hpId, long id, long raiinNo, int seqNo, long ptId, int sinDate, int isDeleted, long ptNum, string kanaName, string name, int kaId, int uketukeNo)
        {
            HpId = hpId;
            Id = id;
            RaiinNo = raiinNo;
            SeqNo = seqNo;
            PtId = ptId;
            SinDate = sinDate;
            IsDeleted = isDeleted; 
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            UketokeNo = uketukeNo;
        }
        public int HpId { get; private set; }
        public long Id { get; private set; }
        public long RaiinNo { get; private set; }
        public int SeqNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int IsDeleted { get; private set; }
        public long PtNum { get; private set; }
        public string KanaName { get; private set; }
        public string Name {get; private set;}
        public int UketokeNo { get; private set; }
    }
}
