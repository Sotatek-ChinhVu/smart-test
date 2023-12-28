using UseCase.Core.Sync.Core;

namespace UseCase.Cache.RemoveAllCache
{
    public class RemoveAllCacheOutputData : IOutputData
    {
        public RemoveAllCacheOutputData(RemoveAllCacheStaus status) 
        {
            Status = status;
        }
        
        public RemoveAllCacheStaus Status { get; private set; }
    }
}
