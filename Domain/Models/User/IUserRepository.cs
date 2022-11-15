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

        IEnumerable<UserMstModel> GetListAnyUser(List<int> userIds);

        IEnumerable<UserMstModel> GetAll();

        List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly);

        int MaxUserId();

        UserMstModel? GetByUserId(int userId);

        UserMstModel? GetByLoginId(string loginId);

        bool CheckExistedId(List<long> ids);

        void Upsert(List<UserMstModel> upsertUserList, int userId);

        bool CheckExistedUserId(int userId);

        bool CheckExistedUserIdCreate(List<int> userIds);

        bool CheckExistedUserIdUpdate(List<long> ids, List<int> userIds);

        bool CheckExistedLoginIdCreate(List<string> loginIds);

        bool CheckExistedJobCd(List<int> jobCds);

        bool CheckExistedLoginIdUpdate(List<long> ids, List<string> loginIds);
    }
}
