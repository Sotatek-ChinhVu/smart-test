using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.SearchHistory;

namespace EmrCloudApi.Tenant.Controllers
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
    }
}
