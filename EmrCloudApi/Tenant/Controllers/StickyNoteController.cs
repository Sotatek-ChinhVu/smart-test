using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.Get;
using EmrCloudApi.Tenant.Responses.StickyNote;
using EmrCloudApi.Tenant.Requests.StickyNote;
using UseCase.StickyNote;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StickyNoteController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public StickyNoteController(UseCaseBus bus)
        {
            _bus = bus;
        }
        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetStickyNoteResponse>> Get([FromQuery] GetStickyNoteRequest request)
        {
            var input = new GetStickyNoteInputData(request.HpId, request.PtId,request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetStickyNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetStickyNoteResponse>>(presenter.Result);
        }


    }
}
