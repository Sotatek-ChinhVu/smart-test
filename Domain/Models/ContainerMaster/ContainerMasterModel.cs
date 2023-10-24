using Helper.Constants;

namespace Domain.Models.ContainerMaster
{
    public class ContainerMasterModel
    {
        public ContainerMasterModel(long containerCd, string containerName, ModelStatus containerModelStatus)
        {
            ContainerCd = containerCd;
            ContainerName = containerName;
            ContainerModelStatus = containerModelStatus;
        }

        public long ContainerCd { get; private set; }

        public string ContainerName { get; private set; }
        
        public ModelStatus ContainerModelStatus { get; private set; }
    }
}
