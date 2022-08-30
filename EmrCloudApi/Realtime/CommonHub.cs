using Microsoft.AspNetCore.SignalR;

namespace EmrCloudApi.Realtime;

public class CommonHub : Hub<ICommonClient>
{
    public override async Task OnConnectedAsync()
    {
        await IfTenantIdExists(tenantId =>
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, tenantId);
        });

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await IfTenantIdExists(tenantId =>
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId);
        });

        await base.OnDisconnectedAsync(exception);
    }

    private async Task IfTenantIdExists(Func<string, Task> useTenantId)
    {
        var tenantId = GetTenantId();
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            await useTenantId(tenantId);
        }
    }

    private string GetTenantId()
    {
        return Context.GetHttpContext()!.Request.Query["tenantId"].ToString();
    }
}
