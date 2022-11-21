using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public HistoryController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetMedicalExaminationHistoryResponse>>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, hpId, request.SinDate, request.StartPage, request.PageSize, request.DeleteCondition, request.KarteDeleteHistory, request.FilterId, userId, request.IsShowApproval, request.SearchType, request.SearchCategory, request.SearchText);
            var output = await Task.Run( () => _bus.Handle(input));

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            var result =  Ok(presenter.Result);
            watch.Stop();
            Console.WriteLine("TimeLog_History-" + request.PtId + "-" + watch.ElapsedMilliseconds);
            return result;
        }
    }
}
