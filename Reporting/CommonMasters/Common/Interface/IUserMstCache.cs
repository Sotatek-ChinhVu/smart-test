namespace Reporting.CommonMasters.Common.Interface;

public interface IUserMstCache
{
    string GetUserSNameIncludedDeleted(int userId, bool fromLastestDb = false);
}
