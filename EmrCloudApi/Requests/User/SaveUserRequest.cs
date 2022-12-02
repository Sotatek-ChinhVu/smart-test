namespace EmrCloudApi.Requests.User
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public int HpId { get; set; }
        public int JobCd { get; set; }
        public int ManagerKbn { get; set; }
        public int KaId { get; set; }
        public string KanaName { get; set; } = string.Empty;
        public string Sname { get; set; } = string.Empty;
        public string LoginId { get; set; } = string.Empty;
        public string LoginPass { get; set; } = string.Empty;
        public string MayakuLicenseNo { get; set; } = string.Empty;
        public int StartDate { get; set; }
        public int Endate { get; set; }
        public int SortNo { get; set; }
        public string RenkeiCd1 { get; set; } = string.Empty;
        public string DrName { get; set; } = string.Empty;
    }
}
