using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.TenantOnboard
{
    public sealed class TenantOnboardInputData : IInputData<TenantOnboardOutputData>
    {
        public TenantOnboardInputData(int tenantId, string hospital, int adminId, string password, string subDomain, double size, int sizeType, byte clusterMode, dynamic webSocketService)
        {
            TenantId = tenantId;
            Hospital = hospital;
            AdminId = adminId;
            Password = password;
            SubDomain = subDomain;
            Size = size;
            SizeType = sizeType;
            ClusterMode = clusterMode;
            WebSocketService = webSocketService;
        }
        public int TenantId { get; private set; }
        public string Hospital { get; private set; } = string.Empty;
        public int AdminId { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public string SubDomain { get; private set; } = string.Empty;

        /// <summary>
        /// Data size per year
        /// </summary>
        public double Size { get; private set; }

        /// <summary>
        /// 1: MB , 2: GB
        /// </summary>
        public int SizeType { get; private set; }

        /// <summary>
        /// 0: Sharing , 1: Dedicated
        /// </summary>
        public byte ClusterMode { get; private set; }

        public dynamic WebSocketService { get; private set; }
    }
}
