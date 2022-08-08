namespace EmrCloudApi.Tenant.Requests.User
{
    public class UpsertUserRequest
    {
        public List<UserInfoRequest> UserInfoList { get; set; } = new List<UserInfoRequest>();
    }

    public class UserInfoRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Sname { get; set; } = string.Empty;

        public string KanaName { get; set; } = string.Empty;

        public string LoginId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int JobCd { get; set; }

        public int ManagerKbn { get; set; }

        public string MayakuLicenseNo { get; set; } = string.Empty;

        public int KaId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public string RenkeiCd { get; set; } = string.Empty;

        public int SortNo { get; set; }

        public int IsDeleted { get; set; }

        public bool IsInsertModel { get; set; }
    }
}
