using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.InputItem;
using EmrCloudApi.Tenant.Requests.InputItem;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InputItem;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.InputItem.Search;
using UseCase.InputItem.UpdateAdopted;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputItemController : ControllerBase
    {
        private readonly UseCaseBus _bus;

        public InputItemController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("SearchInputItem")]
        public ActionResult<Response<SearchInputItemResponse>> SearchInputItem([FromBody] SearchInputItemRequest request)
        {
            var input = new SearchInputItemInputData(request.Keyword, request.KouiKbn, request.SinDate, request.StartIndex, request.PageCount, request.GenericOrSameItem, request.YJCd, request.HpId, request.PointFrom, request.PointTo, request.IsRosai, request.IsMirai, request.IsExpired);
            var output = _bus.Handle(input);
            var presenter = new SearchInputItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost("UpdateAdoptedInputItem")]
        public ActionResult<Response<UpdateAdoptedInputItemResponse>> UpdateAdoptedInputItem([FromBody] UpdateAdoptedInputItemRequest request)
        {
            var input = new UpdateAdoptedInputItemInputData(request.ValueAdopted, request.ItemCdInputItem, request.SinDateInputItem);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedInputItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
