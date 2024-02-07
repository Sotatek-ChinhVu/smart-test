using Domain.Models.Yousiki;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Yousiki;
using EmrCloudApi.Requests.Yousiki;
using EmrCloudApi.Requests.Yousiki.RequestItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Services;
using Helper.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.Yousiki.AddYousiki;
using UseCase.Yousiki.CreateYuIchiFile;
using UseCase.Yousiki.DeleteYousikiInf;
using UseCase.Yousiki.GetHistoryYousiki;
using UseCase.Yousiki.GetKacodeYousikiMstDict;
using UseCase.Yousiki.GetVisitingInfs;
using UseCase.Yousiki.GetYousiki1InfDetails;
using UseCase.Yousiki.GetYousiki1InfDetailsByCodeNo;
using UseCase.Yousiki.GetYousiki1InfModel;
using UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;
using UseCase.Yousiki.UpdateYosiki;
using CreateYuIchiFileStatus = Helper.Messaging.Data.CreateYuIchiFileStatus;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class YousikiController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IMessenger _messenger;
    public YousikiController(UseCaseBus bus, IUserService userService, IMessenger messenger) : base(userService)
    {
        _bus = bus;
        _messenger = messenger;
    }

    [HttpGet(ApiPath.GetYousiki1InfModelWithCommonInf)]
    public ActionResult<Response<GetYousiki1InfModelWithCommonInfResponse>> GetYousiki1InfModelWithCommonInf([FromQuery] GetYousiki1InfModelWithCommonInfRequest request)
    {
        var input = new GetYousiki1InfModelWithCommonInfInputData(HpId, request.SinYm, request.PtNum, request.DataType, request.Status);
        var output = _bus.Handle(input);
        var presenter = new GetYousiki1InfModelWithCommonInfPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetYousiki1InfModelWithCommonInfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetYousiki1InfDetails)]
    public ActionResult<Response<GetYousiki1InfDetailsResponse>> GetYousiki1InfDetails([FromQuery] GetYousiki1InfDetailsRequest request)
    {
        var input = new GetYousiki1InfDetailsInputData(HpId, request.SinYm, request.PtId, request.DataType, request.SeqNo);
        var output = _bus.Handle(input);
        var presenter = new GetYousiki1InfDetailsPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetYousiki1InfDetailsResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetYousiki1InfDetailsByCodeNo)]
    public ActionResult<Response<GetYousiki1InfDetailsByCodeNoResponse>> GetYousiki1InfDetailsByCodeNo([FromQuery] GetYousiki1InfDetailsByCodeNoRequest request)
    {
        var input = new GetYousiki1InfDetailsByCodeNoInputData(HpId, request.SinYm, request.PtId, request.DataType, request.SeqNo, request.CodeNo);
        var output = _bus.Handle(input);
        var presenter = new GetYousiki1InfDetailsByCodeNoPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetYousiki1InfDetailsByCodeNoResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetVisitingInfs)]
    public ActionResult<Response<GetVisitingInfsResponse>> GetVisitingInfs([FromQuery] GetVisitingInfsRequest request)
    {
        var input = new GetVisitingInfsInputData(HpId, request.PtId, request.SinYm);
        var output = _bus.Handle(input);
        var presenter = new GetVisitingInfsPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetVisitingInfsResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetHistoryYousiki)]
    public ActionResult<Response<GetHistoryYousikiResponse>> GetHistoryYousiki([FromQuery] GetHistoryYousikiRequest request)
    {
        var input = new GetHistoryYousikiInputData(HpId, request.SinYm, request.PtId, request.DataType);
        var output = _bus.Handle(input);
        var presenter = new GetHistoryYousikiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetHistoryYousikiResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.AddYousiki)]
    public ActionResult<Response<AddYousikiResponse>> AddYousiki([FromBody] AddYousikiRequest request)
    {
        var input = new AddYousikiInputData(HpId, UserId, request.SinYm, request.PtNum, request.SelectDataType, new ReactAddYousiki(request.ReactAddYousiki.ConfirmSelectDataType));
        var output = _bus.Handle(input);
        var presenter = new AddYousikiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<AddYousikiResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.DeleteYousikiInf)]
    public ActionResult<Response<DeleteYousikiInfResponse>> DeleteYousikiInf([FromBody] DeleteYousikiInfRequest request)
    {
        var input = new DeleteYousikiInfInputData(HpId, UserId, request.SinYm, request.PtId);
        var output = _bus.Handle(input);
        var presenter = new DeleteYousikiInfPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<DeleteYousikiInfResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.CreateYuIchiFile)]
    public void CreateYuIchiFile([FromBody] CreateYuIchiFileRequest request)
    {
        try
        {
            _messenger.Register<CreateYuIchiFileStatus>(this, UpdateCreateYuIchiFileStatus);
            HttpContext.Response.ContentType = "application/json";
            var input = new CreateYuIchiFileInputData(HpId, request.SinYm, request.IsCreateForm1File, request.IsCreateEFFile, request.IsCreateEFile, request.IsCreateFFile, request.IsCreateKData, request.IsCheckedTestPatient, new ReactCreateYuIchiFile(request.ReactCreateYuIchiFile.ConfirmPatientList), _messenger);
            _bus.Handle(input);
        }
        finally
        {
            HttpContext.Response.Body.Close();
            _messenger.Deregister<CreateYuIchiFileStatus>(this, UpdateCreateYuIchiFileStatus);
        }
    }

    [HttpGet(ApiPath.GetYousiki1InfModel)]
    public ActionResult<Response<GetYousiki1InfModelResponse>> GetYousiki1InfModel([FromQuery] GetYousiki1InfModelRequest request)
    {
        var input = new GetYousiki1InfModelInputData(HpId, request.SinYm, request.PtNum, request.DataType);
        var output = _bus.Handle(input);
        var presenter = new GetYousiki1InfModelPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetYousiki1InfModelResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetKacodeYousikiMstDict)]
    public ActionResult<Response<GetKacodeYousikiMstDictResponse>> GetKacodeYousikiMstDict()
    {
        var input = new GetKacodeYousikiMstDictInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetKacodeYousikiMstDictPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKacodeYousikiMstDictResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpdateYousiki)]
    public ActionResult<Response<UpdateYosikiResponse>> UpdateYosiki([FromBody] UpdateYosikiRequest request)
    {
        List<Yousiki1InfDetailModel> yousiki1InfDetails = ConvertRequestToYousiki1InfDetailModel(request).Item1.Where(x => x.PtId != 0).ToList();
        List<CategoryModel> categories = ConvertRequestToYousiki1InfDetailModel(request).Item2;

        var input = new UpdateYosikiInputData(HpId, UserId, new Yousiki1InfModel(HpId, request.Yousiki1Inf.PtId, request.Yousiki1Inf.SinYm, request.Yousiki1Inf.DataType, request.Yousiki1Inf.SeqNo, request.Yousiki1Inf.IsDeleted, request.Yousiki1Inf.Status), yousiki1InfDetails, categories, request.IsTemporarySave);
        var output = _bus.Handle(input);
        var presenter = new UpdateYosikiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<UpdateYosikiResponse>>(presenter.Result);
    }

    private (List<Yousiki1InfDetailModel>, List<CategoryModel>) ConvertRequestToYousiki1InfDetailModel(UpdateYosikiRequest request)
    {
        List<Yousiki1InfDetailModel> result = new();
        List<CategoryModel> categories = new();

        var ptId = request.Yousiki1Inf.PtId;
        var sinYm = request.Yousiki1Inf.SinYm;
        int seqNo = GetSeqNo(request.Yousiki1Inf.DataTypeSeqNoDic, 0, request.Yousiki1Inf.CategoryRequests[0].IsDeleted);
        string prefixString;
        string suffixString;
        string fullByomei;
        string codeNo = "";

        foreach (var categorie in request.Yousiki1Inf.CategoryRequests)
        {
            categories.Add(new CategoryModel(categorie.DataType, categorie.IsDeleted));
        }

        //ConvertTabCommon
        #region 
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.CommonRequest.Yousiki1InfDetails ?? new())
        {
            result.Add(new Yousiki1InfDetailModel(ptId, sinYm, 0, seqNo, "CN00001", yousiki1InfDetailRequest.RowNo, yousiki1InfDetailRequest.Payload, yousiki1InfDetailRequest.Value, yousiki1InfDetailRequest.IsDeleted));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.CommonRequest.DiagnosticInjurys)
        {
            var valueModifierDiagnosticInjurys = "";
            prefixString = "";
            suffixString = "";
            fullByomei = "";
            var isDeleted = yousiki1InfDetailRequest.IsDeleted ? 1 : 0;
            codeNo = "CD00001";
            var rowNo = yousiki1InfDetailRequest.SortNo;

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList ?? new())
            {
                if (value == null)
                {
                    continue;
                }
                valueModifierDiagnosticInjurys = valueModifierDiagnosticInjurys + value.Code;

                if (value.Code.StartsWith('8'))
                {
                    suffixString = suffixString + value.Name;
                }
                else
                {
                    prefixString = prefixString + value.Name;
                }
            }

            fullByomei = prefixString + yousiki1InfDetailRequest.Byomei + suffixString;

            void SetYousiki1InfDetailModel(int payload, string value)
            {
                result.Add(new Yousiki1InfDetailModel(
                   ptId,
                   sinYm,
                   0,
                   seqNo,
                   codeNo,
                   rowNo,
                   payload,
                   value,
                   isDeleted
                   ));
            }
            if (yousiki1InfDetailRequest.ValueSelect != null)
            {
                SetYousiki1InfDetailModel(1, yousiki1InfDetailRequest.ValueSelect.Value);
            }

            if (!string.IsNullOrEmpty(fullByomei))
            {
                SetYousiki1InfDetailModel(9, fullByomei);
            }

            if (yousiki1InfDetailRequest.Icd10 != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.Icd10.Payload, yousiki1InfDetailRequest.Icd10.Value);
            }

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(3, yousiki1InfDetailRequest.ByomeiCd);
            }

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(4, valueModifierDiagnosticInjurys);
            }

            if (yousiki1InfDetailRequest.DateOfHospitalization != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.DateOfHospitalization.Payload, yousiki1InfDetailRequest.DateOfHospitalization.Value);
            }

            if (yousiki1InfDetailRequest.DischargeDate != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.DischargeDate.Payload, yousiki1InfDetailRequest.DischargeDate.Value);
            }

            if (yousiki1InfDetailRequest.Destination != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.Destination.Payload, yousiki1InfDetailRequest.Destination.Value);
            }

            if (yousiki1InfDetailRequest.HouseCallDate != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.HouseCallDate.Payload, yousiki1InfDetailRequest.HouseCallDate.Value);
            }

            if (yousiki1InfDetailRequest.MedicalInstitution != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.MedicalInstitution.Payload, yousiki1InfDetailRequest.MedicalInstitution.Value);
            }

            if (yousiki1InfDetailRequest.StartDate != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.StartDate.Payload, yousiki1InfDetailRequest.StartDate.Value);
            }

            if (yousiki1InfDetailRequest.OnsetDate != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.OnsetDate.Payload, yousiki1InfDetailRequest.OnsetDate.Value);
            }

            if (yousiki1InfDetailRequest.MaximumNumberDate != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.MaximumNumberDate.Payload, yousiki1InfDetailRequest.MaximumNumberDate.Value);
            }
        }

        var duringMonthMedicineInfModel = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.DuringMonthMedicineInfModel;

        if (duringMonthMedicineInfModel != null)
        {
            result.Add(new Yousiki1InfDetailModel(ptId, sinYm, 0, seqNo, duringMonthMedicineInfModel.CodeNo, duringMonthMedicineInfModel.RowNo, duringMonthMedicineInfModel.Payload, duringMonthMedicineInfModel.Value, duringMonthMedicineInfModel.IsDeleted));
        }

        var finalMedicineDateModel = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.FinalMedicineDateModel;

        if (finalMedicineDateModel != null)
        {
            result.Add(new Yousiki1InfDetailModel(ptId, sinYm, 0, seqNo, finalMedicineDateModel.CodeNo, finalMedicineDateModel.RowNo, finalMedicineDateModel.Payload, finalMedicineDateModel.Value, finalMedicineDateModel.IsDeleted));
        }

        var byomeiInfCommon = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.ByomeiInf;

        var valueModifier = "";
        prefixString = "";
        suffixString = "";
        fullByomei = "";

        foreach (var value in byomeiInfCommon.PrefixSuffixList ?? new())
        {
            valueModifier = valueModifier + value.Code;

            if (value.Code.StartsWith('8'))
            {
                suffixString = suffixString + value.Name;
            }
            else
            {
                prefixString = prefixString + value.Name;
            }
        }

        fullByomei = prefixString + byomeiInfCommon.Byomei + suffixString;

        SetYousiki1InfDetail(byomeiInfCommon.ValueSelect);

        if (!string.IsNullOrEmpty(fullByomei))
        {
            result.Add(new Yousiki1InfDetailModel(
                           ptId,
                           sinYm,
                           0,
                           seqNo,
                           "CH00001",
                           byomeiInfCommon.SortNo,
                           9,
                           fullByomei,
                           byomeiInfCommon.IsDeleted ? 1 : 0
                           ));
        }

        SetYousiki1InfDetail(byomeiInfCommon.Icd10);

        if (!string.IsNullOrEmpty(byomeiInfCommon.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               0,
               seqNo,
               "CDF0001",
               byomeiInfCommon.SortNo,
               4,
               byomeiInfCommon.ByomeiCd,
               byomeiInfCommon.IsDeleted ? 1 : 0
               ));
        }

        if (!string.IsNullOrEmpty(byomeiInfCommon.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               0,
               seqNo,
               "CDF0001",
               byomeiInfCommon.SortNo,
               5,
               valueModifier,
               byomeiInfCommon.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetail(byomeiInfCommon.DateOfHospitalization);

        SetYousiki1InfDetail(byomeiInfCommon.DischargeDate);

        SetYousiki1InfDetail(byomeiInfCommon.Destination);

        SetYousiki1InfDetail(byomeiInfCommon.HouseCallDate);

        SetYousiki1InfDetail(byomeiInfCommon.MedicalInstitution);

        SetYousiki1InfDetail(byomeiInfCommon.StartDate);

        SetYousiki1InfDetail(byomeiInfCommon.OnsetDate);

        SetYousiki1InfDetail(byomeiInfCommon.MaximumNumberDate);

        void SetYousiki1InfDetail(Yousiki1InfDetailRequest? requestItem)
        {
            if (requestItem != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                               requestItem.PtId,
                               requestItem.SinYm,
                               requestItem.DataType,
                               requestItem.SeqNo,
                               requestItem.CodeNo,
                               requestItem.RowNo,
                               requestItem.Payload,
                               requestItem.Value,
                               byomeiInfCommon.IsDeleted ? 1 : 0
                               ));
            }
        }

        var duringMonthMedicineInfFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.DuringMonthMedicineInfModel;

        if (duringMonthMedicineInfFinalExaminationInf != null)
        {
            result.Add(new Yousiki1InfDetailModel(
                               duringMonthMedicineInfFinalExaminationInf.PtId,
                               duringMonthMedicineInfFinalExaminationInf.SinYm,
                               duringMonthMedicineInfFinalExaminationInf.DataType,
                               duringMonthMedicineInfFinalExaminationInf.SeqNo,
                               duringMonthMedicineInfFinalExaminationInf.CodeNo,
                               duringMonthMedicineInfFinalExaminationInf.RowNo,
                               duringMonthMedicineInfFinalExaminationInf.Payload,
                               duringMonthMedicineInfFinalExaminationInf.Value,
                               duringMonthMedicineInfFinalExaminationInf.IsDeleted
                               ));
        }

        var finalMedicineDateFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.FinalMedicineDateModel;

        if (finalMedicineDateFinalExaminationInf != null)
        {
            result.Add(new Yousiki1InfDetailModel(
                               finalMedicineDateFinalExaminationInf.PtId,
                               finalMedicineDateFinalExaminationInf.SinYm,
                               finalMedicineDateFinalExaminationInf.DataType,
                               finalMedicineDateFinalExaminationInf.SeqNo,
                               finalMedicineDateFinalExaminationInf.CodeNo,
                               finalMedicineDateFinalExaminationInf.RowNo,
                               finalMedicineDateFinalExaminationInf.Payload,
                               finalMedicineDateFinalExaminationInf.Value,
                               finalMedicineDateFinalExaminationInf.IsDeleted
                    ));
        }

        var byomeiInfCommonFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.ByomeiInf;
        var valueModifierFinalExaminationInf = "";
        prefixString = "";
        suffixString = "";
        fullByomei = "";

        foreach (var value in byomeiInfCommonFinalExaminationInf.PrefixSuffixList ?? new())
        {
            valueModifierFinalExaminationInf = valueModifierFinalExaminationInf + value.Code;

            if (value.Code.StartsWith('8'))
            {
                suffixString = suffixString + value.Name;
            }
            else
            {
                prefixString = prefixString + value.Name;
            }
        }

        fullByomei = prefixString + byomeiInfCommonFinalExaminationInf.Byomei + suffixString;

        void SetYousiki1InfDetailFinalExaminationInf(Yousiki1InfDetailRequest? requestItem)
        {
            if (requestItem != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                               requestItem.PtId,
                               requestItem.SinYm,
                               requestItem.DataType,
                               requestItem.SeqNo,
                               requestItem.CodeNo,
                               requestItem.RowNo,
                               requestItem.Payload,
                               requestItem.Value,
                               byomeiInfCommonFinalExaminationInf.IsDeleted ? 1 : 0
                               ));
            }
        }

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.ValueSelect);

        if (!string.IsNullOrEmpty(fullByomei))
        {
            result.Add(new Yousiki1InfDetailModel(
                           ptId,
                           sinYm,
                           0,
                           seqNo,
                           "CDF0001",
                           byomeiInfCommonFinalExaminationInf.SortNo,
                           9,
                           fullByomei,
                           byomeiInfCommonFinalExaminationInf.IsDeleted ? 1 : 0
                           ));
        }

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.Icd10);

        if (!string.IsNullOrEmpty(byomeiInfCommonFinalExaminationInf.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               0,
               seqNo,
               "CDF0001",
               byomeiInfCommonFinalExaminationInf.SortNo,
               4,
               byomeiInfCommonFinalExaminationInf.ByomeiCd,
               byomeiInfCommonFinalExaminationInf.IsDeleted ? 1 : 0
               ));
        }

        if (!string.IsNullOrEmpty(byomeiInfCommonFinalExaminationInf.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               0,
               seqNo,
               "CDF0001",
               byomeiInfCommonFinalExaminationInf.SortNo,
               5,
               valueModifierFinalExaminationInf,
               byomeiInfCommonFinalExaminationInf.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.DateOfHospitalization);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.DischargeDate);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.Destination);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.HouseCallDate);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.MedicalInstitution);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.StartDate);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.OnsetDate);

        SetYousiki1InfDetailFinalExaminationInf(byomeiInfCommonFinalExaminationInf.MaximumNumberDate);
        #endregion

        //ConvertTabLivingHabit
        #region 
        seqNo = GetSeqNo(request.Yousiki1Inf.DataTypeSeqNoDic, 1, request.Yousiki1Inf.CategoryRequests[1].IsDeleted);

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.Yousiki1InfDetails ?? new())
        {
            result.Add(new Yousiki1InfDetailModel(
                   ptId,
                   sinYm,
                   1,
                   seqNo,
                   yousiki1InfDetailRequest.CodeNo,
                   yousiki1InfDetailRequest.RowNo,
                   yousiki1InfDetailRequest.Payload,
                   yousiki1InfDetailRequest.Value,
                   yousiki1InfDetailRequest.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.OutpatientConsultationInfs)
        {
            void SetYousiki1InfDetailOutpatientConsultationInfs(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetailOutpatientConsultationInfs(yousiki1InfDetailRequest.ConsultationDate);

            SetYousiki1InfDetailOutpatientConsultationInfs(yousiki1InfDetailRequest.FirstVisit);

            SetYousiki1InfDetailOutpatientConsultationInfs(yousiki1InfDetailRequest.AppearanceReferral);

            SetYousiki1InfDetailOutpatientConsultationInfs(yousiki1InfDetailRequest.DepartmentCode);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.StrokeHistorys)
        {

            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Type);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OnsetDate);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.AcuteCoronaryHistorys)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Type);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OnsetDate);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.AcuteAorticHistorys)
        {
            if (yousiki1InfDetailRequest.OnsetDate != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                   ));
            }
        }
        #endregion

        //ConvertTabAtHome
        #region 

        seqNo = GetSeqNo(request.Yousiki1Inf.DataTypeSeqNoDic, 2, request.Yousiki1Inf.CategoryRequests[2].IsDeleted);
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.Yousiki1InfDetails ?? new())
        {
            if (yousiki1InfDetailRequest != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.PtId,
                   yousiki1InfDetailRequest.SinYm,
                   yousiki1InfDetailRequest.DataType,
                   yousiki1InfDetailRequest.SeqNo,
                   yousiki1InfDetailRequest.CodeNo,
                   yousiki1InfDetailRequest.RowNo,
                   yousiki1InfDetailRequest.Payload,
                   yousiki1InfDetailRequest.Value,
                   yousiki1InfDetailRequest.IsDeleted
                   ));
            }
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusVisits)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.SinDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MedicalInstitution);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusVisitNursingList)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.SinDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MedicalInstitution);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusEmergencyConsultations)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.EmergencyConsultationDay);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Destination);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.ConsultationRoute);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OutCome);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusShortTermAdmissions)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.AdmissionDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DischargeDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Service);
        }

        var patientSitutationListValue = "";

        foreach (var patientSitutation in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.PatientSitutations)
        {
            patientSitutationListValue = patientSitutationListValue + patientSitutation.SituationValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            request.Yousiki1Inf.PtId,
            request.Yousiki1Inf.SinYm,
            2,
            request.Yousiki1Inf.SeqNo,
            "HPS0001",
            0,
            1,
            patientSitutationListValue
            ));

        var barthelIndexValue = "";

        foreach (var item in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.BarthelIndexs)
        {
            barthelIndexValue = barthelIndexValue + item.PointValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            request.Yousiki1Inf.PtId,
            request.Yousiki1Inf.SinYm,
            2,
            request.Yousiki1Inf.SeqNo,
            "HPS0002",
            0,
            1,
            barthelIndexValue
            ));

        var statusNurtritionValue = "";

        foreach (var statusNurtritionList in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusNurtritions)
        {
            statusNurtritionValue = statusNurtritionValue + statusNurtritionList.PointValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            request.Yousiki1Inf.PtId,
            request.Yousiki1Inf.SinYm,
            2,
            request.Yousiki1Inf.SeqNo,
            "HPS0006",
            0,
            3,
            patientSitutationListValue
            ));

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.HospitalizationStatus)
        {
            var valueModifierHospitalizationStatus = "";
            prefixString = "";
            suffixString = "";
            fullByomei = "";

            codeNo = "HCH0001";
            var rowNo = yousiki1InfDetailRequest.SortNo;
            var isDeleted = yousiki1InfDetailRequest.IsDeleted ? 1 : 0;

            void SetYousiki1InfDetailModel(int payload, string value)
            {
                result.Add(new Yousiki1InfDetailModel(
                   ptId,
                   sinYm,
                   0,
                   seqNo,
                   codeNo,
                   rowNo,
                   payload,
                   value,
                   isDeleted
                   ));
            }

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList ?? new())
            {
                valueModifierHospitalizationStatus = valueModifierHospitalizationStatus + value.Code;

                if (value.Code.StartsWith('8'))
                {
                    suffixString = suffixString + value.Name;
                }
                else
                {
                    prefixString = prefixString + value.Name;
                }

            }

            fullByomei = prefixString + yousiki1InfDetailRequest.Byomei + suffixString;

            if (yousiki1InfDetailRequest.ValueSelect != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.ValueSelect.Payload, yousiki1InfDetailRequest.ValueSelect.Value);
            }

            if (!string.IsNullOrEmpty(fullByomei))
            {
                SetYousiki1InfDetailModel(9, fullByomei);
            }

            if (yousiki1InfDetailRequest.Icd10 != null)
            {
                SetYousiki1InfDetailModel(yousiki1InfDetailRequest.Icd10.Payload, yousiki1InfDetailRequest.Icd10.Value);
            }

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(5, yousiki1InfDetailRequest.ByomeiCd);
            }

            if (yousiki1InfDetailRequest.ModifierCode != null)
            {
                SetYousiki1InfDetailModel(6, valueModifierHospitalizationStatus);
            }

            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DateOfHospitalization);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DischargeDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Destination);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.HouseCallDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MedicalInstitution);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.StartDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OnsetDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MaximumNumberDate);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusHomeVisits)
        {
            var valueModifierStatusHomeVisits = "";
            prefixString = "";
            suffixString = "";
            fullByomei = "";

            codeNo = "HCHC001";
            var rowNo = yousiki1InfDetailRequest.SortNo;
            var isDeleted = yousiki1InfDetailRequest.IsDeleted ? 1 : 0;

            void SetYousiki1InfDetailModel(int payload, string value)
            {
                result.Add(new Yousiki1InfDetailModel(
                   ptId,
                   sinYm,
                   0,
                   seqNo,
                   codeNo,
                   rowNo,
                   payload,
                   value,
                   isDeleted
                   ));
            }

            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList ?? new())
            {
                valueModifierStatusHomeVisits = valueModifierStatusHomeVisits + value.Code;

                if (value.Code.StartsWith('8'))
                {
                    suffixString = suffixString + value.Name;
                }
                else
                {
                    prefixString = prefixString + value.Name;
                }
            }

            fullByomei = prefixString + yousiki1InfDetailRequest.Byomei + suffixString;

            SetYousiki1InfDetails(yousiki1InfDetailRequest.ValueSelect);

            if (!string.IsNullOrEmpty(fullByomei))
            {
                SetYousiki1InfDetailModel(9, fullByomei);
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Icd10);

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(4, yousiki1InfDetailRequest.ByomeiCd);
            }

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(5, valueModifierStatusHomeVisits);
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DateOfHospitalization);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DischargeDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Destination);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.HouseCallDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MedicalInstitution);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.StartDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OnsetDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MaximumNumberDate);
        }

        var finalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.FinalExaminationInf;
        var valueModifierFinalExaminationInfAtHome = "";
        prefixString = "";
        suffixString = "";
        fullByomei = "";

        foreach (var value in finalExaminationInf.PrefixSuffixList ?? new())
        {
            valueModifierFinalExaminationInfAtHome = valueModifierFinalExaminationInfAtHome + value.Code;

            if (value.Code.StartsWith('8'))
            {
                suffixString = suffixString + value.Name;
            }
            else
            {
                prefixString = prefixString + value.Name;
            }
        }

        fullByomei = prefixString + finalExaminationInf.Byomei + suffixString;

        void SetYousiki1InfDetailFinalExaminationInfs(Yousiki1InfDetailRequest? requestItem)
        {
            if (requestItem != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                               requestItem.PtId,
                               requestItem.SinYm,
                               requestItem.DataType,
                               requestItem.SeqNo,
                               requestItem.CodeNo,
                               requestItem.RowNo,
                               requestItem.Payload,
                               requestItem.Value,
                               finalExaminationInf.IsDeleted ? 1 : 0
                               ));
            }
        }

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.ValueSelect);

        if (!string.IsNullOrEmpty(fullByomei))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HCVD001",
               finalExaminationInf.SortNo,
               9,
               fullByomei,
               finalExaminationInf.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.Icd10);

        if (!string.IsNullOrEmpty(finalExaminationInf.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HCVD001",
               finalExaminationInf.SortNo,
               3,
               finalExaminationInf.ByomeiCd,
               finalExaminationInf.IsDeleted ? 1 : 0
               ));
        }

        if (!string.IsNullOrEmpty(finalExaminationInf.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HCVD001",
               finalExaminationInf.SortNo,
               4,
               valueModifierFinalExaminationInfAtHome,
               finalExaminationInf.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.DateOfHospitalization);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.DischargeDate);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.Destination);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.HouseCallDate);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.MedicalInstitution);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.StartDate);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.OnsetDate);

        SetYousiki1InfDetailFinalExaminationInfs(finalExaminationInf.MaximumNumberDate);



        var finalExaminationInf2 = request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.FinalExaminationInf2;
        var valueModifierFinalExaminationInf2 = "";
        prefixString = "";
        suffixString = "";
        fullByomei = "";

        void SetYousiki1InfDetailFinalExaminationInf2s(Yousiki1InfDetailRequest? requestItem)
        {
            if (requestItem != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                               requestItem.PtId,
                               requestItem.SinYm,
                               requestItem.DataType,
                               requestItem.SeqNo,
                               requestItem.CodeNo,
                               requestItem.RowNo,
                               requestItem.Payload,
                               requestItem.Value,
                               finalExaminationInf2.IsDeleted ? 1 : 0
                               ));
            }
        }

        foreach (var value in finalExaminationInf2.PrefixSuffixList ?? new())
        {
            valueModifierFinalExaminationInf2 = valueModifierFinalExaminationInf2 + value.Code;

            if (value.Code.StartsWith('8'))
            {
                suffixString = suffixString + value.Name;
            }
            else
            {
                prefixString = prefixString + value.Name;
            }
        }

        fullByomei = prefixString + finalExaminationInf2.Byomei + suffixString;

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.ValueSelect);

        if (!string.IsNullOrEmpty(fullByomei))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HPCD001",
               finalExaminationInf2.SortNo,
               9,
               fullByomei,
               finalExaminationInf2.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.Icd10);

        if (!string.IsNullOrEmpty(finalExaminationInf2.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HPCD001",
               finalExaminationInf2.SortNo,
               2,
               finalExaminationInf2.ByomeiCd,
               finalExaminationInf2.IsDeleted ? 1 : 0
               ));
        }

        if (!string.IsNullOrEmpty(finalExaminationInf2.ByomeiCd))
        {
            result.Add(new Yousiki1InfDetailModel(
               ptId,
               sinYm,
               2,
               seqNo,
               "HPCD001",
               finalExaminationInf2.SortNo,
               4,
               valueModifierFinalExaminationInf2,
               finalExaminationInf2.IsDeleted ? 1 : 0
               ));
        }

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.DateOfHospitalization);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.DischargeDate);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.Destination);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.HouseCallDate);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.MedicalInstitution);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.StartDate);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.OnsetDate);

        SetYousiki1InfDetailFinalExaminationInf2s(finalExaminationInf2.MaximumNumberDate);
        #endregion

        //ConvertTabRehabilitation
        #region

        seqNo = GetSeqNo(request.Yousiki1Inf.DataTypeSeqNoDic, 3, request.Yousiki1Inf.CategoryRequests[3].IsDeleted);
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.Yousiki1InfDetails)
        {
            if (yousiki1InfDetailRequest != null)
            {
                result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.PtId,
                   yousiki1InfDetailRequest.SinYm,
                   yousiki1InfDetailRequest.DataType,
                   yousiki1InfDetailRequest.SeqNo,
                   yousiki1InfDetailRequest.CodeNo,
                   yousiki1InfDetailRequest.RowNo,
                   yousiki1InfDetailRequest.Payload,
                   yousiki1InfDetailRequest.Value,
                   yousiki1InfDetailRequest.IsDeleted
                   ));
            }
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.OutpatientConsultations)
        {
            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   requestItem.IsDeleted
                                   ));
                }
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.ConsultationDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.FirstVisit);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Referral);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DepartmentCode);
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.ByomeiRehabilitations)
        {
            var valueModifierByomeiRehabilitations = "";
            prefixString = "";
            suffixString = "";
            fullByomei = "";
            codeNo = "RCD0001";
            var rowNo = yousiki1InfDetailRequest.SortNo;
            var isDeleted = yousiki1InfDetailRequest.IsDeleted ? 1 : 0;

            void SetYousiki1InfDetailModel(int payload, string value)
            {
                result.Add(new Yousiki1InfDetailModel(
                   ptId,
                   sinYm,
                   0,
                   seqNo,
                   codeNo,
                   rowNo,
                   payload,
                   value,
                   isDeleted
                   ));
            }

            void SetYousiki1InfDetails(Yousiki1InfDetailRequest? requestItem)
            {
                if (requestItem != null)
                {
                    result.Add(new Yousiki1InfDetailModel(
                                   requestItem.PtId,
                                   requestItem.SinYm,
                                   requestItem.DataType,
                                   requestItem.SeqNo,
                                   requestItem.CodeNo,
                                   requestItem.RowNo,
                                   requestItem.Payload,
                                   requestItem.Value,
                                   yousiki1InfDetailRequest.IsDeleted ? 1 : 0
                                   ));
                }
            }

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList ?? new())
            {
                valueModifierByomeiRehabilitations = valueModifierByomeiRehabilitations + value.Code;

                if (value.Code.StartsWith('8'))
                {
                    suffixString = suffixString + value.Name;
                }
                else
                {
                    prefixString = prefixString + value.Name;
                }
            }

            fullByomei = prefixString + yousiki1InfDetailRequest.Byomei + suffixString;


            SetYousiki1InfDetails(yousiki1InfDetailRequest.ValueSelect);

            if (!string.IsNullOrEmpty(fullByomei))
            {
                SetYousiki1InfDetailModel(9, fullByomei);
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Icd10);

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(5, yousiki1InfDetailRequest.ByomeiCd);
            }

            if (!string.IsNullOrEmpty(yousiki1InfDetailRequest.ByomeiCd))
            {
                SetYousiki1InfDetailModel(6, valueModifierByomeiRehabilitations);
            }

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DateOfHospitalization);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.DischargeDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.Destination);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.HouseCallDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MedicalInstitution);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.StartDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.OnsetDate);

            SetYousiki1InfDetails(yousiki1InfDetailRequest.MaximumNumberDate);
        }

        var barthelIndex = "";
        foreach (var item in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.BarthelIndexs)
        {
            barthelIndex = barthelIndex + item.StatusValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            request.Yousiki1Inf.PtId,
            request.Yousiki1Inf.SinYm,
            3,
            request.Yousiki1Inf.SeqNo,
            "RPADL01",
            0,
            1,
            barthelIndex
            )
        );

        var fimListValue = "";
        foreach (var item in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.Fims)
        {
            fimListValue = fimListValue + item.StatusValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            request.Yousiki1Inf.PtId,
            request.Yousiki1Inf.SinYm,
            3,
            request.Yousiki1Inf.SeqNo,
            "RPADL01",
            0,
            2,
            fimListValue
            )
            );
        #endregion

        return (result, categories);
    }

    #region private function
    /// <summary>
    /// Status = 0 => update
    /// Status = 1 => delete
    /// Status = 2 => addNew
    /// </summary>
    /// <param name="dataTypeSeqNoDic"></param>
    /// <param name="dataType"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    private int GetSeqNo(Dictionary<int, int> dataTypeSeqNoDic, int dataType, int status)
    {
        int seqNo = dataTypeSeqNoDic.ContainsKey(dataType) ? dataTypeSeqNoDic[dataType] : 0;
        if (status == 2)
        {
            if (seqNo <= 0)
            {
                seqNo = 1;
            }
            else
            {
                seqNo = seqNo + 1;
            }
        }
        return seqNo;
    }

    private void UpdateCreateYuIchiFileStatus(CreateYuIchiFileStatus status)
    {
        string result = "\n" + JsonSerializer.Serialize(status);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.Write(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.Flush();
    }
    #endregion
}