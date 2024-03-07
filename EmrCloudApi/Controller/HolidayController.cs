using Domain.Models.FlowSheet;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Holiday;
using EmrCloudApi.Requests.Holiday;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Holiday;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Holiday.SaveHoliday;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class HolidayController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;

        public HolidayController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) => _bus = bus;

        [HttpPost(ApiPath.SaveHolidayMst)]
        public ActionResult<Response<SaveHolidayMstResponse>> SaveHolidayMst([FromBody] SaveHolidayMstRequest request)
        {
            var input = new SaveHolidayMstInputData(new HolidayModel(HpId,
                                                                    request.Holiday.SeqNo,
                                                                    request.Holiday.SinDate,
                                                                    request.Holiday.HolidayKbn,
                                                                    request.Holiday.KyusinKbn,
                                                                    request.Holiday.HolidayName), UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveHolidayMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveHolidayMstResponse>>(presenter.Result);
        }
    }
}
