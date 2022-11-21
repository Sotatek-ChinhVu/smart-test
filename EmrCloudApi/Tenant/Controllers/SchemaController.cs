using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Schema;
using EmrCloudApi.Tenant.Requests.Schema;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.SaveImageTodayOrder;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchemaController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public SchemaController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetListImageTemplatesResponse>>> GetList()
        {
            var input = new GetListImageTemplatesInputData();
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetListImageTemplatesPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListImageTemplatesResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveImageTodayOrder)]
        public async Task<ActionResult<Response<SaveImageResponse>>> SaveImageTodayOrder([FromQuery] SaveImageTodayOrderRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new SaveImageTodayOrderInputData(hpId, request.PtId, request.RaiinNo, request.OldImage, Request.Body);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveImageTodayOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }
    }
}
