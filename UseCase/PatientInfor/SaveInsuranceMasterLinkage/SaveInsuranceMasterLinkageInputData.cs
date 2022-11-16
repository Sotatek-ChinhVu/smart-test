using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.SaveInsuranceMasterLinkage
{
    public class SaveInsuranceMasterLinkageInputData : IInputData<SaveInsuranceMasterLinkageOutputData>
    {
        public SaveInsuranceMasterLinkageInputData(List<DefHokenNoModel> defHokenNoModels, int hpId, int userId)
        {
            DefHokenNoModels = defHokenNoModels;
            HpId = hpId;
            UserId = userId;
        }

        public List<DefHokenNoModel> DefHokenNoModels { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
