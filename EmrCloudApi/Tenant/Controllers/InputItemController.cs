﻿using EmrCloudApi.Tenant.Presenters.DrugDetail;
using EmrCloudApi.Tenant.Presenters.DrugInfor;
using EmrCloudApi.Tenant.Requests.DrugDetail;
using EmrCloudApi.Tenant.Requests.DrugInfor;
using EmrCloudApi.Tenant.Presenters.UsageTreeSet;
using EmrCloudApi.Tenant.Requests.UsageTreeSet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugDetail;
using EmrCloudApi.Tenant.Responses.DrugInfor;
using EmrCloudApi.Tenant.Responses.UsageTreeSetResponse;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugDetail;
using UseCase.DrugInfor.Get;
using UseCase.UsageTreeSet.GetTree;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputItemController : ControllerBase
    {
        private readonly UseCaseBus _bus;

        public InputItemController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetDrugInf")]
        public ActionResult<Response<GetDrugInforResponse>> GetDrugInformation([FromQuery] GetDrugInforRequest request)
        {
            var input = new GetDrugInforInputData(request.HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugInforResponse>>(presenter.Result);
        }

        [HttpGet("GetMenuDataAndDetail")]
        public ActionResult<Response<GetDrugDetailResponse>> GetMenuDataAndDetail([FromQuery] GetDrugDetailRequest request)
        {
            var input = new GetDrugDetailInputData(request.HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailResponse>>(presenter.Result);
        }

        [HttpGet("GetListUsageTreeSet")]
        public ActionResult<Response<GetUsageTreeSetListResponse>> GetUsageTree([FromQuery] GetUsageTreeSetListRequest request)
        {
            var input = new GetUsageTreeSetInputData(request.HpId, request.SinDate, request.SetUsageKbn);
            var output = _bus.Handle(input);
            var present = new GetUsageTreeSetListPresenter();
            present.Complete(output);
            return new ActionResult<Response<GetUsageTreeSetListResponse>>(present.Result);
        }
    }
}
