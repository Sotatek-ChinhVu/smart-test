using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Notification;
using SuperAdminAPI.Reponse.Notification;
using SuperAdminAPI.Request.Notification;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.GetNotification;

namespace SuperAdminAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public NotificationController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet("GetNotification")]
    public ActionResult<Response<GetNotificationResponse>> GetNotification([FromQuery] GetNotificationRequest request)
    {
        var input = new GetNotificationInputData(request.Skip, request.Take);
        var output = _bus.Handle(input);
        var presenter = new GetNotificationPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetNotificationResponse>>(presenter.Result);
    }
}
