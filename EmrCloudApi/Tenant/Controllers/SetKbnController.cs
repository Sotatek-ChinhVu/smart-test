using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetKbnMst;
using EmrCloudApi.Tenant.Requests.SetKbnMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbnMst;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbnMst.GetList;

namespace EmrCloudApi.Tenant.Controllers
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
    }
}
