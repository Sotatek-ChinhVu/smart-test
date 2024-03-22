using Domain.Common;

using static Helper.Constants.UserConst;

namespace Domain.Models.User
{
    public interface IUserRepository : IRepositoryBase
    {
        void Create(UserMstModel user);

        IEnumerable<UserMstModel> GetDoctorsList(int hpId, int userId);

        IEnumerable<UserMstModel> GetDoctorsList(int hpId, List<int> userIds);

        IEnumerable<UserMstModel> GetListAnyUser(int hpId, List<int> userIds);

        List<UserMstModel> GetAll(int hpId, int sinDate, bool isDoctorOnly, bool isAll);

        int MaxUserId(int hpId);

        UserMstModel GetByUserId(int hpId, int userId);

        UserMstModel GetByUserId(int hpId, int userId, int sinDate);

        UserMstModel? GetByLoginId(string loginId, string password);

        bool CheckExistedId(int hpId, List<int> ids);

        bool Upsert(int hpId, List<UserMstModel> upsertUserList, int userId);

        bool CheckExistedUserId(int hpId, int userId);

        bool CheckExistedUserIdCreate(int hpId, List<int> userIds);

        bool CheckExistedUserIdUpdate(int hpId, List<int> ids, List<int> userIds);

        bool CheckExistedLoginIdCreate(int hpId, List<string> loginIds);

        bool CheckExistedJobCd(int hpId, List<int> jobCds);

        bool CheckExistedLoginIdUpdate(int hpId, List<int> ids, List<string> loginIds);

        bool CheckLoginInfo(string userName, string password);

        bool MigrateDatabase();

        bool CheckLockMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId);

        bool NotAllowSaveMedicalExamination(int hpId, long ptId, long raiinNo, int sinDate, int userId);

        PermissionType GetPermissionByScreenCode(int hpId, int userId, string permisionCode);

        List<UserPermissionModel> GetAllPermission(int hpId, int userId);

        List<UserMstModel> GetUsersByCurrentUser(int hpId, int currentUser);

        bool SaveListUserMst(int hpId, List<UserMstModel> users, int currentUser);

        bool GetShowRenkeiCd1ColumnSetting(int hpId);

        bool UserIdIsExistInDb(int hpId, int userId);

        List<int> ListDepartmentValid(int hpId);

        List<int> ListJobCdValid(int hpId);

        List<JobMstModel> GetListJob(int hpId);

        List<FunctionMstModel> GetListFunctionPermission(int hpId);

        UserMstModel GetUserInfo(int hpId, int userId);

        List<UserMstModel> GetUsersByPermission(int hpId, int managerKbn);

        void UpdateHashPassword();

        byte[] GenerateSalt();

        byte[] CreateHash(byte[] password, byte[] salt);
    }
}
