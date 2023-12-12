using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpgradePremium
{
    public class UpdateTenantInputData : IInputData<UpdateTenantOutputData>
    {
        public UpdateTenantInputData(int tenantId, int size, int sizeType, string subDomain, byte type, string hospital, int adminId, string password, dynamic webSocketService)
        {
            TenantId = tenantId;
            Size = size;
            SizeType = sizeType;
            SubDomain = subDomain;
            Type = type;
            Hospital = hospital;
            AdminId = adminId;
            Password = password;
            WebSocketService = webSocketService; 
        }
        public int TenantId { get; private set; }

        public int Size { get; private set; }
            
        public int SizeType { get; private set; }

        public string SubDomain { get; private set; }

        public byte Type { get; private set; }

        public string Hospital { get; private set; }

        public int AdminId { get; private set; }

        public string Password { get; private set; }

        public dynamic WebSocketService { get; private set; }
    }
}
