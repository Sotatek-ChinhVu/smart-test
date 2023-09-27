using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MstItem;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MstItem.GetListResultKensaMst;
using UseCase.KensaHistory.UpdateKensaSet;
using EmrCloudApi.Presenters.KensaHistory;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Requests.KensaHistory;

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
        public ActionResult<Response<UpdateKensaSetResponse>> UpdateKensaSet([FromQuery] UpdateKensaSetRequest request)
        {
            var input = new UpdateKensaSetInputData(HpId, UserId, request.SetId, request.SetName, request.SortNo, request.IsDeleted, request.KensaSetDetails);
            var output = _bus.Handle(input);
            var presenter = new UpdateKensaSetPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateKensaSetResponse>>(presenter.Result);
        }
    }
}
