using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class ConfirmDateModel
    {
        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckMachine { get; private set; }

        public string CheckComment { get; private set; }

        public DateTime ConfirmDate { get; private set; }

        public ConfirmDateModel(int hokenGrp, int hokenId, long seqNo, int checkId, string checkMachine, string checkComment, DateTime confirmDate)
        {
            HokenGrp = hokenGrp;
            HokenId = hokenId;
            SeqNo = seqNo;
            CheckId = checkId;
            CheckMachine = checkMachine;
            CheckComment = checkComment;
            ConfirmDate = confirmDate;
        }
    }
}
