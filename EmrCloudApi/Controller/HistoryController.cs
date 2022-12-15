using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class HistoryController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public HistoryController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetMedicalExaminationHistoryResponse>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, HpId, request.SinDate, request.Offset, request.Limit, request.DeleteCondition, request.KarteDeleteHistory, request.FilterId, UserId, request.IsShowApproval);
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            watch.Stop();
            Console.WriteLine("TimeLog_History-" + request.PtId + "-" + watch.ElapsedMilliseconds);
            return result;
        }
    }
}
