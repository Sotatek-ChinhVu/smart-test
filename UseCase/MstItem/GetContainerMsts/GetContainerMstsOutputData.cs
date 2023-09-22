using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetContainerMsts
{
    public class GetContainerMstsOutputData : IOutputData
    {
        public GetContainerMstsOutputData(Dictionary<int, string> containerMsts, GetContainerMstsStatus status)
        {
            ContainerMsts = containerMsts;
            Status = status;
        }

        public Dictionary<int, string> ContainerMsts { get; private set; }

        public GetContainerMstsStatus Status { get; private set; }
    }
}
