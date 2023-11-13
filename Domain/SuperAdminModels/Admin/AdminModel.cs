namespace Domain.SuperAdminModels.Admin
{
    public class AdminModel
    {
        public AdminModel(int id, string name, string fullName, int role, int loginId, int isDeleted, DateTime createDate, DateTime updateDate)
        {
            Id = id;
            Name = name;
            FullName = fullName;
            Role = role;
            LoginId = loginId;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            UpdateDate = updateDate;
            PassWord = string.Empty;
        }

        public AdminModel()
        {
            FullName = string.Empty;
            Name = string.Empty;
            PassWord = string.Empty;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public int Role { get; private set; }

        public int LoginId { get; private set; }

        public int IsDeleted { get; private set; }

        public string PassWord { get; set; }

        public DateTime CreateDate { get; private set; }

        public DateTime UpdateDate { get; private set; }
    }
}
