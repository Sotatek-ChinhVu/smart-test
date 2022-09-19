using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.OrdInfs;
using EmrCloudApi.Tenant.Requests.OrdInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public OrdInfController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetOrdInfListTreeResponse>> GetList([FromQuery] GetOrdInfListTreeRequest request)
        {
            var input = new GetOrdInfListTreeInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate, request.IsDeleted, request.UserId);
            var output = _bus.Handle(input);

            var presenter = new GetOrdInfListTreePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetOrdInfListTreeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetMaxRpNo)]
        public ActionResult<Response<GetMaxRpNoResponse>> GetMaxRpNo([FromBody] GetMaxRpNoRequest request)
        {
            var input = new GetMaxRpNoInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxRpNoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMaxRpNoResponse>>(presenter.Result);
        }
    }
}
