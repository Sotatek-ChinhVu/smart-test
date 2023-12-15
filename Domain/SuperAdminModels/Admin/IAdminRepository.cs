using Domain.Common;
using Domain.Models.UserToken;

namespace Domain.SuperAdminModels.Admin;

public interface IAdminRepository : IRepositoryBase
{
    AdminModel Get(int loginId, string password);

    bool SignInRefreshToken(int userId, string refreshToken, DateTime expirateToken);
    UserTokenModel RefreshTokenByUser(int userId, string refreshToken, string refreshTokenNew);

    void ReleaseResource();
}
