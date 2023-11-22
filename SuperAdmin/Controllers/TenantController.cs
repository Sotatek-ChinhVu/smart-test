using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.TenantOnboard;
using UseCase.SuperAdmin.TerminateTenant;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var input = new UpgradePremiumInputData(request.TenantId, request.Size, request.Domain);
            var output = _bus.Handle(input);
            var presenter = new UpgradePremiumPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpgradePremiumResponse>>(presenter.Result);
        }

        [HttpPost("TenantOnboard")]
        public ActionResult<Response<TenantOnboardResponse>> TenantOnboardAsync([FromBody] TenantOnboardRequest request)
        {
            var input = new TenantOnboardInputData(
                request.Hospital,
                request.AdminId,
                request.Password,
                request.SubDomain,
                request.Size,
                request.SizeType,
                request.ClusterMode);
            var output = _bus.Handle(input);
            var presenter = new TenantOnboardPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TenantOnboardResponse>>(presenter.Result);
        }

        [HttpPost("TerminateTenant")]
        public ActionResult<Response<TerminateTenantResponse>> TerminateTenant([FromBody] TerminateTenantRequest request)
        {
            var input = new TerminateTenantInputData(request.TenantId);
            var output = _bus.Handle(input);
            var presenter = new TerminateTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TerminateTenantResponse>>(presenter.Result);
        }
    }
}
