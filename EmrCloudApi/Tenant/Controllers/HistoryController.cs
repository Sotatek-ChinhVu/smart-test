using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.Karte2Print;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public HistoryController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetMedicalExaminationHistoryResponse>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request)
        {
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, request.HpId, request.SinDate, request.StartPage, request.PageSize, request.DeleteCondition, request.KarteDeleteHistory, request.FilterId, request.UserId, request.IsShowApproval, request.SearchType, request.SearchCategory, request.SearchText);
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMedicalExaminationHistoryResponse>>(presenter.Result);
        }
        [HttpGet(ApiPath.Export)]
        public ActionResult<Response<Karte2ExportResponse>> Karte2Export([FromQuery] Karte2ExportRequest request)
        {
            var input = new Karte2ExportInputData(request.PtId,request.HpId,request.UserId,request.SinDate,request.RaiinNo,request.EmptyMode,request.FDisp,request.FDojitu,request.FSijiType,request.StartDate,request.EndDate,request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder, request.ChkDummy, request.Chk_Gairaikanri, request.ChkIppan, request.ChkPrtDate, request.RaiinTermDelKbn);
            var output = _bus.Handle(input);

            var presenter = new Karte2ExportPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<Karte2ExportResponse>>(presenter.Result);
        }
    }
}
