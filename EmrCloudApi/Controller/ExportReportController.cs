using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Reporting;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ExportReportController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IReporting _reporting;
    public ExportReportController(UseCaseBus bus, IUserService userService, IReporting reporting) : base(userService)
    {
        _bus = bus;
        _reporting = reporting;
    }
}
