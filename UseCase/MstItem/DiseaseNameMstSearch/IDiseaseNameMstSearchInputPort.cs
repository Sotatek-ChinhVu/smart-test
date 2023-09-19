using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.SaveSetDataTenMst;

namespace UseCase.MstItem.DiseaseNameMstSearch
{
    public interface IDiseaseNameMstSearchInputPort : IInputPort<DiseaseNameMstSearchInputData, DiseaseNameMstSearchOutputData>
    {
    }
}
