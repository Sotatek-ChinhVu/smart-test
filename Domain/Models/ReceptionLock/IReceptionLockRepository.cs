using Domain.Common;
using Domain.Models.LockInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionLock
{
    public interface IReceptionLockRepository : IRepositoryBase
    {
        public List<ReceptionLockModel> ReceptionLockModel(long sinDate, long ptId, long raiinNo, string functionCd);
    }
}
