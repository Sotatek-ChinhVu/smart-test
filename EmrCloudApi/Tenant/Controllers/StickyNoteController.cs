using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Tenant.Responses.StickyNote;
using EmrCloudApi.Tenant.Requests.StickyNote;
using UseCase.StickyNote;
using EmrCloudApi.Tenant.Services;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickyNoteController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public StickyNoteController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }
        [HttpGet(ApiPath.Get)]
        public async Task<ActionResult<Response<GetStickyNoteResponse>>> Get([FromQuery] GetStickyNoteRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetStickyNoteResponse>>(new Response<GetStickyNoteResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetStickyNoteInputData(hpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Revert)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Revert([FromBody] DeleteRevertStickyNoteRequest request)
        {
            var input = new RevertStickyNoteInputData(request.HpId, request.PtId, request.SeqNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new RevertStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Delete)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Delete([FromBody] DeleteRevertStickyNoteRequest request)
        {
            var input = new DeleteStickyNoteInputData(request.HpId, request.PtId, request.SeqNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new DeleteStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Save)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Save([FromBody] SaveStickyNoteRequest request)
        {
            var input = new SaveStickyNoteInputData(request.stickyNoteModels.Select(x=> x.Map()).ToList());
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpGet(ApiPath.Get + "Setting")]
        public async Task<ActionResult<Response<GetSettingStickyNoteResponse>>> GetSetting([FromQuery] GetSettingStickyNoteRequest request)
        {
            var input = new GetSettingStickyNoteInputData(request.UserId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSettingStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSettingStickyNoteResponse>>(presenter.Result);
        }
    }
}
