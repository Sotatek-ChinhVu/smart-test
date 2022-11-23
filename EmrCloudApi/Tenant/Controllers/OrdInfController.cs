using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.OrdInfs;
using EmrCloudApi.Tenant.Requests.OrdInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.OrdInfs.GetHeaderInf;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;
using UseCase.OrdInfs.ValidationInputItem;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public OrdInfController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetOrdInfListTreeResponse>> GetList([FromQuery] GetOrdInfListTreeRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new GetOrdInfListTreeInputData(request.PtId, hpId, request.RaiinNo, request.SinDate, request.IsDeleted, userId);
            var output = _bus.Handle(input);

            var presenter = new GetOrdInfListTreePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetOrdInfListTreeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateInputItem)]
        public ActionResult<Response<ValidationInputItemOrdInfListResponse>> ValidateInputItem([FromBody] ValidationInputItemRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new ValidationInputItemInputData(
                        hpId,
                        request.SinDate,
                        request.OdrInfs.Select(o =>
                            new ValidationInputItemItem(
                                o.OdrKouiKbn,
                                o.InoutKbn,
                                o.DaysCnt,
                                o.OdrDetails.Select(od => new ValidationInputItemDetailItem(
                                    od.RowNo,
                                    od.SinKouiKbn,
                                    od.ItemCd,
                                    od.ItemName,
                                    od.Suryo,
                                    od.UnitName,
                                    od.KohatuKbn,
                                    od.SyohoKbn,
                                    od.DrugKbn,
                                    od.YohoKbn,
                                    od.Bunkatu,
                                    od.CmtName,
                                    od.CmtOpt
                                )).ToList()
                            )
                    ).ToList());
            var output = _bus.Handle(input);

            var presenter = new ValidationInputItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidationInputItemOrdInfListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetMaxRpNo)]
        public ActionResult<Response<GetMaxRpNoResponse>> GetMaxRpNo([FromBody] GetMaxRpNoRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetMaxRpNoInputData(request.PtId, hpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxRpNoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMaxRpNoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHeaderInf)]
        public ActionResult<Response<GetHeaderInfResponse>> GetHeaderInf([FromQuery] GetMaxRpNoRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetHeaderInfInputData(request.PtId, hpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetHeaderInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHeaderInfResponse>>(presenter.Result);
        }
    }
}
