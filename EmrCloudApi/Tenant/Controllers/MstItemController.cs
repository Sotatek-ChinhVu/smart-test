using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MstItem;
using EmrCloudApi.Tenant.Requests.MstItem;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.SearchOTC;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MstItemController : ControllerBase
    {
        private readonly UseCaseBus _bus;

        public MstItemController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.GetDosageDrugList)]
        public ActionResult<Response<GetDosageDrugListResponse>> GetDosageDrugList([FromBody] GetDosageDrugListRequest request)
        {
            var input = new GetDosageDrugListInputData(request.YjCds);
            var output = _bus.Handle(input);

            var presenter = new GetDosageDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDosageDrugListResponse>>(presenter.Result);
        }
        [HttpPost(ApiPath.SearchOTC)]
        public ActionResult<Response<SearchOTCResponse>> SearchOTC([FromBody] SearchOTCRequest request)
        {
            var input = new SearchOTCInputData(request.SearchValue,request.PageIndex,request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchOTCPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchOTCResponse>>(presenter.Result);
        }
    }
}
