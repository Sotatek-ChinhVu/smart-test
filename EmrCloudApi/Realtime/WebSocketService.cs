using Infrastructure.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace EmrCloudApi.Realtime;

public class WebSocketService : IWebSocketService
{
    private readonly IHubContext<CommonHub, ICommonClient> _hubContext;
    private readonly ITenantProvider _tenantProvider;

    public WebSocketService(IHubContext<CommonHub, ICommonClient> hubContext,
        ITenantProvider tenantProvider)
    {
        _hubContext = hubContext;
        _tenantProvider = tenantProvider;
    }

    public async Task SendMessageAsync(string functionCode, object message)
    {
        var tenantId = _tenantProvider.GetTenantId();
        await _hubContext.Clients.Group(tenantId).ReceiveMessage(functionCode, message);
    }
}
