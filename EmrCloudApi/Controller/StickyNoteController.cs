using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.StickyNote;
using EmrCloudApi.Requests.StickyNote;
using UseCase.StickyNote;
using EmrCloudApi.Services;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class StickyNoteController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public StickyNoteController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetStickyNoteResponse>> Get([FromQuery] GetStickyNoteRequest request)
        {
            var input = new GetStickyNoteInputData(HpId, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new GetStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetStickyNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Revert)]
        public ActionResult<Response<ActionStickyNoteResponse>> Revert([FromBody] DeleteRevertStickyNoteRequest request)
        {
            var input = new RevertStickyNoteInputData(HpId, request.PtId, request.SeqNo, UserId);
            var output = _bus.Handle(input);

            var presenter = new RevertStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Delete)]
        public ActionResult<Response<ActionStickyNoteResponse>> Delete([FromBody] DeleteRevertStickyNoteRequest request)
        {
            var input = new DeleteStickyNoteInputData(HpId, request.PtId, request.SeqNo, UserId);
            var output = _bus.Handle(input);

            var presenter = new DeleteStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public ActionResult<Response<ActionStickyNoteResponse>> Save([FromBody] SaveStickyNoteRequest request)
        {
            var input = new SaveStickyNoteInputData(request.stickyNoteModels.Select(x => x.Map()).ToList(), UserId);
            var output = _bus.Handle(input);

            var presenter = new SaveStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.Get + "Setting")]
        public ActionResult<Response<GetSettingStickyNoteResponse>> GetSetting([FromQuery] GetSettingStickyNoteRequest request)
        {
            var input = new GetSettingStickyNoteInputData(UserId);
            var output = _bus.Handle(input);

            var presenter = new GetSettingStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSettingStickyNoteResponse>>(presenter.Result);
        }
    }
}
