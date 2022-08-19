using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.UpdateAdopted
{
    public class UpdateAdoptedInputItemOutputData: IOutputData
    {
        public UpdateAdoptedInputItemOutputData(bool statusUpdate, UpdateAdoptedInputItemStatus status)
        {
            StatusUpdate = statusUpdate;
            Status = status;
        }

        public bool StatusUpdate { get; private set; }

        public UpdateAdoptedInputItemStatus Status { get; private set; }
    }
}
