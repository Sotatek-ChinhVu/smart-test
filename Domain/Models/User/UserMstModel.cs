using Domain.Constant;
using Helper.Common;
using Helper.Constants;
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
        }

        public long Id { get; private set; }
        public int UserId { get; private set; }
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
        public int HpId { get; private set; }

        public ValidationStatus Validation()
        {
            #region common

            if (HpId <= 0)
            {
                return ValidationStatus.InvalidHpId;
            }

            if (Id < 0)
            {
                return ValidationStatus.InvalidId;
            }

            if (UserId < 0)
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
            if (StartDate < 0)
            {
                return ValidationStatus.InvalidStartDate;
            }
            if (EndDate < 0)
            {
                return ValidationStatus.InvalidEndDate;
            }
            if (IsDeleted != 0 && IsDeleted != 1)
            {
                return ValidationStatus.InvalidIsDeleted;
            }
            #endregion

            return ValidationStatus.Valid;
        }
    }
}
