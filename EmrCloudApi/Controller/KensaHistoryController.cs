﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.KensaHistory;
using EmrCloudApi.Requests.KensaHistory;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KensaHistory.GetListKensaCmtMst;
using UseCase.KensaHistory.GetListKensaSet;
using UseCase.KensaHistory.GetListKensaSetDetail;
using UseCase.KensaHistory.UpdateKensaInfDetail;
using UseCase.KensaHistory.UpdateKensaSet;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class KensaHistoryController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public KensaHistoryController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.UpdateKensaSet)]
        public ActionResult<Response<UpdateKensaSetResponse>> UpdateKensaSet([FromBody] UpdateKensaSetRequest request)
        {
            var input = new UpdateKensaSetInputData(HpId, UserId, request.SetId, request.SetName, request.SortNo, request.IsDeleted, request.KensaSetDetails);
            var output = _bus.Handle(input);
            var presenter = new UpdateKensaSetPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateKensaSetResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListKensaSet)]
        public ActionResult<Response<GetListKensaSetResponse>> GetListKensaSet()
        {
            var input = new GetListKensaSetInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetListKensaSetPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListKensaSetResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListKensaSetDetail)]
        public ActionResult<Response<GetListKensaSetDetailResponse>> GetListKensaSetDetail([FromQuery] GetListKensaSetDetailRequest request)
        {
            var input = new GetListKensaSetDetailInputData(HpId, request.SetId);
            var output = _bus.Handle(input);
            var presenter = new GetListKensaSetDetailPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListKensaSetDetailResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListKensaCmtMst)]
        public ActionResult<Response<GetListKensaCmtMstResponse>> GetListKensaCmtMst([FromQuery] GetListKensaCmtMstRequest request)
        {
            var input = new GetListKensaCmtMstInputData(HpId, request.Keyword);
            var output = _bus.Handle(input);
            var presenter = new GetListKensaCmtMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListKensaCmtMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateKensaInfDetail)]
        public ActionResult<Response<UpdateKensaInfDetailResponse>> UpdateKensaInfDetail([FromBody] UpdateKensaInfDetailRequest request)
        {
            var input = new UpdateKensaInfDetailInputData(HpId, UserId, request.kensaInfDetails);
            var output = _bus.Handle(input);
            var presenter = new UpdateKensaInfDetailPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateKensaInfDetailResponse>>(presenter.Result);
        }
    }
}
