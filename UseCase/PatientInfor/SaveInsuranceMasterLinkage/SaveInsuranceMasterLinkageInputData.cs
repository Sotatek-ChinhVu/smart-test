using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SaveInsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageInputData : IInputData<SaveInsuranceMasterLinkageOutputData>
    {
        public SaveInsuranceMasterLinkageInputData(List<DefHokenNoModel> defHokenNoModels)
        {
            DefHokenNoModels = defHokenNoModels;
        }

        public List<DefHokenNoModel> DefHokenNoModels { get; private set; }
    }
}
