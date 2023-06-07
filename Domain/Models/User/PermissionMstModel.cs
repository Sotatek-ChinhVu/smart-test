namespace Domain.Models.User
{
    public class PermissionMstModel
    {
        public PermissionMstModel(string functionCd, int permission)
        {
            FunctionCd = functionCd;
            Permission = permission;
        }

        public string FunctionCd { get; set; } 

        public int Permission { get; set; }

        public string PermissionKey
        {
            get
            {
                //0: 制限なし(既定値)
                //1: 参照権限
                //99:使用不可"
                switch (Permission)
                {
                    case 0:
                        return " 制限なし";
                    case 1:
                        return "参照権限";
                    case 99:
                        return "使用不可";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
