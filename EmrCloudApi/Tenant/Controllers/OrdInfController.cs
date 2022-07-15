﻿using EmrCloudApi.Tenant.Presenters.User;
using EmrCloudApi.Tenant.Requests.OrdInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public OrdInfController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetListTree")]
        public ActionResult<Response<GetOrdInfListTreeResponse>> GetList([FromQuery] GetOrdInfListTreeRequest request)
        {
            var input = new GetOrdInfListTreeInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetOrdInfListTreePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetOrdInfListTreeResponse>>(presenter.Result);
        }
    }
}
