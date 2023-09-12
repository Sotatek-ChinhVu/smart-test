using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.GetCmtCheckMstList;

namespace UseCase.MstItem.GetAllCmtCheckMst
{
    public interface IGetAllCmtCheckMstInputPort : IInputPort<GetAllCmtCheckMstInputData, GetAllCmtCheckMstOutputData>
    {
    }
}
