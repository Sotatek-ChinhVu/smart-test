using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetKbn;
using EmrCloudApi.Tenant.Requests.SetKbn;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbn;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbn.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetKbnController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetKbnController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSetKbnListResponse>> GetList([FromQuery] GetSetKbnListRequest request)
        {
            var input = new GetSetKbnListInputData(request.HpId, request.SinDate, request.SetKbnFrom, request.SetKbnTo);
            var output = _bus.Handle(input);

            var presenter = new GetSetKbnListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnListResponse>>(presenter.Result);
        }
    }
}
