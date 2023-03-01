namespace EmrCloudApi.Services;

public class LoginTokenResponse
{
    public int HpId { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
}
