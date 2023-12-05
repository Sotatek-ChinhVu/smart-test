using Domain.Models.AccountDue;
using Domain.Models.UserToken;
using Domain.SuperAdminModels.Admin;
using Entity.SuperAdmin;
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


    public bool SignInRefreshToken(int userId, string refreshToken, DateTime expirateToken)
    {
        TrackingDataContext.UserTokens.Add(new UserToken()
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = expirateToken,
            UserId = userId
        });
        return TrackingDataContext.SaveChanges() > 0;
    }

    public UserTokenModel RefreshTokenByUser(int userId, string refreshToken, string refreshTokenNew)
    {
        var instance = TrackingDataContext.UserTokens.FirstOrDefault(x => x.UserId == userId && x.RefreshToken.Equals(refreshToken));
        if (instance is null)
            return new UserTokenModel();

        UserTokenModel info = new UserTokenModel(instance.UserId, instance.RefreshToken, instance.RefreshTokenExpiryTime, instance.RefreshTokenIsUsed);
        var expiredToken = DateTime.UtcNow.AddHours(8);
        if (!info.RefreshTokenIsValid)
            return info;
        else
        {
            instance.RefreshTokenIsUsed = true;
            TrackingDataContext.UserTokens.Add(new UserToken()
            {
                RefreshToken = refreshTokenNew,
                RefreshTokenExpiryTime = expiredToken,
                UserId = userId
            });

            var refreshTokenExpireds = TrackingDataContext.UserTokens.Where(u => u.UserId == userId && u.RefreshTokenExpiryTime < DateTime.UtcNow);
            if (refreshTokenExpireds != null)
            {
                TrackingDataContext.RemoveRange(refreshTokenExpireds);
            }

            if (TrackingDataContext.SaveChanges() > 0)
                return new UserTokenModel(instance.UserId, refreshTokenNew, expiredToken, false);
            else
                return new UserTokenModel();
        }
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
