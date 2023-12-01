using Microsoft.AspNetCore.SignalR;

namespace Interactor.Realtime;

public class WebSocketService : IWebSocketService
{
    private readonly IHubContext<CommonHub, ICommonClient> _hubContext;

    public WebSocketService(IHubContext<CommonHub, ICommonClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessageAsync(string functionCode, object message)
    {
        await _hubContext.Clients.Group("SuperAdmin").ReceiveMessage(functionCode, message);
    }
}
