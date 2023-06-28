namespace Helper.Constants
{
    public class UserPermissionConst
    {
        public const int Normal = 0;
        public const int Admin = 7;
        public const int AdminSystem = 9;

        public enum Permission
        {
            /// <summary>
            /// 一般
            /// </summary>
            Normal = UserPermissionConst.Normal,
            /// <summary>
            /// 管理者
            /// </summary>
            Admin = UserPermissionConst.Admin,
            /// <summary>
            /// システム管理者
            /// </summary>
            AdminSystem = UserPermissionConst.AdminSystem
        }

        public const int Unlimited = 0;
        public const int ReadOnly = 1;
        public const int NotAvailable = 99;

        public enum FunctionPermission
        {
            Unlimited,
            ReadOnly,
            NotAvailable = 99,
        }
    }
}
