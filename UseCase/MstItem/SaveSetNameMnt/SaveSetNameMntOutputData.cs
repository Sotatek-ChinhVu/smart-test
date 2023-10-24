using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveSetNameMnt
{
    public class SaveSetNameMntOutputData: IOutputData
    {
        public SaveSetNameMntOutputData(bool result, SaveSetNameMntStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }
        public SaveSetNameMntStatus Status { get; private set; }
    }
}
