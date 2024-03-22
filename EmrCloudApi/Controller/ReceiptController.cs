using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Presenters.Reception;
using EmrCloudApi.Presenters.SinKoui;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Requests.Receipt.RequestItem;
using EmrCloudApi.Requests.Reception;
using EmrCloudApi.Requests.SinKoui;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Responses.Reception;
using EmrCloudApi.Responses.SinKoui;
using Helper.Extension;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using UseCase.Core.Sync;
using UseCase.Receipt;
using UseCase.Receipt.CheckExisReceInfEdit;
using UseCase.Receipt.CheckExistsReceInf;
using UseCase.Receipt.CheckExistSyobyoKeika;
using UseCase.Receipt.CreateUKEFile;
using UseCase.Receipt.DoReceCmt;
using UseCase.Receipt.GetDiseaseReceList;
using UseCase.Receipt.GetInsuranceReceInfList;
using UseCase.Receipt.GetListKaikeiInf;
using UseCase.Receipt.GetListRaiinInf;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.GetListSokatuMst;
using UseCase.Receipt.GetListSyobyoKeika;
using UseCase.Receipt.GetListSyoukiInf;
using UseCase.Receipt.GetReceByomeiChecking;
using UseCase.Receipt.GetReceCheckOptionList;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.GetReceHenReason;
using UseCase.Receipt.GetReceiCheckList;
using UseCase.Receipt.GetRecePreviewList;
using UseCase.Receipt.GetReceStatus;
using UseCase.Receipt.GetSinDateRaiinInfList;
using UseCase.Receipt.GetSinMeiInMonthList;
using UseCase.Receipt.MedicalDetail;
using UseCase.Receipt.ReceCmtHistory;
using UseCase.Receipt.ReceiptEdit;
using UseCase.Receipt.ReceiptListAdvancedSearch;
using UseCase.Receipt.SaveListReceCmt;
using UseCase.Receipt.SaveListSyobyoKeika;
using UseCase.Receipt.SaveListSyoukiInf;
using UseCase.Receipt.SaveReceCheckCmtList;
using UseCase.Receipt.SaveReceCheckOpt;
using UseCase.Receipt.SaveReceiptEdit;
using UseCase.Receipt.SaveReceStatus;
using UseCase.Receipt.SyobyoKeikaHistory;
using UseCase.Receipt.SyoukiInfHistory;
using UseCase.Receipt.ValidateCreateUKEFile;
using UseCase.Reception.GetNextUketukeNoBySetting;
using UseCase.SinKoui.GetSinKoui;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ReceiptController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;
    public ReceiptController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.GetList)]
    public ActionResult<Response<ReceiptListAdvancedSearchResponse>> GetList([FromBody] ReceiptListAdvancedSearchRequest request)
    {
        var input = ConvertToReceiptListAdvancedSearchInputData(HpId, request);
        var output = _bus.Handle(input);

        var presenter = new ReceiptListAdvancedSearchPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReceiptListAdvancedSearchResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceCmtList)]
    public ActionResult<Response<GetReceCmtListResponse>> GetListReceCmt([FromQuery] GetReceCmtListRequest request)
    {
        var input = new GetReceCmtListInputData(HpId, request.SinYm, request.PtId, request.HokenId, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetReceCmtListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceCmtListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceCmtList)]
    public ActionResult<Response<SaveReceCmtListResponse>> SaveListReceCmt([FromBody] SaveReceCmtListRequest request)
    {
        var listReceCmtItem = request.ReceCmtList.Select(item => ConvertToReceCmtItem(item)).ToList();
        var input = new SaveReceCmtListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceCmtItem);
        var output = _bus.Handle(input);

        var presenter = new SaveReceCmtListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceCmtListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSyoukiInfList)]
    public ActionResult<Response<GetSyoukiInfListResponse>> GetListSyoukiInf([FromQuery] GetSyoukiInfListRequest request)
    {
        var input = new GetSyoukiInfListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetSyoukiInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSyoukiInfListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSyobyoKeikaList)]
    public ActionResult<Response<GetSyobyoKeikaListResponse>> GetListSyobyoKeika([FromQuery] GetSyobyoKeikaListRequest request)
    {
        var input = new GetSyobyoKeikaListInputData(HpId, request.SinYm, request.PtId, request.HokenId, request.HokenKbn);
        var output = _bus.Handle(input);

        var presenter = new GetSyobyoKeikaListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSyobyoKeikaListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSyoukiInfList)]
    public ActionResult<Response<SaveSyoukiInfListResponse>> SaveListSyoukiInf([FromBody] SaveSyoukiInfListRequest request)
    {
        var listReceSyoukiInf = request.SyoukiInfList.Select(item => ConvertToSyoukiInfItem(item)).ToList();
        var input = new SaveSyoukiInfListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceSyoukiInf);
        var output = _bus.Handle(input);

        var presenter = new SaveSyoukiInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSyoukiInfListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSyobyoKeikaList)]
    public ActionResult<Response<SaveSyobyoKeikaListResponse>> SaveListSyobyoKeika([FromBody] SaveSyobyoKeikaListRequest request)
    {
        var listReceSyoukiInf = request.SyobyoKeikaList.Select(item => ConvertToSyobyoKeikaItem(item)).ToList();
        var input = new SaveSyobyoKeikaListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceSyoukiInf);
        var output = _bus.Handle(input);

        var presenter = new SaveSyobyoKeikaListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSyobyoKeikaListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceHenReason)]
    public ActionResult<Response<GetReceHenReasonResponse>> GetReceHenReason([FromQuery] GetReceHenReasonRequest request)
    {
        var input = new GetReceHenReasonInputData(HpId, request.SeikyuYm, request.SinDate, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceHenReasonPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceHenReasonResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceiCheckList)]
    public ActionResult<Response<GetReceiCheckListResponse>> GetReceiCheckList([FromQuery] GetReceiCheckListRequest request)
    {
        var input = new GetReceiCheckListInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceiCheckListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceiCheckListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceCheckCmtList)]
    public ActionResult<Response<SaveReceCheckCmtListResponse>> SaveReceCheckCmtList([FromBody] SaveReceCheckCmtListRequest request)
    {
        var receCheckCmtList = request.ReceCheckCmtList.Select(item => ConvertToReceCheckCmtItem(item)).ToList();
        var receCheckErrorList = request.ReceCheckErrorList.Select(item => ConvertToReceCheckErrorItem(item)).ToList();
        var input = new SaveReceCheckCmtListInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, receCheckCmtList, receCheckErrorList);
        var output = _bus.Handle(input);

        var presenter = new SaveReceCheckCmtListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceCheckCmtListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetInsuranceReceInfList)]
    public ActionResult<Response<GetInsuranceReceInfListResponse>> GetInsuranceReceInfList([FromQuery] GetInsuranceReceInfListRequest request)
    {
        var input = new GetInsuranceReceInfListInputData(HpId, request.SeikyuYm, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetInsuranceReceInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetInsuranceReceInfListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetDiseaseReceList)]
    public ActionResult<Response<GetDiseaseReceListResponse>> GetDiseaseReceList([FromQuery] GetDiseaseReceListRequest request)
    {
        var input = new GetDiseaseReceListInputData(HpId, UserId, request.PtId, request.HokenId, request.SinYm);
        var output = _bus.Handle(input);

        var presenter = new GetDiseaseReceListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetDiseaseReceListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceCheckOptionList)]
    public ActionResult<Response<GetReceCheckOptionListResponse>> GetReceCheckOptionList()
    {
        var input = new GetReceCheckOptionListInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetReceCheckOptionListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceCheckOptionListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceCheckOpt)]
    public ActionResult<Response<SaveReceCheckOptResponse>> SaveReceCheckOpt([FromBody] SaveReceCheckOptRequest request)
    {
        var receCheckOptList = request.ReceCheckOptList.Select(item => new ReceCheckOptItem(
                                                                           item.ErrCd,
                                                                           item.CheckOpt,
                                                                           string.Empty,
                                                                           item.IsInvalid))
                                                       .ToList();

        var input = new SaveReceCheckOptInputData(HpId, UserId, receCheckOptList);
        var output = _bus.Handle(input);

        var presenter = new SaveReceCheckOptPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceCheckOptResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.ReceCmtHistory)]
    public ActionResult<Response<ReceCmtHistoryResponse>> ReceCmtHistory([FromQuery] ReceCmtHistoryRequest request)
    {
        var input = new ReceCmtHistoryInputData(HpId, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new ReceCmtHistoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReceCmtHistoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetInsuranceInf)]
    public ActionResult<Response<GetInsuranceInfResponse>> GetInsuranceInf([FromQuery] GetInsuranceInfRequest request)
    {
        var input = new GetInsuranceInfInputData(HpId, request.PtId, request.SinYm);
        var output = _bus.Handle(input);

        var presenter = new GetInsuranceInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetInsuranceInfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.SyoukiInfHistory)]
    public ActionResult<Response<SyoukiInfHistoryResponse>> SyoukiInfHistory([FromQuery] SyoukiInfHistoryRequest request)
    {
        var input = new SyoukiInfHistoryInputData(HpId, request.SinYm, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new SyoukiInfHistoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SyoukiInfHistoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.SyobyoKeikaHistory)]
    public ActionResult<Response<SyobyoKeikaHistoryResponse>> SyobyoKeikaHistory([FromQuery] SyobyoKeikaHistoryRequest request)
    {
        var input = new SyobyoKeikaHistoryInputData(HpId, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new SyobyoKeikaHistoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SyobyoKeikaHistoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetMedicalDetails)]
    public ActionResult<Response<GetMedicalDetailsResponse>> GetMedicalDetails([FromQuery] GetMedicalDetailsRequest request)
    {
        var input = new GetMedicalDetailsInputData(HpId, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetMedicalDetailsPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetMedicalDetailsResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.DoReceCmt)]
    public ActionResult<Response<GetReceCmtListResponse>> DoReceCmt([FromQuery] DoReceCmtRequest request)
    {
        var input = new DoReceCmtInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new DoReceCmtPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceCmtListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetRecePreviewList)]
    public ActionResult<Response<GetRecePreviewListResponse>> GetRecePreviewList([FromQuery] GetRecePreviewListRequest request)
    {
        var input = new GetRecePreviewListInputData(HpId, request.ReceiptPreviewType, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetRecePreviewListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetRecePreviewListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceiptEdit)]
    public ActionResult<Response<GetReceiptEditResponse>> GetReceiptEdit([FromQuery] GetReceiptEditRequest request)
    {
        var input = new GetReceiptEditInputData(HpId, request.SeikyuYm, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceiptEditPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceiptEditResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSinDateRaiinInfList)]
    public ActionResult<Response<GetSinDateRaiinInfListResponse>> GetSinDateRaiinInfList([FromQuery] GetSinDateRaiinInfListRequest request)
    {
        var input = new GetSinDateRaiinInfListInputData(HpId, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetSinDateRaiinInfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSinDateRaiinInfListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceByomeiChecking)]
    public ActionResult<Response<GetReceByomeiCheckingResponse>> GetReceByomeiChecking([FromQuery] GetReceByomeiCheckingRequest request)
    {
        var input = new GetReceByomeiCheckingInputData(HpId, request.PtId, request.SinDate, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceByomeiCheckingPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceByomeiCheckingResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSinMeiInMonthList)]
    public ActionResult<Response<GetMedicalDetailsResponse>> GetSinMeiInMonthList([FromQuery] GetSinMeiInMonthListRequest request)
    {
        var input = new GetSinMeiInMonthListInputData(HpId, request.PtId, request.SinYm, request.HokenId, request.SeikyuYm);
        var output = _bus.Handle(input);

        var presenter = new GetSinMeiInMonthListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetMedicalDetailsResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetReceStatus)]
    public ActionResult<Response<GetReceStatusResponse>> GetReceStatus([FromQuery] GetReceStatusRequest request)
    {
        var input = new GetReceStatusInputData(HpId, request.SeikyuYm, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetReceStatusPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetReceStatusResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceiptEdit)]
    public ActionResult<Response<SaveReceiptEditResponse>> SaveReceiptEdit([FromBody] SaveReceiptEditRequest request)
    {
        var input = new SaveReceiptEditInputData(HpId, UserId, request.SeikyuYm, request.PtId, request.SinYm, request.HokenId, ConvertToReceiptEditItem(request));
        var output = _bus.Handle(input);

        var presenter = new SaveReceiptEditPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceiptEditResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveReceStatus)]
    public ActionResult<Response<SaveReceStatusResponse>> SaveReceStatus([FromBody] SaveReceStatusRequest request)
    {
        var input = new SaveReceStatusInputData(HpId, UserId, new ReceStatusItem(
                                                                  request.PtId,
                                                                  request.SeikyuYm,
                                                                  request.HokenId,
                                                                  request.SinYm,
                                                                  request.FusenKbn,
                                                                  request.IsPaperRece,
                                                                  false,
                                                                  request.StatusKbn,
                                                                  request.IsPrechecked));

        var output = _bus.Handle(input);

        var presenter = new SaveReceStatusPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveReceStatusResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.CreateUKEFile)]
    public ActionResult<Response<CreateUKEFileResponse>> CreateUKEFile([FromBody] CreateUKEFileRequest request)
    {
        var input = new CreateUKEFileInputData(HpId,
                                                request.ModeType,
                                                request.SeikyuYm,
                                                request.SeikyuYmOutput,
                                                request.ChkHenreisai,
                                                request.ChkTogetsu,
                                                request.IncludeOutDrug,
                                                request.IncludeTester,
                                                request.KaId,
                                                request.DoctorId,
                                                request.Sort,
                                                UserId);
        var output = _bus.Handle(input);
        var presenter = new CreateUKEFilePresenter();
        presenter.Complete(output);
        if (output.Status == CreateUKEFileStatus.Successful)
        {
            var result = new List<FileContentResult>();
            foreach (var file in output.UKEFiles)
            {
                result.Add(File(file.OutputStream.ToArray(), MediaTypeNames.Application.Octet, file.FileName));
            }
            presenter.Result.Data.File = result;

            return Ok(presenter.Result);
        }
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.ValidateCreateUKEFile)]
    public ActionResult<Response<ValidateCreateUKEFileResponse>> ValidateCreateUKEFile([FromQuery] ValidateCreateUKEFileRequest request)
    {
        var input = new ValidateCreateUKEFileInputData(HpId, request.SeikyuYm, request.ModeType);
        var output = _bus.Handle(input);
        var presenter = new ValidateCreateUKEFilePresenter();
        presenter.Complete(output);
        return new ActionResult<Response<ValidateCreateUKEFileResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListSinKoui)]
    public ActionResult<Response<GetListSinKouiResponse>> GetListSinKoui([FromQuery] GetListSinKouiRequest req)
    {
        var input = new GetListSinKouiInputData(HpId, req.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetListSinKouiPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListSinKouiResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListKaikeiInf)]
    public ActionResult<Response<GetListKaikeiInfResponse>> GetListKaikeiInf([FromQuery] GetListKaikeiInfRequest request)
    {
        var input = new GetListKaikeiInfInputData(HpId, request.SinYm, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetListKaikeiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListKaikeiInfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListRaiinInf)]
    public ActionResult<Response<GetListRaiinInfResponse>> GetListRaiinInf([FromQuery] GetListRaiinInfRequest request)
    {
        var input = new GetListRaiinInfInputData(HpId, request.PtId, request.SinYm, request.DayInMonth, request.RpNo, request.SeqNo);
        var output = _bus.Handle(input);

        var presenter = new GetListRaiinInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListRaiinInfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.CheckExisReceInfEdit)]
    public ActionResult<Response<CheckExisReceInfEditResponse>> CheckExisReceInfEdit([FromQuery] CheckExisReceInfEditRequest request)
    {
        var input = new CheckExisReceInfEditInputData(HpId, request.SeikyuYm, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new CheckExisReceInfEditPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckExisReceInfEditResponse>>(presenter.Result);
    }


    [HttpGet(ApiPath.GetListSokatuMst)]
    public ActionResult<Response<GetListSokatuMstResponse>> GetSokatuMstModels([FromQuery] GetListSokatuMstRequest req)
    {
        var input = new GetListSokatuMstInputData(HpId, req.SeikyuYm);
        var output = _bus.Handle(input);

        var presenter = new GetListSokatuMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListSokatuMstResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.CheckExistsReceInf)]
    public ActionResult<Response<CheckExistsReceInfResponse>> CheckExistsReceInf([FromQuery] CheckExistsReceInfRequest request)
    {
        var input = new CheckExistsReceInfInputData(HpId, request.SeikyuYm, request.PtId, request.SinYm, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new CheckExistsReceInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckExistsReceInfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.CheckExistSyobyoKeika)]
    public ActionResult<Response<CheckExistSyobyoKeikaResponse>> CheckExistSyobyoKeika([FromQuery] CheckExistSyobyoKeikaRequest request)
    {
        var input = new CheckExistSyobyoKeikaInputData(HpId, request.PtId, request.SinYm, request.HokenId, request.SinDay);
        var output = _bus.Handle(input);

        var presenter = new CheckExistSyobyoKeikaPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckExistSyobyoKeikaResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetNextUketukeNoBySetting)]
    public ActionResult<Response<GetNextUketukeNoBySettingResponse>> GetNextUketukeNoBySetting([FromQuery] GetNextUketukeNoBySettingRequest request)
    {
        var input = new GetNextUketukeNoBySettingInputData(HpId, request.Sindate, request.InfKbn, request.KaId, request.UketukeMode, request.DefaultUkeNo);
        var output = _bus.Handle(input);

        var presenter = new GetNextUketukeNoBySettingPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextUketukeNoBySettingResponse>>(presenter.Result);
    }

    #region Private function
    private ReceiptListAdvancedSearchInputData ConvertToReceiptListAdvancedSearchInputData(int hpId, ReceiptListAdvancedSearchRequest request)
    {
        var itemList = request.ItemList.Select(item => new ItemSearchInputItem(
                                                            item.ItemCd,
                                                            item.InputName,
                                                            item.RangeSeach,
                                                            item.Amount,
                                                            item.OrderStatus,
                                                            item.IsComment
                                        )).ToList();

        var byomeiCdList = request.ByomeiCdList.Select(item => new SearchByoMstInputItem(
                                                                    item.ByomeiCd,
                                                                    item.InputName,
                                                                    item.IsComment
                                               )).ToList();

        return new ReceiptListAdvancedSearchInputData(
                   hpId,
                   request.SeikyuYm,
                   request.Tokki,
                   request.IsAdvanceSearch,
                   request.HokenSbts,
                   request.IsAll,
                   request.IsNoSetting,
                   request.IsSystemSave,
                   request.IsSave1,
                   request.IsSave2,
                   request.IsSave3,
                   request.IsTempSave,
                   request.IsDone,
                   request.ReceSbtCenter,
                   request.ReceSbtRight,
                   request.HokenHoubetu,
                   request.Kohi1Houbetu,
                   request.Kohi2Houbetu,
                   request.Kohi3Houbetu,
                   request.Kohi4Houbetu,
                   request.IsIncludeSingle,
                   request.HokensyaNoFrom,
                   request.HokensyaNoTo,
                   request.HokensyaNoFrom.AsLong(),
                   request.HokensyaNoTo.AsLong(),
                   request.PtId,
                   request.PtIdFrom,
                   request.PtIdTo,
                   request.PtSearchOption,
                   request.TensuFrom,
                   request.TensuTo,
                   request.LastRaiinDateFrom,
                   request.LastRaiinDateTo,
                   request.BirthDayFrom,
                   request.BirthDayTo,
                   itemList,
                   request.ItemQuery,
                   request.IsOnlySuspectedDisease,
                   request.ByomeiQuery,
                   byomeiCdList,
                   request.IsFutanIncludeSingle,
                   request.FutansyaNoFromLong,
                   request.FutansyaNoToLong,
                   request.KaId,
                   request.DoctorId,
                   request.Name,
                   request.IsTestPatientSearch,
                   request.IsNotDisplayPrinted,
                   request.GroupSearchModels,
                   request.SeikyuKbnAll,
                   request.SeikyuKbnDenshi,
                   request.SeikyuKbnPaper);
    }

    private ReceCmtItem ConvertToReceCmtItem(SaveReceCmtRequestItem requestItem)
    {
        return new ReceCmtItem(
                    requestItem.Id,
                    requestItem.SeqNo,
                    requestItem.CmtKbn,
                    requestItem.CmtSbt,
                    requestItem.Cmt,
                    requestItem.CmtData,
                    requestItem.ItemCd,
                    requestItem.IsDeleted);
    }

    private SyoukiInfItem ConvertToSyoukiInfItem(SaveSyoukiInfRequestItem requestItem)
    {
        return new SyoukiInfItem(
                    requestItem.SeqNo,
                    requestItem.SortNo,
                    requestItem.SyoukiKbn,
                    requestItem.SyoukiKbnStartYm,
                    requestItem.Syouki,
                    requestItem.IsDeleted);
    }

    private SyobyoKeikaItem ConvertToSyobyoKeikaItem(SaveSyobyoKeikaRequestItem item)
    {
        return new SyobyoKeikaItem(
                    item.SinDay,
                    item.SeqNo,
                    item.Keika,
                    item.IsDeleted
                );
    }

    private ReceCheckCmtItem ConvertToReceCheckCmtItem(SaveReceCheckCmtListRequestItem item)
    {
        return new ReceCheckCmtItem(
                    item.SeqNo,
                    item.StatusColor,
                    item.Cmt,
                    item.IsChecked ? 1 : 0,
                    item.SortNo,
                    item.IsDeleted);
    }

    private ReceiptEditItem ConvertToReceiptEditItem(SaveReceiptEditRequest request)
    {
        return new ReceiptEditItem(
                   request.SeqNo,
                   request.HokenNissu,
                   request.Kohi1Nissu,
                   request.Kohi2Nissu,
                   request.Kohi3Nissu,
                   request.Kohi4Nissu,
                   request.Kohi1ReceKyufu,
                   request.Kohi2ReceKyufu,
                   request.Kohi3ReceKyufu,
                   request.Kohi4ReceKyufu,
                   request.HokenReceTensu,
                   request.HokenReceFutan,
                   request.Kohi1ReceTensu,
                   request.Kohi1ReceFutan,
                   request.Kohi2ReceTensu,
                   request.Kohi2ReceFutan,
                   request.Kohi3ReceTensu,
                   request.Kohi3ReceFutan,
                   request.Kohi4ReceTensu,
                   request.Kohi4ReceFutan,
                   request.IsDeleted,
                   request.Tokki1Id,
                   request.Tokki2Id,
                   request.Tokki3Id,
                   request.Tokki4Id,
                   request.Tokki5Id
            );
    }

    private ReceCheckErrorItem ConvertToReceCheckErrorItem(SaveReceCheckErrorListRequestItem item)
    {
        int isChecked = item.IsChecked ? 1 : 0;
        return new ReceCheckErrorItem(
                   item.ErrCd,
                   item.SinDate,
                   item.ACd,
                   item.BCd,
                   string.Empty,
                   string.Empty,
                   isChecked
               );
    }
    #endregion
}
