using UseCase.Core.Sync.Core;

namespace UseCase.Cache
{
    public class RemoveCacheOutputData : IOutputData
    {
        public RemoveCacheOutputData(RemoveCacheStaus status) 
        {
            Status = status;
        }

        public RemoveCacheStaus Status { get; private set; }
    }
}
