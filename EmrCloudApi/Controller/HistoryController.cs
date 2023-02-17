using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryIndex;
using UseCase.MedicalExamination.SearchHistory;

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
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, HpId, request.SinDate, request.Offset, request.Limit, request.DeleteCondition, request.FilterId, UserId, request.IsShowApproval);
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet(ApiPath.Search)]
        public ActionResult<Response<SearchHistoryResponse>> Search([FromQuery] SearchHistoryRequest request)
        {
            var input = new SearchHistoryInputData(HpId, UserId, request.PtId, request.SinDate, request.CurrentIndex, request.FilterId, request.IsDeleted, request.KeyWord, request.SearchType, request.IsNext);
            var output = _bus.Handle(input);

            var presenter = new SearchHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet(ApiPath.GetHistoryIndex)]
        public ActionResult<Response<GetHistoryIndexResponse>> GetHistoryIndex([FromQuery] GetHistoryIndexRequest request)
        {
            var input = new GetHistoryIndexInputData(HpId, UserId, request.PtId, request.FilterId, request.IsDeleted, request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetHistoryIndexPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }
    }
}
