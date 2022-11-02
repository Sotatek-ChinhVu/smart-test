using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.Create
{
    public class CreateUserInputData : IInputData<CreateUserOutputData>
    {
        public CreateUserInputData(int hpId, int jobCd, int managerKbn, int kaId, string kanaName, string name, string sname, string loginId, string loginPass, string mayakuLicenseNo, int startDate, int endDate, int sortNo, int isDeleted, string renkeiCd1, string drName)
        {
            HpId = hpId;
            JobCd = jobCd;
            ManagerKbn = managerKbn;
            KaId = kaId;
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
        }

        public int HpId { get;private set; }
        public int JobCd { get; private set; }
        public int ManagerKbn { get; private set; }
        public int KaId { get; private set; }
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


        public UserMstModel GenerateUserModel(int userId)
        {
            return new UserMstModel(HpId, 0, userId, JobCd, ManagerKbn, KaId, KanaName, Name, Sname, DrName, LoginId, LoginPass, MayakuLicenseNo, StartDate, EndDate, SortNo, RenkeiCd1, IsDeleted);
        }
    }
}
