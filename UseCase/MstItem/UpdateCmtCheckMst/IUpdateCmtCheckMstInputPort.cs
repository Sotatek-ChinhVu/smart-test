using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.GetAllCmtCheckMst;

namespace UseCase.MstItem.UpdateCmtCheckMst
{
    public interface IUpdateCmtCheckMstInputPort : IInputPort<UpdateCmtCheckMstInputData, UpdateCmtCheckMstOutputData>
    {
    }
}
