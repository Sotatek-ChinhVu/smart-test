using Domain.Models.AccountDue;
using Domain.SuperAdminModels.Admin;
using Entity.SuperAdmin;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.SuperAdminRepositories;

public class AdminRepository : SuperAdminRepositoryBase, IAdminRepository
{
    public AdminRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }
    public AdminModel Get(int loginId, string password)
    {
        var admin = NoTrackingDataContext.Admins.Where(a => a.LoginId == loginId && a.PassWord == password).FirstOrDefault();
        var adminModel = admin == null ? new() : ConvertEntityToModel(admin);
        return adminModel;
    }

    private AdminModel ConvertEntityToModel(Admin admin)
    {
        return new AdminModel(
                admin.Id,
                admin.Name ?? string.Empty,
                admin.FullName ?? string.Empty,
                admin.Role,
                admin.LoginId,
                admin.IsDeleted,
                admin.CreateDate,
                admin.UpdateDate
            );
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
