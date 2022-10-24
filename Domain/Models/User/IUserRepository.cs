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

        bool CheckExistedUserIdCreate(List<int> UserIds);

        bool CheckExistedUserIdUpdate(List<long> Ids, List<int> UserIds);

        bool CheckExistedLoginIdCreate(List<string> LoginIds);

        bool CheckExistedJobCd(List<int> JobCds);

        bool CheckExistedLoginIdUpdate(List<long> Ids, List<string> LoginIds);

        bool CheckInputData(List<int> UserIds, List<string> LoginIds);
    }
}
