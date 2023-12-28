using UseCase.Core.Sync.Core;

namespace UseCase.Cache
{
    public class RemoveCacheInputData : IInputData<RemoveCacheOutputData>
    {
        public RemoveCacheInputData(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}
