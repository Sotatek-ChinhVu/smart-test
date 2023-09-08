using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveAddressMst
{
    public class SaveAddressMstOutputData : IOutputData
    {
        public SaveAddressMstOutputData(SaveAddressMstStatus status)
        {
            Status = status;
        }

        public SaveAddressMstStatus Status { get; set; }
    }
}
