using Domain.Common;

namespace Domain.Models.UserToken
{
    public interface IUserTokenRepository : IRepositoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expirateToken"></param>
        /// <returns></returns>
        bool SignInRefreshToken(int userId, string refreshToken, DateTime expirateToken);

        /// <summary>
        /// Write new refresh token by current refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"> current refresh token</param>
        /// <param name="refreshTokenNew"> new refresh token</param>
        /// <returns></returns>
        UserTokenModel RefreshTokenByUser(int userId, string refreshToken, string refreshTokenNew);

        /// <summary>
        /// Check RefreshToken is valid for report
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        bool RefreshTokenIsValid(int userId, string refreshToken);
    }
}
