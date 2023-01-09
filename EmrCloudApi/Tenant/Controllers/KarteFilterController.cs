using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteFilter;
using EmrCloudApi.Tenant.Requests.KarteFilter;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteFilter;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class KarteFilterController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteFilterController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetKarteFilterMstResponse>> GetList()
        {
            var input = new GetKarteFilterInputData(HpId, UserId);
            var output = _bus.Handle(input);

            var presenter = new GetKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKarteFilterMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
        public ActionResult<Response<SaveKarteFilterMstResponse>> SaveList([FromBody] SaveKarteFilterMstRequest request)
        {
            var input = new SaveKarteFilterInputData(HpId, UserId, request.KarteFilters);
            var output = _bus.Handle(input);

            var presenter = new SaveKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveKarteFilterMstResponse>>(presenter.Result);
        }
    }
}
