using Domain.Models.ContainerMaster;
using UseCase.Core.Sync.Core;

namespace UseCase.ContainerMasterUpdate
{
    public class ContainerMasterUpdateInputData : IInputData<ContainerMasterUpdateOutPutData>
    {
        public ContainerMasterUpdateInputData(int hpId, int userId, List<ContainerMasterModel> containerMasters)
        {
            HpId = hpId;
            ContainerMasters = containerMasters;
            UserId = userId;
        }

        public int HpId { get; private set; }

        public List<ContainerMasterModel> ContainerMasters { get; private set; }

        public int UserId { get; private set; }
    }
}
