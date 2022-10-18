namespace Domain.Models.User
{
    public interface IUserRepository
    {
        void Create(UserMstModel user);

        UserMstModel Read(int userId);

        void Update(UserMstModel user);

        void Delete(int userId);

        IEnumerable<UserMstModel> GetDoctorsList(int userId);

        IEnumerable<UserMstModel> GetDoctorsList(List<int> userIds);

        IEnumerable<UserMstModel> GetAll();

        List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly);

        int MaxUserId();

        UserMstModel? GetByUserId(int userId);

        UserMstModel? GetByLoginId(string loginId);

        bool CheckExistedId(List<long> idList);

        void Upsert(List<UserMstModel> upsertUserList);

        bool CheckExistedUserId(int userId);

        bool CheckExistedUserIdCreate(int userId);

        bool CheckExistedUserIdUpdate(long id, int userId);

        bool CheckExistedLoginIdCreate(string loginId);

        bool CheckExistedLoginIdUpdate(long id, string loginId);

        bool CheckExistedJobCd(int jobCd);
    }
}
