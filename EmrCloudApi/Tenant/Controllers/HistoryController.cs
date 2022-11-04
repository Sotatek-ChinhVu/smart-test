﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;

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
        public async Task<ActionResult<Response<GetMedicalExaminationHistoryResponse>>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, request.HpId, request.SinDate, request.StartPage, request.PageSize, request.DeleteCondition, request.KarteDeleteHistory, request.FilterId, request.UserId, request.IsShowApproval, request.SearchType, request.SearchCategory, request.SearchText);
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
