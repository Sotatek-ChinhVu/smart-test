namespace EmrCloudApi.Realtime;

public interface IWebSocketService
{
    Task SendMessageAsync(string functionCode, object message);
}
