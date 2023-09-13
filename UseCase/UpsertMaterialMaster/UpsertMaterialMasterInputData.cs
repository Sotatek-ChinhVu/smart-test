using Domain.Models.MaterialMaster;
using UseCase.Core.Sync.Core;

namespace UseCase.UpsertMaterialMaster
{
    public class UpsertMaterialMasterInputData : IInputData<UpsertMaterialMasterOutputData>
    {
        public UpsertMaterialMasterInputData(int hpId, int userId, List<MaterialMasterModel> materialMasters)
        {
            HpId = hpId;
            UserId = userId;
            MaterialMasters = materialMasters;
        }

        public int HpId { get; private set; }

        public List<MaterialMasterModel> MaterialMasters { get; private set; }

        public int UserId { get; private set; }
    }
}
