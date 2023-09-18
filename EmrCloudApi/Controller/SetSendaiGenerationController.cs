using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SetSendaiGeneration;
using EmrCloudApi.Requests.SetSendaiGeneration;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.GetSendaiGeneration;
using EmrCloudApi.Responses.SetSendaiGeneration;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetSendaiGeneration.Add;
using UseCase.SetSendaiGeneration.Delete;
using UseCase.SetSendaiGeneration.GetList;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SetSendaiGenerationController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetSendaiGenerationController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<SetSendaiGenerationGetListResponse>> GetList()
        {
            var input = new SetSendaiGenerationInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new SetSendaiGenerationGetListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SetSendaiGenerationGetListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Delete)]
        public ActionResult<Response<DeleteSetSendaiGenerationResponse>> Delete([FromBody]  DeleteSetSendaiGenerationRequest request)
        {
            var input = new DeleteSendaiGenerationInputData(request.GenerationId, request.RowIndex, UserId);
            var output = _bus.Handle(input);

            var presenter = new DeleteSetSendaiGenerationPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<DeleteSetSendaiGenerationResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Insert)]
        public ActionResult<Response<AddSetSendaiGenerationResponse>> AddSetSensaiGeneration([FromBody]  AddSetSendaiGenerationRequest request)
        {
            var input = new AddSetSendaiGenerationInputData(request.StartDate, HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new AddSetSendaiGenerationPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<AddSetSendaiGenerationResponse>>(presenter.Result);
        }
    }
}
