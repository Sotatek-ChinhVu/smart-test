namespace AWSSDK.Dto
{
    public class TenantCacheMemory
    {
        public TenantCacheMemory(CancellationTokenSource cancelToken, string tmpRdsIdentifier)
        {
            CancelToken = cancelToken;
            TmpRdsIdentifier = tmpRdsIdentifier;
        }

        public TenantCacheMemory()
        {
        }

        public CancellationTokenSource CancelToken { get; set; }
        public string TmpRdsIdentifier { get; set; }
    }
}
