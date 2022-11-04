using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetKbnMst;
using EmrCloudApi.Tenant.Requests.SetKbnMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbnMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbnMst.GetList;

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
        public async Task<ActionResult<Response<GetSetKbnMstListResponse>>> GetList([FromQuery] GetSetKbnMstListRequest request)
        {
            var input = new GetSetKbnMstListInputData(request.HpId, request.SinDate, request.SetKbnFrom, request.SetKbnTo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSetKbnMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnMstListResponse>>(presenter.Result);
        }
    }
}
