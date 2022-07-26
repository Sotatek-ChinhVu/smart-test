using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Diseases.GetDiseaseList
{
    public interface IGetPtDiseaseListInputPort : IInputPort<GetPtDiseaseListInputData, GetPtDiseaseListOutputData>
    {
    }
}
