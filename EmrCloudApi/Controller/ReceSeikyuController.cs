using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ReceSeikyu;
using EmrCloudApi.Requests.ReceSeikyu;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.ReceSeikyu.GetList;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceSeikyuController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public ReceSeikyuController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [AllowAnonymous]
        [HttpGet(ApiPath.GetListReceSeikyu)]
        public ActionResult<Response<GetListReceSeikyuResponse>> GetListReceSeikyu([FromQuery] GetListReceSeikyuRequest request)
        {
            var input = new GetListReceSeikyuInputData(1, 
                                                       request.SinDate,
                                                       request.SinYm,
                                                       request.IsIncludingUnConfirmed,
                                                       request.PtNumSearch,
                                                       request.NoFilter,
                                                       request.IsFilterMonthlyDelay,
                                                       request.IsFilterReturn,
                                                       request.IsFilterOnlineReturn);
            var output = _bus.Handle(input);
            var presenter = new GetListReceSeikyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListReceSeikyuResponse>>(presenter.Result);
        }
    }
}
