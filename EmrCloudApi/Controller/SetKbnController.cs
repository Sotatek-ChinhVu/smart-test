using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SetKbnMst;
using EmrCloudApi.Requests.SetKbnMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetKbnMst;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbnMst.GetList;
using UseCase.SetKbnMst.GetSetKbnMstListByGenerationId;
using UseCase.SetKbnMst.Upsert;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SetKbnController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetKbnController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSetKbnMstListResponse>> GetList([FromQuery] GetSetKbnMstListRequest request)
        {
            var input = new GetSetKbnMstListInputData(HpId, request.SinDate, request.SetKbnFrom, request.SetKbnTo);
            var output = _bus.Handle(input);

            var presenter = new GetSetKbnMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnMstListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSetKbnMstListByGenerationId)]
        public ActionResult<Response<GetSetKbnMstListByGenerationIdResponse>> GetSetKbnMstListByGenerationId([FromQuery] GetSetKbnMstListByGenerationIdRequest request)
        {
            var input = new GetSetKbnMstListByGenerationIdInputData(HpId, request.GenerationId);
            var output = _bus.Handle(input);

            var presenter = new GetSetKbnMstListByGenerationIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnMstListByGenerationIdResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertSetKbnMstResponse>> Upsert([FromBody] UpsertSetKbnMstRequest request)
        {
            var input = new UpsertSetKbnMstInputData(request.SinDate, UserId, request.SetKbnMstItems);
            var output = _bus.Handle(input);

            var presenter = new UpsertSetKbnMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertSetKbnMstResponse>>(presenter.Result);
        }
    }
}
