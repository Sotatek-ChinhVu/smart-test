using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetInsuranceMasterLinkage
{
    public class GetInsuranceMasterLinkageOutputData : IOutputData
    {

        public List<DefHokenNoModel> DefHokenNoModels { get; private set; }
    }

}
