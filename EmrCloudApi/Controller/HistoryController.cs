﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetDataPrintKarte2;
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
        public ActionResult<Response<GetMedicalExaminationHistoryResponse>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request, List<long> raiinNos)
        {
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, HpId, request.SinDate, request.Offset, request.Limit, request.DeleteCondition, request.FilterId, UserId, request.IsShowApproval, raiinNos);
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet("GetDataPrintKarte2")]
        public ActionResult<Response<GetDataPrintKarte2Response>> GetDataPrintKarte2([FromQuery] GetDataPrintKarte2Request request)
        {
            var input = new GetDataPrintKarte2InputData(request.PtId, HpId, request.SinDate, request.StartDate, request.EndDate, request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder);
            var output = _bus.Handle(input);

            var presenter = new GetDataPrintKarte2Presenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet(ApiPath.Search)]
        public ActionResult<Response<SearchHistoryResponse>> Search([FromQuery] SearchHistoryRequest request, List<long> raiinNos)
        {
            var input = new SearchHistoryInputData(HpId, UserId, request.PtId, request.SinDate, request.CurrentIndex, request.FilterId, request.IsDeleted, request.KeyWord, request.SearchType, request.IsNext, raiinNos);
            var output = _bus.Handle(input);

            var presenter = new SearchHistoryPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }

        [HttpGet(ApiPath.GetHistoryIndex)]
        public ActionResult<Response<GetHistoryIndexResponse>> GetHistoryIndex([FromQuery] GetHistoryIndexRequest request, List<long> raiinNos)
        {
            var input = new GetHistoryIndexInputData(HpId, UserId, request.PtId, request.FilterId, request.IsDeleted, request.RaiinNo, raiinNos);
            var output = _bus.Handle(input);

            var presenter = new GetHistoryIndexPresenter();
            presenter.Complete(output);

            var result = Ok(presenter.Result);
            return result;
        }
    }
}
