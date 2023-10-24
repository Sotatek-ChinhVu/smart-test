namespace Domain.Models.User
{
    public class UserPermissionModel
    {
        public UserPermissionModel(int hpId, int userId, string functionCd, int permission, bool isDefault)
        {
            HpId = hpId;
            UserId = userId;
            FunctionCd = functionCd;
            Permission = permission;
            IsDefault = isDefault;
        }

        public UserPermissionModel(int userId)
        {
            UserId = userId;
            FunctionCd = string.Empty;
        }

        public UserPermissionModel()
        {
            FunctionCd = string.Empty;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string FunctionCd { get; private set; }

        public int Permission { get; private set; }

        public bool IsDefault { get; private set; }
    }
}
