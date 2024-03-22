using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.TenantOnboard
{
    public sealed class TenantOnboardInputData : IInputData<TenantOnboardOutputData>
    {
        public TenantOnboardInputData(int tenantId, string hospital, int adminId, string password, string subDomain, dynamic webSocketService)
        {
            TenantId = tenantId;
            Hospital = hospital;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            WebSocketService = webSocketService;
        }
        public int TenantId { get; private set; }
        public string Hospital { get; private set; } = string.Empty;
        public int AdminId { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public string SubDomain { get; private set; } = string.Empty;
        public dynamic WebSocketService { get; private set; }
    }
}
