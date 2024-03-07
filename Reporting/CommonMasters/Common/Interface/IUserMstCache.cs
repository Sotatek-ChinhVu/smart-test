namespace Reporting.CommonMasters.Common.Interface;

public interface IUserMstCache
{
    string GetUserSNameIncludedDeleted(int hpId, int userId, bool fromLastestDb = false);
}
