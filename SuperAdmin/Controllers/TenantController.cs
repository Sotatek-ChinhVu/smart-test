using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.GetTenant;
using UseCase.SuperAdmin.GetTenantDetail;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;
using UseCase.SuperAdmin.RestoreTenant;
using UseCase.SuperAdmin.StopedTenant;
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
        private IWebSocketService _webSocketService;
        public TenantController(UseCaseBus bus, IWebSocketService webSocketService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpPost("UpgradePremium")]
        public ActionResult<Response<UpgradePremiumResponse>> UpgradePremium([FromBody] UpgradePremiumRequest request)
        {
            var input = new UpgradePremiumInputData(request.TenantId, request.Size, request.SizeType, request.Domain, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new UpgradePremiumPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpgradePremiumResponse>>(presenter.Result);
        }

        [HttpPost("GetTenant")]
        public ActionResult<Response<GetTenantResponse>> GetTenant([FromBody] GetTenantRequest request)
        {
            var input = new GetTenantInputData(GetSearchTenantModel(request.SearchModel), request.SortDictionary, request.Skip, request.Take);
            var output = _bus.Handle(input);
            var presenter = new GetTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenantResponse>>(presenter.Result);
        }

        [HttpGet("GetTenantDetail")]
        public ActionResult<Response<GetTenantDetailResponse>> GetTenantDetail([FromQuery] GetTenantDetailRequest request)
        {
            var input = new GetTenantDetailInputData(request.TenantId);
            var output = _bus.Handle(input);
            var presenter = new GetTenantDetailPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenantDetailResponse>>(presenter.Result);
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
                request.ClusterMode,
                _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new TenantOnboardPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TenantOnboardResponse>>(presenter.Result);
        }

        [HttpPost("TerminateTenant")]
        public ActionResult<Response<TerminateTenantResponse>> TerminateTenant([FromBody] TerminateTenantRequest request)
        {
            var input = new TerminateTenantInputData(request.TenantId, _webSocketService, request.Type);
            var output = _bus.Handle(input);
            var presenter = new TerminateTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TerminateTenantResponse>>(presenter.Result);
        }

        #region 
        private SearchTenantModel GetSearchTenantModel(SearchTenantRequestItem requestItem)
        {
            return new SearchTenantModel(
                       requestItem.KeyWord,
                       requestItem.FromDate,
                       requestItem.ToDate,
                       requestItem.Type,
                       requestItem.StatusTenant,
                       requestItem.StorageFull);
        }
        #endregion

        [HttpPost("ToggleTenant")]
        public ActionResult<Response<ToggleTenantResponse>> ToggleTenant([FromBody] ToggleTenantRequest request)
        {
            var input = new ToggleTenantInputData(request.TenantId, _webSocketService, request.Type);
            var output = _bus.Handle(input);
            var presenter = new ToggleTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ToggleTenantResponse>>(presenter.Result);
        }

        [HttpPost("RestoreTenant")]
        public ActionResult<Response<RestoreTenantResponse>> RestoreTenant([FromBody] RestoreTenantRequest request)
        {
            var input = new RestoreTenantInputData(request.TenantId, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new RestoreTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RestoreTenantResponse>>(presenter.Result);
        }

        [HttpPost("RestoreObjectS3Tenant")]
        public ActionResult<Response<RestoreObjectS3TenantResponse>> RestoreObjectS3Tenant([FromBody] RestoreObjectS3TenantRequest request)
        {
            var input = new RestoreObjectS3TenantInputData(request.ObjectName, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new RestoreObjectS3TenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RestoreObjectS3TenantResponse>>(presenter.Result);
        }

        [HttpPost("UpdateTenant")]
        public ActionResult<Response<RestoreObjectS3TenantResponse>> UpdateTenant([FromBody] RestoreObjectS3TenantRequest request)
        {
            var input = new RestoreObjectS3TenantInputData(request.ObjectName, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new RestoreObjectS3TenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RestoreObjectS3TenantResponse>>(presenter.Result);
        }
    }
}
