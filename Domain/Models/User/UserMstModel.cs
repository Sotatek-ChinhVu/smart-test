using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.User;

namespace Domain.Models.User
{
    public class UserMstModel
    {
        public UserMstModel(int hpId, long id, int userId, int jobCd, int managerKbn, int kaId, string kanaName, string name, string sname, string loginId, string loginPass, string mayakuLicenseNo, int startDate, int endDate, int sortNo, int isDeleted, string renkeiCd1, string drName)
        {
            HpId = hpId;
            Id = id;
            UserId = userId;
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

        public int HpId { get; set; }
        public long Id { get; set; }
        public int UserId { get; set; }
        public int JobCd { get; set; }
        public int ManagerKbn { get; set; }
        public int KaId { get; set; }
        public string KanaName { get; set; }
        public string Name { get; set; }
        public string Sname { get; set; }
        public string LoginId { get; set; }
        public string LoginPass { get; set; }
        public string MayakuLicenseNo { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
        public int SortNo { get; set; }
        public int IsDeleted { get; set; }
        public string RenkeiCd1 { get; set; }
        public string DrName { get; set; }
       
    }
}
