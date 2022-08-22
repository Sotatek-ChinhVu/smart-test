﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetMst;
using EmrCloudApi.Tenant.Requests.SetMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetMst.GetList;
using UseCase.SetMst.SaveSetMst;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSetMstListResponse>> GetList([FromQuery] GetSetMstListRequest request)
        {
            var input = new GetSetMstListInputData(request.HpId, request.SetKbn, request.SetKbnEdaNo, request.TextSearch, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetSetMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetMstListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public ActionResult<Response<SaveSetMstResponse>> Save([FromBody] SaveSetMstRequest request)
        {
            var input = new SaveSetMstInputData(request.SinDate, request.SetCd, request.SetKbn, request.SetKbnEdaNo, request.GenerationId, request.Level1, request.Level2, request.Level3, request.SetName, request.WeightKbn, request.Color, request.IsDeleted, request.IsGroup);
            var output = _bus.Handle(input);

            var presenter = new SaveSetMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSetMstResponse>>(presenter.Result);
        }
    }
}
