using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.AddAlrgyDrugList;
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

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetSpecialNoteResponse>> Get([FromQuery] SpecialNoteRequest request)
        {
            var input = new GetSpecialNoteInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new GetSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSpecialNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AddAlrgyDrugList)]
        public ActionResult<Response<AddAlrgyDrugListResponse>> AddAlrgyDrugList([FromBody] AddAlrgyDrugListRequest request)
        {
            var input = new AddAlrgyDrugListInputData(request.AlgrgyDrugs.Select(
                    a => new AddAlrgyDrugListItemInputData(
                            a.PtId,
                            a.SortNo,
                            a.ItemCd,
                            a.DrugName,
                            a.StartDate,
                            a.EndDate,
                            a.Cmt
                        )
                ).ToList());
            var output = _bus.Handle(input);

            var presenter = new AddAlrgyDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAlrgyDrugListResponse>>(presenter.Result);
        }
    }
}
