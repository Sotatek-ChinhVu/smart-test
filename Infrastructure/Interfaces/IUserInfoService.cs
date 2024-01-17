using Entity.Tenant;

namespace Infrastructure.Interfaces;

public interface IUserInfoService
{
    void Reload();

    string GetNameById(int id);

    string GetFullNameById(int id);

    void DisposeSource();

    List<UserMst> AllUserMstList();
}
