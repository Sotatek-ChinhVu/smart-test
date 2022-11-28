using EmrCloudApi.Presenters.DrugDetail;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Requests.DrugDetail;
using EmrCloudApi.Requests.DrugInfor;
using EmrCloudApi.Presenters.UsageTreeSet;
using EmrCloudApi.Requests.UsageTreeSet;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugDetail;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Responses.UsageTreeSetResponse;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugDetail;
using UseCase.DrugInfor.Get;
using UseCase.UsageTreeSet.GetTree;
using EmrCloudApi.Constants;
using UseCase.DrugDetailData;
using EmrCloudApi.Presenters.DrugDetailData;
using EmrCloudApi.Services;
using EmrCloudApi.Requests.YohoSetMst;
using EmrCloudApi.Responses.YohoSetMst;
using UseCase.YohoSetMst.GetByItemCd;
using EmrCloudApi.Presenters.YohoSetMst;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class InputItemController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public InputItemController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet("GetDrugInf")]
        public ActionResult<Response<GetDrugInforResponse>> GetDrugInformation([FromQuery] GetDrugInforRequest request)
        {
            var input = new GetDrugInforInputData(HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugInforResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDrugMenuTree)]
        public ActionResult<Response<GetDrugDetailResponse>> GetDrugMenuTree([FromQuery] GetDrugDetailRequest request)
        {
            var input = new GetDrugDetailInputData(HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailResponse>>(presenter.Result);
        }

        [HttpGet("GetListUsageTreeSet")]
        public ActionResult<Response<GetUsageTreeSetListResponse>> GetUsageTree([FromQuery] GetUsageTreeSetListRequest request)
        {
            var input = new GetUsageTreeSetInputData(HpId, request.SinDate, request.KouiKbn);
            var output = _bus.Handle(input);
            var present = new GetUsageTreeSetListPresenter();
            present.Complete(output);
            return new ActionResult<Response<GetUsageTreeSetListResponse>>(present.Result);
        }

        [HttpGet(ApiPath.DrugDataSelectedTree)]
        public ActionResult<Response<GetDrugDetailDataResponse>> DrugDataSelectedTree([FromQuery] GetDrugDetailDataRequest request)
        {
            var input = new GetDrugDetailDataInputData(request.SelectedIndexOfMenuLevel, request.Level, request.DrugName, request.ItemCd, request.YJCode);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailDataResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetYohoSetMstByItemCd)]
        public ActionResult<Response<GetYohoSetMstByItemCdResponse>> GetYohoSetMstByItemCd([FromQuery] GetYohoSetMstByItemCdRequest request)
        {
            var input = new GetYohoMstByItemCdInputData(HpId, request.ItemCd, request.StartDate);
            var output = _bus.Handle(input);
            var presenter = new GetYohoMstByItemCdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetYohoSetMstByItemCdResponse>>(presenter.Result);
        }
    }
}
