﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.Get;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialNoteController
    {
        private readonly UseCaseBus _bus;
        public SpecialNoteController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSpecialNoteResponse>> GetList([FromQuery] SpecialNoteRequest request)
        {
            var input = new GetSpecialNoteInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new GetSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSpecialNoteResponse>>(presenter.Result);
        }
    }
}
