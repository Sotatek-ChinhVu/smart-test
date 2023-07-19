namespace UseCase.UserToken.GetInfoRefresh
{
    public enum RefreshTokenByUserStatus
    {
        Successful,
        CurrentRefreshTokenIsInvalid,
        InvalidUserId,
        InvalidRefreshToken
    }
}
