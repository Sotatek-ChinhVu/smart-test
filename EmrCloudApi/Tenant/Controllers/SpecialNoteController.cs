using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.Read;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialNoteController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SpecialNoteController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("get-sepecial-note")]
        public ActionResult<Response<GetSpecialNoteResponse>> Save(long ptId, int sinDate)
        {
            var input = new GetSpecialNoteInputData((int)ptId, sinDate);
            var output = _bus.Handle(input);

            var presenter = new GetSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSpecialNoteResponse>>(presenter.Result);
        }
    }
}
