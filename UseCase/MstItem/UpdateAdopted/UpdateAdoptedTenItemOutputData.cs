using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdopted
{
    public class UpdateAdoptedTenItemOutputData: IOutputData
    {
        public UpdateAdoptedTenItemOutputData(bool statusUpdate, UpdateAdoptedTenItemStatus status)
        {
            StatusUpdate = statusUpdate;
            Status = status;
        }

        public bool StatusUpdate { get; private set; }

        public UpdateAdoptedTenItemStatus Status { get; private set; }
    }
}
