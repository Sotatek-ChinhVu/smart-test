using Domain.Models.LockInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.ReceptionLock
{
    public class GetReceptionLockOutputData:IOutputData
    {
        public List<ReceptionLockModel> ListLockModels { get; private set; }

        public GetReceptionLockStatus Status { get; private set; }

        public GetReceptionLockOutputData(List<ReceptionLockModel> lockModels, GetReceptionLockStatus status)
        {
            ListLockModels = lockModels;
            Status = status;
        }
    }
}
