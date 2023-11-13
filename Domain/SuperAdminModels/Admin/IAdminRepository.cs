using Domain.Common;

namespace Domain.SuperAdminModels.Admin;

public interface IAdminRepository : IRepositoryBase
{
    AdminModel Get(int loginId, string password);

    void ReleaseResource();
}
