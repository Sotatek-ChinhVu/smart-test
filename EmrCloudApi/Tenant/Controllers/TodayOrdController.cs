using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.GetComboList;
using UseCase.Insurance.GetDefaultSelectPattern;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodayOrdController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public TodayOrdController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpPost(ApiPath.Upsert)]
        public async Task<ActionResult<Response<UpsertTodayOdrResponse>>> Upsert([FromBody] UpsertTodayOdrRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpsertTodayOdrResponse>>(new Response<UpsertTodayOdrResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpsertTodayOdrResponse>>(new Response<UpsertTodayOdrResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new UpsertTodayOrdInputData(request.SyosaiKbn, request.JikanKbn, request.HokenPid, request.SanteiKbn, request.TantoId, request.KaId, request.UketukeTime, request.SinStartTime, request.SinEndTime, request.OdrInfs.Select(
                    o => new OdrInfItemInputData(
                            hpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            hpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList(),
                new KarteItemInputData(
                    hpId,
                    request.KarteItem.RaiinNo,
                    request.KarteItem.PtId,
                    request.KarteItem.SinDate,
                    request.KarteItem.Text,
                    request.KarteItem.IsDeleted,
                    request.KarteItem.RichText),
                userId
            );
            var output = await Task.Run(()=> _bus.Handle(input));

            var presenter = new UpsertTodayOdrPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertTodayOdrResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Validate)]
        public async Task<ActionResult<Response<ValidationTodayOrdResponse>>> Validate([FromBody] ValidationTodayOrdRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<ValidationTodayOrdResponse>>(new Response<ValidationTodayOrdResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new ValidationTodayOrdInputData(
                request.SyosaiKbn,
                request.JikanKbn,
                request.HokenPid,
                request.SanteiKbn,
                request.TantoId,
                request.KaId,
                request.UketukeTime,
                request.SinStartTime,
                request.SinEndTime,
                request.OdrInfs.Select(o =>
                    new ValidationOdrInfItem(
                        hpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.IsDeleted,
                        o.Id,
                        o.OdrDetails.Select(od => new ValidationOdrInfDetailItem(
                            hpId,
                            od.RaiinNo,
                            od.RpNo,
                            od.RpEdaNo,
                            od.RowNo,
                            od.PtId,
                            od.SinDate,
                            od.SinKouiKbn,
                            od.ItemCd,
                            od.ItemName,
                            od.Suryo,
                            od.UnitName,
                            od.UnitSbt,
                            od.TermVal,
                            od.KohatuKbn,
                            od.SyohoKbn,
                            od.SyohoLimitKbn,
                            od.DrugKbn,
                            od.YohoKbn,
                            od.Kokuji1,
                            od.Kokuji2,
                            od.IsNodspRece,
                            od.IpnCd,
                            od.IpnName,
                            od.JissiKbn,
                            od.JissiDate,
                            od.JissiId,
                            od.JissiMachine,
                            od.ReqCd,
                            od.Bunkatu,
                            od.CmtName,
                            od.CmtOpt,
                            od.FontColor,
                            od.CommentNewline
                        )).ToList()
                    )
               ).ToList(),
                new ValidationKarteItem(
                    hpId,
                    request.Karte.RaiinNo,
                    request.Karte.PtId,
                    request.Karte.SinDate,
                    request.Karte.Text,
                    request.Karte.IsDeleted,
                    request.Karte.RichText
                )
               );
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new ValidationTodayOrdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidationTodayOrdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultSelectPattern)]
        public async Task<ActionResult<Response<GetDefaultSelectPatternResponse>>> Validate([FromQuery] GetDefaultSelectPatternRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetDefaultSelectPatternResponse>>(new Response<GetDefaultSelectPatternResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetDefaultSelectPatternInputData(
                            hpId,
                            request.PtId,
                            request.SinDate,
                            request.HistoryPid,
                            request.SelectedHokenPid);

            var output = await Task.Run( () => _bus.Handle(input));

            var presenter = new GetDefaultSelectPatternPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDefaultSelectPatternResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetInsuranceComboList)]
        public async Task<ActionResult<Response<GetInsuranceComboListResponse>>> GetInsuranceComboList([FromQuery] GetInsuranceComboListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetInsuranceComboListResponse>>(new Response<GetInsuranceComboListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetInsuranceComboListInputData(hpId, request.PtId, request.SinDate);
            var output = await Task.Run(()=>_bus.Handle(input));
            var presenter = new GetInsuranceComboListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceComboListResponse>>(presenter.Result);
        }
    }
}
