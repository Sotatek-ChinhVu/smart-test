using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ReceSeikyu;
using EmrCloudApi.Requests.ReceSeikyu;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.ReceSeikyu.GetList;
using UseCase.ReceSeikyu.Save;

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

        [HttpGet(ApiPath.GetListReceSeikyu)]
        public ActionResult<Response<GetListReceSeikyuResponse>> GetListReceSeikyu([FromQuery] GetListReceSeikyuRequest request)
        {
            var input = new GetListReceSeikyuInputData(HpId, 
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

        [HttpPost(ApiPath.SaveReceSeikyu)]
        public ActionResult<Response<SaveReceSeiKyuResponse>> SaveReceSeikyu([FromBody] SaveReceSeiKyuRequest request)
        {
            var input = new SaveReceSeiKyuInputData(request.Data, request.SinYm, HpId, UserId);
            var output = _bus.Handle(input);
            if (output.Status == SaveReceSeiKyuStatus.Successful && output.PtIds.Any() && output.SeikyuYm != 0)
            {
               
            }
            var presenter = new SaveReceSeiKyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveReceSeiKyuResponse>>(presenter.Result);
        }
    }
}
