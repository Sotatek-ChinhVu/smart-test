using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetMaterialMsts
{
    public class GetMaterialMstsOutputData : IOutputData
    {
        public GetMaterialMstsOutputData(Dictionary<int, string> getContainerMsts, GetMaterialMstsStatus status)
        {
            GetContainerMsts = getContainerMsts;
            Status = status;
        }

        public Dictionary<int, string> GetContainerMsts { get; private set; }

        public GetMaterialMstsStatus Status { get; private set; }
    }
}
