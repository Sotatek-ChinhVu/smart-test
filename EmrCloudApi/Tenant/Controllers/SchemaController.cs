using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Schema;
using EmrCloudApi.Tenant.Requests.Schema;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.SaveImageTodayOrder;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SchemaController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListImageTemplatesResponse>> GetList()
        {
            var input = new GetListImageTemplatesInputData();
            var output = _bus.Handle(input);

            var presenter = new GetListImageTemplatesPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListImageTemplatesResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveImageTodayOrder)]
        public ActionResult<Response<SaveImageResponse>> SaveImageTodayOrder([FromQuery] SaveImageTodayOrderRequest request)
        {
            var input = new SaveImageTodayOrderInputData(request.HpId, request.PtId, request.RaiinNo, request.OldImage, Request.Body);
            var output = _bus.Handle(input);

            var presenter = new SaveImageTodayOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }
    }
}
