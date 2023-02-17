using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.SaveHokenMaster
{
    public class SaveHokenMasterInputData : IInputData<SaveHokenMasterOutputData>
    {
        public SaveHokenMasterInputData(int hpId, int userId, HokenMstModel insurance)
        {
            HpId = hpId;
            UserId = userId;
            Insurance = insurance;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public HokenMstModel Insurance { get; private set; }
    }
}
