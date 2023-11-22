using Domain.SuperAdminModels.Tenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.GetTenant;
using UseCase.SuperAdmin.TenantOnboard;
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
            var input = new UpgradePremiumInputData(request.TenantId);
            var output = _bus.Handle(input);
            var presenter = new UpgradePremiumPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpgradePremiumResponse>>(presenter.Result);
        }

        [HttpPost("GetTenant")]
        public ActionResult<Response<GetTenantResponse>> GetTenant([FromBody] GetTenantRequest request)
        {
            var input = new GetTenantInputData(request.TenantId, GetSearchTenantModel(request.SearchModel), request.SortDictionary, request.Skip, request.Take);
            var output = _bus.Handle(input);
            var presenter = new GetTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenantResponse>>(presenter.Result);
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

        #region 
        private SearchTenantModel GetSearchTenantModel(SearchTenantRequestItem requestItem)
        {
            return new SearchTenantModel(
                       requestItem.KeyWord,
                       requestItem.FromDate,
                       requestItem.ToDate,
                       requestItem.Type,
                       requestItem.Status,
                       requestItem.StorageFull);
        }
        #endregion
    }
}
