using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.OrdInfs;
using EmrCloudApi.Tenant.Requests.OrdInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.OrdInfs;
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
    public class OrdInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public OrdInfController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public Task<ActionResult<Response<GetOrdInfListTreeResponse>>> GetList([FromQuery] GetOrdInfListTreeRequest request)
        {
            var input = new GetOrdInfListTreeInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate, request.IsDeleted, request.UserId);
            var output = _bus.Handle(input);

            var presenter = new GetOrdInfListTreePresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetOrdInfListTreeResponse>>(presenter.Result));
        }

        [HttpPost(ApiPath.ValidateInputItem)]
        public Task<ActionResult<Response<ValidationInputItemOrdInfListResponse>>> ValidateInputItem([FromBody] ValidationInputItemRequest request)
        {
            var input = new ValidationInputItemInputData(
                        request.HpId,
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

            return Task.FromResult(new ActionResult<Response<ValidationInputItemOrdInfListResponse>>(presenter.Result));
        }

        [HttpPost(ApiPath.GetMaxRpNo)]
        public Task<ActionResult<Response<GetMaxRpNoResponse>>> GetMaxRpNo([FromBody] GetMaxRpNoRequest request)
        {
            var input = new GetMaxRpNoInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxRpNoPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetMaxRpNoResponse>>(presenter.Result));
        }

        [HttpGet(ApiPath.GetHeaderInf)]
        public Task<ActionResult<Response<GetHeaderInfResponse>>> GetHeaderInf([FromQuery] GetMaxRpNoRequest request)
        {
            var input = new GetHeaderInfInputData(request.PtId, request.HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetHeaderInfPresenter();
            presenter.Complete(output);

            return Task.FromResult(new ActionResult<Response<GetHeaderInfResponse>>(presenter.Result));
        }
    }
}
