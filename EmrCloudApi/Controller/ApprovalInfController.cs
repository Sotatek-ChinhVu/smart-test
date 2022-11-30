﻿using Domain.Models.ApprovalInfo;
using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using EmrCloudApi.Tenant.Presenters.ApprovalInfo;
using EmrCloudApi.Tenant.Requests.ApprovalInfo;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using EmrCloudApi.Tenant.Responses.ApprovalInfo;
using Microsoft.AspNetCore.Mvc;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.ApprovalInfo.UpdateApprovalInfList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class ApprovalInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public ApprovalInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetApprovalInfListResponse>>> GetList([FromQuery] GetApprovalInfListRequest req)
        {
            var input = new GetApprovalInfListInputData(HpId, req.StartDate, req.EndDate, req.KaId, req.TantoId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetApprovalInfListResponse>>(presenter.Result);
        }
    }
}
