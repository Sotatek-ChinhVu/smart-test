using Helper.Constants;
using Microsoft.AspNetCore.SignalR;

namespace EmrCloudApi.Realtime;

public class CommonHub : Hub<ICommonClient>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, TempIdentity.TenantId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, TempIdentity.TenantId);
        await base.OnDisconnectedAsync(exception);
    }
}
