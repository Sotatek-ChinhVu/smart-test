﻿using EmrCloudApi.Tenant.Constants;
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
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetStickyNoteInputData(hpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Revert)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Revert([FromBody] DeleteRevertStickyNoteRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new RevertStickyNoteInputData(hpId, request.PtId, request.SeqNo, userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new RevertStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Delete)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Delete([FromBody] DeleteRevertStickyNoteRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new DeleteStickyNoteInputData(hpId, request.PtId, request.SeqNo, userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new DeleteStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.Save)]
        public async Task<ActionResult<Response<ActionStickyNoteResponse>>> Save([FromBody] SaveStickyNoteRequest request)
        {
            int userId = _userService.GetLoginUser().UserId;
            var input = new SaveStickyNoteInputData(request.stickyNoteModels.Select(x => x.Map()).ToList(), userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ActionStickyNoteResponse>>(presenter.Result);
        }
        [HttpGet(ApiPath.Get + "Setting")]
        public async Task<ActionResult<Response<GetSettingStickyNoteResponse>>> GetSetting([FromQuery] GetSettingStickyNoteRequest request)
        {
            int userId = _userService.GetLoginUser().UserId;
            var input = new GetSettingStickyNoteInputData(userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSettingStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSettingStickyNoteResponse>>(presenter.Result);
        }
    }
}
