using EmrCloudApi.Tenant.Presenters.DrugDetail;
using EmrCloudApi.Tenant.Presenters.DrugInfor;
using EmrCloudApi.Tenant.Requests.DrugDetail;
using EmrCloudApi.Tenant.Requests.DrugInfor;
using EmrCloudApi.Tenant.Presenters.UsageTreeSet;
using EmrCloudApi.Tenant.Requests.UsageTreeSet;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugDetail;
using EmrCloudApi.Tenant.Responses.DrugInfor;
using EmrCloudApi.Tenant.Responses.UsageTreeSetResponse;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugDetail;
using UseCase.DrugInfor.Get;
using UseCase.UsageTreeSet.GetTree;
using EmrCloudApi.Tenant.Constants;
using UseCase.DrugDetailData;
using EmrCloudApi.Tenant.Presenters.DrugDetailData;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InputItemController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;

        public InputItemController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet("GetDrugInf")]
        public async Task<ActionResult<Response<GetDrugInforResponse>>> GetDrugInformation([FromQuery] GetDrugInforRequest request)
        {
            int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            var input = new GetDrugInforInputData(hpId, request.SinDate, request.ItemCd);
            var output = await Task.Run(()=>_bus.Handle(input));

            var presenter = new GetDrugInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugInforResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDrugMenuTree)]
        public async Task<ActionResult<Response<GetDrugDetailResponse>>> GetDrugMenuTree([FromQuery] GetDrugDetailRequest request)
        {
            int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            var input = new GetDrugDetailInputData(hpId, request.SinDate, request.ItemCd);
            var output = await Task.Run( () => _bus.Handle(input));

            var presenter = new GetDrugDetailPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailResponse>>(presenter.Result);
        }

        [HttpGet("GetListUsageTreeSet")]
        public async Task<ActionResult<Response<GetUsageTreeSetListResponse>>> GetUsageTree([FromQuery] GetUsageTreeSetListRequest request)
        {
            int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            var input = new GetUsageTreeSetInputData(hpId, request.SinDate, request.KouiKbn);
            var output =  await Task.Run( () => _bus.Handle(input));
            var present = new GetUsageTreeSetListPresenter();
            present.Complete(output);
            return new ActionResult<Response<GetUsageTreeSetListResponse>>(present.Result);
        }

        [HttpGet(ApiPath.DrugDataSelectedTree)]
        public async Task<ActionResult<Response<GetDrugDetailDataResponse>>> DrugDataSelectedTree([FromQuery] GetDrugDetailDataRequest request)
        {
            var input = new GetDrugDetailDataInputData(request.SelectedIndexOfMenuLevel, request.Level, request.DrugName, request.ItemCd, request.YJCode);
            var output = await Task.Run( () => _bus.Handle(input));

            var presenter = new GetDrugDetailDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailDataResponse>>(presenter.Result);
        }
    }
}
