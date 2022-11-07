using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SpecialNote;
using EmrCloudApi.Tenant.Requests.SpecialNote;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SpecialNote;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.Save;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<Response<GetSpecialNoteResponse>>> Get([FromQuery] SpecialNoteRequest request)
        {
            var input = new GetSpecialNoteInputData(request.HpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSpecialNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AddAlrgyDrugList)]
        public async Task<ActionResult<Response<AddAlrgyDrugListResponse>>> AddAlrgyDrugList([FromBody] AddAlrgyDrugListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<AddAlrgyDrugListResponse>>(new Response<AddAlrgyDrugListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<AddAlrgyDrugListResponse>>(new Response<AddAlrgyDrugListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
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
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new AddAlrgyDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAlrgyDrugListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public async Task<ActionResult<Response<SaveSpecialNoteResponse>>> Save([FromBody] SpecialNoteSaveRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveSpecialNoteResponse>>(new Response<SaveSpecialNoteResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveSpecialNoteResponse>>(new Response<SaveSpecialNoteResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveSpecialNoteInputData(hpId, request.PtId, request.SummaryTab.Map(), request.ImportantNoteTab.Map(), request.PatientInfoTab.Map(), userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSpecialNoteResponse>>(presenter.Result);
        }
    }
}