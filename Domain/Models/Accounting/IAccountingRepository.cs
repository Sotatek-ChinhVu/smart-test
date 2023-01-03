using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Accounting
{
    public interface IAccountingRepository
    {
        AccountingModel GetAccountingInfo(int hpId, long ptId, long oyaRaiinNo);
    }
}
