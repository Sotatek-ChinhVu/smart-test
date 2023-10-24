using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveCompareTenMst
{
    public class SaveCompareTenMstOutputData: IOutputData
    {
        public SaveCompareTenMstOutputData(bool result, SaveCompareTenMstStatus status)
        {
            Result = result;
            Status = status;
        }

        public bool Result { get; private set; }
        public SaveCompareTenMstStatus Status { get; private set; }
    }
}
