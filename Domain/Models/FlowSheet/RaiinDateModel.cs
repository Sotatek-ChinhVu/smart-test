using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinDateModel
    {
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public RaiinDateModel(long raiinNo, int sinDate)
        {
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }
    }
}
