using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Presenters.OrdInfs;
using EmrCloudApi.Requests.OrdInfs;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Responses.OrdInfs;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.ConvertInputItemToTodayOdr;
using UseCase.OrdInfs.GetHeaderInf;
using UseCase.OrdInfs.GetListTrees;
using UseCase.OrdInfs.GetMaxRpNo;
using UseCase.OrdInfs.ValidationInputItem;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class OrdInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public OrdInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetOrdInfListTreeResponse>> GetList([FromQuery] GetOrdInfListTreeRequest request)
        {
            var input = new GetOrdInfListTreeInputData(request.PtId, HpId, request.RaiinNo, request.SinDate, request.IsDeleted, UserId);
            var output = _bus.Handle(input);

            var presenter = new GetOrdInfListTreePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetOrdInfListTreeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateInputItem)]
        public ActionResult<Response<ValidationInputItemOrdInfListResponse>> ValidateInputItem([FromBody] ValidationInputItemRequest request)
        {
            var input = new ValidationInputItemInputData(
                        HpId,
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
                                )).ToList(),
                                o.IsAutoAddItem
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
            var input = new GetMaxRpNoInputData(request.PtId, HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxRpNoPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMaxRpNoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHeaderInf)]
        public ActionResult<Response<GetHeaderInfResponse>> GetHeaderInf([FromQuery] GetMaxRpNoRequest request)
        {
            var input = new GetHeaderInfInputData(request.PtId, HpId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetHeaderInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHeaderInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertInputItemToTodayOrder)]
        public ActionResult<Response<ConvertInputItemToTodayOrdResponse>> ConvertInputItemToTodayOrder([FromBody] ConvertInputItemToTodayOrdInputData request)
        {
            var input = new ConvertInputItemToTodayOrdInputData(HpId, request.SinDate, request.DetailInfs);
            var output = _bus.Handle(input);

            var presenter = new ConvertInputItemToTodayOrdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ConvertInputItemToTodayOrdResponse>>(presenter.Result);
        }
    }
}
