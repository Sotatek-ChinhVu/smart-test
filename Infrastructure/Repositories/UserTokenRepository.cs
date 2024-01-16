using Domain.Models.UserToken;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserTokenRepository : RepositoryBase, IUserTokenRepository
    {
        public UserTokenRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
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
                //instance.RefreshTokenIsUsed = true;
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
