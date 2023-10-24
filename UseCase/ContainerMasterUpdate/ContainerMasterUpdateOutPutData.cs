using UseCase.Core.Sync.Core;

namespace UseCase.ContainerMasterUpdate
{
    public class ContainerMasterUpdateOutPutData : IOutputData
    {
        public ContainerMasterUpdateOutPutData(ContainerMasterUpdateStatus status) 
        {
            Status = status;
        }

        public ContainerMasterUpdateStatus Status { get; private set; }
    }
}
