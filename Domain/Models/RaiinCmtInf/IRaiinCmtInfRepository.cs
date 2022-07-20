using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RaiinCmtInf
{
    public interface IRaiinCmtInfRepository
    {
        RaiinCmtInfModel GetRaiinCmtInf(long ptId, long raiinNo, long sinDate);
    }
}
