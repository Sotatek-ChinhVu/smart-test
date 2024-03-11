using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.GetHistoryIndex;
using UseCase.MedicalExamination.SearchHistory;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class HistoryController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public HistoryController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.GetList)]
        public ActionResult<Response<GetMedicalExaminationHistoryResponse>> GetList([FromBody] GetMedicalExaminationHistoryRequest request)
        {
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, HpId, request.SinDate, request.Offset, request.Limit, request.DeleteCondition, request.FilterId, UserId, request.IsShowApproval, request.RaiinBookmarked.Select(r => new Tuple<long, bool>(r.RaiinNo, r.Flag)).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet("GetDataPrintKarte2")]
        public ActionResult<Response<GetDataPrintKarte2Response>> GetDataPrintKarte2([FromQuery] GetDataPrintKarte2Request request)
        {
            var input = new GetDataPrintKarte2InputData(request.PtId, HpId, request.SinDate, request.StartDate, request.EndDate, request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder, request.EmptyMode);
            var output = _bus.Handle(input);

            var presenter = new GetDataPrintKarte2Presenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpPost(ApiPath.Search)]
        public ActionResult<Response<SearchHistoryResponse>> Search([FromBody] SearchHistoryRequest request)
        {
            var input = new SearchHistoryInputData(HpId, UserId, request.PtId, request.SinDate, request.CurrentIndex, request.FilterId, request.IsDeleted, request.KeyWord, request.SearchType, request.IsNext, request.RaiinBookmarked.Select(r => new Tuple<long, bool>(r.RaiinNo, r.Flag)).ToList());
            var output = _bus.Handle(input);

            var presenter = new SearchHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpPost(ApiPath.GetHistoryIndex)]
        public ActionResult<Response<GetHistoryIndexResponse>> GetHistoryIndex([FromBody] GetHistoryIndexRequest request)
        {
            var input = new GetHistoryIndexInputData(HpId, UserId, request.PtId, request.FilterId, request.IsDeleted, request.RaiinNo, request.RaiinBookmarked.Select(r => new Tuple<long, bool>(r.RaiinNo, r.Flag)).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetHistoryIndexPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }
    }
}
