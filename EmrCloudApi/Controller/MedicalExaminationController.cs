﻿using Domain.Models.Diseases;
using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Diseases.Upsert;
using UseCase.MedicalExamination.GetCheckDisease;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.MedicalExamination.InitKbnSetting;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.CheckedSpecialItem;

namespace EmrCloudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalExaminationController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public MedicalExaminationController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.GetCheckDiseases)]
        public ActionResult<Response<GetCheckDiseaseResponse>> GetCheckDiseases([FromBody] GetCheckDiseaseRequest request)
        {
            var input = new GetCheckDiseaseInputData(HpId, request.SinDate, request.TodayByomeis.Select(b => new PtDiseaseModel(
                    HpId,
                    b.PtId,
                    b.SeqNo,
                    b.ByomeiCd,
                    b.SortNo,
                    b.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                    b.Byomei,
                    b.StartDate,
                    b.TenkiKbn,
                    b.TenkiDate,
                    b.SyubyoKbn,
                    b.SikkanKbn,
                    b.NanByoCd,
                    b.IsNodspRece,
                    b.IsNodspKarte,
                    b.IsDeleted,
                    b.Id,
                    b.IsImportant,
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    b.HokenPid,
                    b.HosokuCmt,
                    0
                )).ToList(), request.TodayOdrs.Select(
                    o => new OdrInfItemInputData(
                            HpId,
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
                                            HpId,
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
                ).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetCheckDiseasePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetCheckDiseaseResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetInfCheckedSpecialItem)]
        public ActionResult<Response<CheckedSpecialItemResponse>> GetInfCheckedSpecialItem([FromBody] CheckedSpecialItemRequest request)
        {
            var input = new CheckedSpecialItemInputData(HpId, UserId, request.PtId, request.SinDate, request.IBirthDay, request.CheckAge, request.RaiinNo, request.OdrInfs.Select(
                o => new OdrInfItemInputData(
                            HpId,
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
                                            HpId,
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
                request.CheckedOrderItems.Select(
                    c => new CheckedSpecialItemOrderItem(
                            c.CheckingType,
                            c.Santei,
                            c.CheckingContent,
                            c.ItemCd,
                            c.SinKouiKbn,
                            c.ItemName,
                            c.InOutKbn
                        )
                ).ToList(),
                new KarteItemInputData(
                    request.KarteInf.HpId,
                    request.KarteInf.RaiinNo,
                    request.KarteInf.PtId,
                    request.KarteInf.SinDate,
                    request.KarteInf.Text,
                    request.KarteInf.IsDeleted,
                    request.KarteInf.RichText),
                request.EnabledInputCheck,
                request.EnabledCommentCheck
                );
            var output = _bus.Handle(input);

            var presenter = new CheckedSpecialItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckedSpecialItemResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.InitKbnSetting)]
        public ActionResult<Response<InitKbnSettingResponse>> InitKbnSetting([FromBody] InitKbnSettingRequest request)
        {
            var input = new InitKbnSettingInputData(HpId, request.WindowType, request.FrameId, request.IsEnableLoadDefaultVal, request.PtId, request.RaiinNo, request.SinDate, request.OdrInfs.Select(
                o => new OdrInfItemInputData(
                            HpId,
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
                                            HpId,
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
                ).ToList()
                );
            var output = _bus.Handle(input);

            var presenter = new InitKbnSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<InitKbnSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetCheckedOrder)]
        public ActionResult<Response<GetCheckedOrderResponse>> GetCheckedOrder([FromBody] GetCheckedOrderRequest request)
        {
            var input = new GetCheckedOrderInputData(HpId, UserId, request.SinDate, request.HokenId, request.PtId, request.IBirthDay, request.RaiinNo, request.SyosaisinKbn, request.OyaRaiinNo, request.PrimaryDoctor, request.TantoId, request.OdrInfItemInputDatas.Select(
                o => new OdrInfItemInputData(
                            HpId,
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
                                            HpId,
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
                request.PtDiseaseListInputItems.Select(
                    d => new UpsertPtDiseaseListInputItem(
                                                            d.Id,
                                                            d.PtId,
                                                            d.SortNo,
                                                            d.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            d.SuffixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            d.Byomei,
                                                            d.StartDate,
                                                            d.TenkiKbn,
                                                            d.TenkiDate,
                                                            d.SyubyoKbn,
                                                            d.SikkanKbn,
                                                            d.NanByoCd,
                                                            d.HosokuCmt,
                                                            d.HokenPid,
                                                            d.IsNodspRece,
                                                            d.IsNodspKarte,
                                                            d.SeqNo,
                                                            d.IsImportant,
                                                            d.IsDeleted,
                                                            d.ByomeiCd,
                                                            HpId
                                                        )).ToList()
                );
            var output = _bus.Handle(input);

            var presenter = new GetCheckedOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetCheckedOrderResponse>>(presenter.Result);
        }
    }
}
