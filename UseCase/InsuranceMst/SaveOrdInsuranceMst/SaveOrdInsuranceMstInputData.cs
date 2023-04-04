using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.SaveOrdInsuranceMst
{
    public class SaveOrdInsuranceMstInputData : IInputData<SaveOrdInsuranceMstOutputData>
    {
        public SaveOrdInsuranceMstInputData(List<HokenMstModel> insurances, int hpId, int userId)
        {
            Insurances = insurances;
            HpId = hpId;
            UserId = userId;
        }

        public List<HokenMstModel> Insurances { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
