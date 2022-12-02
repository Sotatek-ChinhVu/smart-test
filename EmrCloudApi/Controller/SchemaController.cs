using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Schema;
using EmrCloudApi.Requests.Schema;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Schema.Insurance.SaveInsuranceScan;
using UseCase.Core.Sync;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.SaveImageTodayOrder;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SchemaController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SchemaController(UseCaseBus bus, IUserService userService) : base(userService)
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
            var input = new SaveImageTodayOrderInputData(HpId, request.PtId, request.RaiinNo, request.OldImage, Request.Body);
            var output = _bus.Handle(input);

            var presenter = new SaveImageTodayOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveInsuranceScanImage)]
        public ActionResult<Response<SaveImageResponse>> SaveInsuranceScanImage([FromQuery] SaveInsuranceScanRequest request)
        {
            var input = new SaveInsuranceScanInputData(HpId, request.PtId, request.HokenGrp, request.HokenId, request.OldImage , UserId, Request.Body);
            var output = _bus.Handle(input);

            var presenter = new SaveInsuranceScanPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }
    }
}
