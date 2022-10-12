using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.LockInf
{
    public class ReceptionLockModel
    {
        public ReceptionLockModel(int hpId, long ptId, string functionCd, long sinDate, long raiinNo, long oyaRaiinNo, string machine, int userId, DateTime lockDate)
        {
            HpId = hpId;
            PtId = ptId;
            FunctionCd = functionCd;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OyaRaiinNo = oyaRaiinNo;
            Machine = machine;
            UserId = userId;
            LockDate = lockDate;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public string FunctionCd { get; set; }
        public long SinDate { get; set; }
        public long RaiinNo { get; set; }
        public long OyaRaiinNo { get; set; }
        public string Machine { get; set; }
        public int UserId { get; set; }
        public DateTime LockDate { get; set; }
    }
}
