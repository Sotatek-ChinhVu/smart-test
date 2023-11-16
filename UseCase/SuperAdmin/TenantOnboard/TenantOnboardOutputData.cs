using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.TenantOnboard
{
    public sealed class TenantOnboardOutputData : IOutputData
    {
        public TenantOnboardOutputData(TenantOnboardItem data, TenantOnboardStatus status)
        {
            Data = data;
            Status = status;
        }
        public TenantOnboardItem Data { get; private set; }
        public TenantOnboardStatus Status { get; private set; }
    }
}
