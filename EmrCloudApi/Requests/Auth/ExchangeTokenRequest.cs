namespace EmrCloudApi.Requests.Auth;

public class ExchangeTokenRequest
{
    public string LoginId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
