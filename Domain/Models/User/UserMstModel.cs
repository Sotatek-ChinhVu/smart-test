using Helper.Extension;
using static Helper.Constants.UserConst;

namespace Domain.Models.User
{
    public class UserMstModel
    {
        public UserMstModel(int hpId, long id, int userId, int jobCd, int managerKbn, int kaId,
            string kanaName, string name, string sname, string drName, string loginId,
            string loginPass, string mayakuLicenseNo, int startDate, int endDate,
            int sortNo, string renkeiCd1, int isDeleted)
        {
            UserId = userId;
            JobCd = jobCd;
            ManagerKbn = managerKbn;
            KaId = kaId;
            KaSName = string.Empty;
            KanaName = kanaName;
            Name = name;
            Sname = sname;
            LoginId = loginId;
            LoginPass = loginPass;
            MayakuLicenseNo = mayakuLicenseNo;
            StartDate = startDate;
            EndDate = endDate;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RenkeiCd1 = renkeiCd1;
            DrName = drName;
            Id = id;
            HpId = hpId;
            Permissions = new List<UserPermissionModel>();
            FunctionMstModels = new();
        }



        public UserMstModel(int hpId, long id, int userId, int jobCd, int managerKbn, int kaId,
            string kaSName, string kanaName, string name, string sname, string drName, string loginId,
            string loginPass, string mayakuLicenseNo, int startDate, int endDate,
            int sortNo, string renkeiCd1, int isDeleted)
        {
            UserId = userId;
            JobCd = jobCd;
            ManagerKbn = managerKbn;
            KaId = kaId;
            KaSName = kaSName;
            KanaName = kanaName;
            Name = name;
            Sname = sname;
            LoginId = loginId;
            LoginPass = loginPass;
            MayakuLicenseNo = mayakuLicenseNo;
            StartDate = startDate;
            EndDate = endDate;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RenkeiCd1 = renkeiCd1;
            DrName = drName;
            Id = id;
            HpId = hpId;
            Permissions = new List<UserPermissionModel>();
            FunctionMstModels = new();
        }
        public UserMstModel()
        {
            KanaName = string.Empty;
            Name = string.Empty;
            Sname = string.Empty;
            LoginId = string.Empty;
            LoginPass = string.Empty;
            MayakuLicenseNo = string.Empty;
            IsDeleted = 1;
            RenkeiCd1 = string.Empty;
            DrName = string.Empty;
            KaSName = string.Empty;
            Permissions = new List<UserPermissionModel>();
            FunctionMstModels = new();
        }

        public UserMstModel(int hpId, long id, int userId, int jobCd, int managerKbn, int kaId, string kaSName, string kanaName, string name, string sname, string loginId, string loginPass, string mayakuLicenseNo, int startDate, int endDate, int sortNo, int isDeleted, string renkeiCd1, string drName, List<UserPermissionModel> permissions)
        {
            Id = id;
            UserId = userId;
            JobCd = jobCd;
            ManagerKbn = managerKbn;
            KaId = kaId;
            KaSName = kaSName;
            KanaName = kanaName;
            Name = name;
            Sname = sname;
            LoginId = loginId;
            LoginPass = loginPass;
            MayakuLicenseNo = mayakuLicenseNo;
            StartDate = startDate;
            EndDate = endDate;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RenkeiCd1 = renkeiCd1;
            DrName = drName;
            HpId = hpId;
            Permissions = permissions;
            FunctionMstModels = new();
        }

        public UserMstModel(int hpId, long id, int userId, int jobCd, int managerKbn, int kaId, string kaSName, string kanaName, string name, string sname, string loginId, string loginPass, string mayakuLicenseNo, int startDate, int endDate, int sortNo, int isDeleted, string renkeiCd1, string drName, List<FunctionMstModel> functionMstModels)
        {
            Id = id;
            UserId = userId;
            JobCd = jobCd;
            ManagerKbn = managerKbn;
            KaId = kaId;
            KaSName = kaSName;
            KanaName = kanaName;
            Name = name;
            Sname = sname;
            LoginId = loginId;
            LoginPass = loginPass;
            MayakuLicenseNo = mayakuLicenseNo;
            StartDate = startDate;
            EndDate = endDate;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RenkeiCd1 = renkeiCd1;
            DrName = drName;
            HpId = hpId;
            Permissions = new();
            FunctionMstModels = functionMstModels;
        }
        public UserMstModel(int hpId, int userId, string sname, string kanaName, string name, int startDate, int endDate, int isDeleted, long id)
        {
            HpId = hpId;
            UserId = userId;
            Sname = sname;
            KanaName = kanaName;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            IsDeleted = isDeleted;
            Id = id;

        }

        public long Id { get; private set; }

        public int UserId { get; private set; }

        public int JobCd { get; private set; }

        public int ManagerKbn { get; private set; }

        public int KaId { get; private set; }

        public string KaSName { get; private set; }

        public string KanaName { get; private set; }

        public string Name { get; private set; }

        public string Sname { get; private set; }

        public string LoginId { get; private set; }

        public string LoginPass { get; private set; }

        public string MayakuLicenseNo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public string RenkeiCd1 { get; private set; }

        public string DrName { get; private set; }

        public int HpId { get; private set; }

        public string SNameBinding
        {
            get
            {
                string sUserId;
                if (UserId / 10000 == 0)
                {
                    sUserId = UserId.AsString().PadLeft(5, '0');
                }
                else
                {
                    sUserId = UserId.AsString();
                }
                return "[" + sUserId + "] " + Sname;
            }
        }

        public List<UserPermissionModel> Permissions { get; private set; }

        public List<FunctionMstModel> FunctionMstModels { get; private set; }

        public ValidationStatus Validation()
        {
            if (HpId <= 0)
            {
                return ValidationStatus.InvalidHpId;
            }

            if (Id < 0)
            {
                return ValidationStatus.InvalidId;
            }

            if (UserId <= 0)
            {
                return ValidationStatus.InvalidUserId;
            }

            if (Name.Length > 40)
            {
                return ValidationStatus.InvalidName;
            }

            if (Sname.Length > 40)
            {
                return ValidationStatus.InvalidSname;
            }

            if (KanaName.Length > 40)
            {
                return ValidationStatus.InvalidKanaName;
            }

            if (LoginId.Length > 20)
            {
                return ValidationStatus.InvalidLoginId;
            }

            if (LoginPass.Length > 20)
            {
                return ValidationStatus.InvalidLoginPass;
            }

            if (JobCd <= 0)
            {
                return ValidationStatus.InvalidJobCd;
            }

            if (ManagerKbn != 0 && ManagerKbn != 7 && ManagerKbn != 9)
            {
                return ValidationStatus.InvalidManagerKbn;
            }

            if (MayakuLicenseNo.Length > 20)
            {
                return ValidationStatus.InvalidMayakuLicenseNo;
            }

            if (KaId <= 0)
            {
                return ValidationStatus.InvalidKaId;
            }

            if (SortNo <= 0)
            {
                return ValidationStatus.InvalidSortNo;
            }

            if (StartDate < 0 || StartDate > EndDate)
            {
                return ValidationStatus.InvalidStartDate;
            }

            if (EndDate <= 0)
            {
                return ValidationStatus.InvalidEndDate;
            }

            if (IsDeleted != 0 && IsDeleted != 1)
            {
                return ValidationStatus.InvalidIsDeleted;
            }

            return ValidationStatus.Valid;
        }
    }
}
