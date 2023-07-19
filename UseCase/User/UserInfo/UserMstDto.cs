using Domain.Models.User;

namespace UseCase.User.UserInfo
{
    public class UserMstDto
    {
        public UserMstDto(long id, int userId, int jobCd, int managerKbn, int kaId, string kaSName, string kanaName, string name, string sname, string loginId, string mayakuLicenseNo, int startDate, int endDate, int sortNo, string renkeiCd1, string drName, int hpId, List<UserPermissionModel> permissions)
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
            MayakuLicenseNo = mayakuLicenseNo;
            StartDate = startDate;
            EndDate = endDate;
            SortNo = sortNo;
            RenkeiCd1 = renkeiCd1;
            DrName = drName;
            HpId = hpId;
            Permissions = permissions;
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

        public string MayakuLicenseNo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int SortNo { get; private set; }

        public string RenkeiCd1 { get; private set; }

        public string DrName { get; private set; }

        public int HpId { get; private set; }

        public List<UserPermissionModel> Permissions { get; private set; }
    }
}
