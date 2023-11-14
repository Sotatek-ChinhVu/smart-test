using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public TenantController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("UpgradePremium")]
        public ActionResult<Response<UpgradePremiumResponse>> UpgradePremium([FromBody] UpgradePremiumRequest request)
        {
            var input = new UpgradePremiumInputData();
            var output = _bus.Handle(input);
            var presenter = new UpgradePremiumPresenter();
            //presenter.Complete(output);
            //return new ActionResult<Response<UpgradePremiumResponse>>();
            return Ok();
        }
    }
}
