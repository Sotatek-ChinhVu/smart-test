using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteFilter;
using EmrCloudApi.Tenant.Requests.KarteFilter;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteFilter;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KarteFilterController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteFilterController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetKarteFilterMstResponse>>> GetList()
        {
            var input = new GetKarteFilterInputData();
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKarteFilterMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
        public async Task<ActionResult<Response<SaveKarteFilterMstResponse>>> SaveList([FromBody] SaveKarteFilterMstRequest request)
        {
            var input = new SaveKarteFilterInputData(request.KarteFilters);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveKarteFilterMstResponse>>(presenter.Result);
        }
    }
}
