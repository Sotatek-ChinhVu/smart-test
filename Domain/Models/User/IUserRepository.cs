using Domain.Common;

using static Helper.Constants.UserConst;

namespace Domain.Models.User
{
    public interface IUserRepository : IRepositoryBase
    {
        void Create(UserMstModel user);

        UserMstModel Read(int userId);

        void Update(UserMstModel user);

        void Delete(int userId);

        IEnumerable<UserMstModel> GetDoctorsList(int userId);

        IEnumerable<UserMstModel> GetDoctorsList(List<int> userIds);

        IEnumerable<UserMstModel> GetListAnyUser(List<int> userIds);

        IEnumerable<UserMstModel> GetAll();

        List<UserMstModel> GetAll(int sinDate, bool isDoctorOnly, bool isAll);

        int MaxUserId();

        UserMstModel GetByUserId(int userId);

        UserMstModel GetByUserId(int userId, int sinDate);

        UserMstModel? GetByLoginId(string loginId, string password);

        bool CheckExistedId(List<long> ids);

        bool Upsert(List<UserMstModel> upsertUserList, int userId);

        bool CheckExistedUserId(int userId);

        bool CheckExistedUserIdCreate(List<int> userIds);

        bool CheckExistedUserIdUpdate(List<long> ids, List<int> userIds);

        bool CheckExistedLoginIdCreate(List<string> loginIds);

        bool CheckExistedJobCd(List<int> jobCds);

        bool CheckExistedLoginIdUpdate(List<long> ids, List<string> loginIds);

        bool CheckLoginInfo(string userName, string password);

        bool MigrateDatabase();

        bool CheckLockMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId);

        bool NotAllowSaveMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId);

        PermissionType GetPermissionByScreenCode(int hpId, int userId, string permisionCode);

        List<UserPermissionModel> GetAllPermission(int hpId, int userId);

        List<UserMstModel> GetUsersByCurrentUser(int hpId, int currentUser);

        bool SaveListUserMst(int hpId, List<UserMstModel> users, int currentUser);

        bool GetShowRenkeiCd1ColumnSetting();

        bool UserIdIsExistInDb(int userId);

        List<int> ListDepartmentValid(int hpId);

        List<int> ListJobCdValid(int hpId);

        List<JobMstModel> GetListJob(int hpId);

        List<FunctionMstModel> GetListFunctionPermission();

        UserMstModel GetUserInfo(int hpId, int userId);

        List<UserMstModel> GetUsersByPermission(int hpId, int managerKbn);
    }
}
