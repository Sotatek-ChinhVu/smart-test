using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.Save;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecialNoteController
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public SpecialNoteController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
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
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new AddAlrgyDrugListInputData(request.AlgrgyDrugs.Select(
                                                        a => new AddAlrgyDrugListItemInputData(
                                                                a.PtId,
                                                                a.ItemCd,
                                                                a.DrugName,
                                                                a.StartDate,
                                                                a.EndDate,
                                                                a.Cmt
                                                            )
                                                        ).ToList(),
                                                        hpId,
                                                        userId
                                                        );
            var output = _bus.Handle(input);

            var presenter = new AddAlrgyDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAlrgyDrugListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public ActionResult<Response<SaveSpecialNoteResponse>> Save([FromBody] SpecialNoteSaveRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new SaveSpecialNoteInputData(hpId, request.PtId, request.SummaryTab.Map(), request.ImportantNoteTab.Map(), request.PatientInfoTab.Map(), userId);
            var output = _bus.Handle(input);

            var presenter = new SaveSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSpecialNoteResponse>>(presenter.Result);
        }
    }
}