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

        foreach (var categorie in request.Yousiki1Inf.CategoryRequests)
        {
            categories.Add(new CategoryModel(categorie.DataType, categorie.IsDeleted));
        }

        //ConvertTabCommon
        #region 
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.CommonRequest.Yousiki1InfDetails)
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

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.CommonRequest.DiagnosticInjurys)
        {
            var valueModifierDiagnosticInjurys = "";

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList)
            {
                valueModifierDiagnosticInjurys = valueModifierDiagnosticInjurys + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ValueSelect.PtId,
                   yousiki1InfDetailRequest.ValueSelect.SinYm,
                   yousiki1InfDetailRequest.ValueSelect.DataType,
                   yousiki1InfDetailRequest.ValueSelect.SeqNo,
                   yousiki1InfDetailRequest.ValueSelect.CodeNo,
                   yousiki1InfDetailRequest.ValueSelect.RowNo,
                   yousiki1InfDetailRequest.ValueSelect.Payload,
                   yousiki1InfDetailRequest.ValueSelect.Value,
                   yousiki1InfDetailRequest.ValueSelect.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameLast.PtId,
                   yousiki1InfDetailRequest.InjuryNameLast.SinYm,
                   yousiki1InfDetailRequest.InjuryNameLast.DataType,
                   yousiki1InfDetailRequest.InjuryNameLast.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameLast.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameLast.RowNo,
                   yousiki1InfDetailRequest.InjuryNameLast.Payload,
                   yousiki1InfDetailRequest.FullByomei,
                   yousiki1InfDetailRequest.InjuryNameLast.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Icd10.PtId,
                   yousiki1InfDetailRequest.Icd10.SinYm,
                   yousiki1InfDetailRequest.Icd10.DataType,
                   yousiki1InfDetailRequest.Icd10.SeqNo,
                   yousiki1InfDetailRequest.Icd10.CodeNo,
                   yousiki1InfDetailRequest.Icd10.RowNo,
                   yousiki1InfDetailRequest.Icd10.Payload,
                   yousiki1InfDetailRequest.Icd10.Value,
                   yousiki1InfDetailRequest.Icd10.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameCode.PtId,
                   yousiki1InfDetailRequest.InjuryNameCode.SinYm,
                   yousiki1InfDetailRequest.InjuryNameCode.DataType,
                   yousiki1InfDetailRequest.InjuryNameCode.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameCode.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameCode.RowNo,
                   yousiki1InfDetailRequest.InjuryNameCode.Payload,
                   yousiki1InfDetailRequest.ByomeiCd,
                   yousiki1InfDetailRequest.InjuryNameCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ModifierCode.PtId,
                   yousiki1InfDetailRequest.ModifierCode.SinYm,
                   yousiki1InfDetailRequest.ModifierCode.DataType,
                   yousiki1InfDetailRequest.ModifierCode.SeqNo,
                   yousiki1InfDetailRequest.ModifierCode.CodeNo,
                   yousiki1InfDetailRequest.ModifierCode.RowNo,
                   yousiki1InfDetailRequest.ModifierCode.Payload,
                   valueModifierDiagnosticInjurys,
                   yousiki1InfDetailRequest.ModifierCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DateOfHospitalization.PtId,
                   yousiki1InfDetailRequest.DateOfHospitalization.SinYm,
                   yousiki1InfDetailRequest.DateOfHospitalization.DataType,
                   yousiki1InfDetailRequest.DateOfHospitalization.SeqNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.CodeNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.RowNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.Payload,
                   yousiki1InfDetailRequest.DateOfHospitalization.Value,
                   yousiki1InfDetailRequest.DateOfHospitalization.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DischargeDate.PtId,
                   yousiki1InfDetailRequest.DischargeDate.SinYm,
                   yousiki1InfDetailRequest.DischargeDate.DataType,
                   yousiki1InfDetailRequest.DischargeDate.SeqNo,
                   yousiki1InfDetailRequest.DischargeDate.CodeNo,
                   yousiki1InfDetailRequest.DischargeDate.RowNo,
                   yousiki1InfDetailRequest.DischargeDate.Payload,
                   yousiki1InfDetailRequest.DischargeDate.Value,
                   yousiki1InfDetailRequest.DischargeDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Destination.PtId,
                   yousiki1InfDetailRequest.Destination.SinYm,
                   yousiki1InfDetailRequest.Destination.DataType,
                   yousiki1InfDetailRequest.Destination.SeqNo,
                   yousiki1InfDetailRequest.Destination.CodeNo,
                   yousiki1InfDetailRequest.Destination.RowNo,
                   yousiki1InfDetailRequest.Destination.Payload,
                   yousiki1InfDetailRequest.Destination.Value,
                   yousiki1InfDetailRequest.Destination.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.HouseCallDate.PtId,
                   yousiki1InfDetailRequest.HouseCallDate.SinYm,
                   yousiki1InfDetailRequest.HouseCallDate.DataType,
                   yousiki1InfDetailRequest.HouseCallDate.SeqNo,
                   yousiki1InfDetailRequest.HouseCallDate.CodeNo,
                   yousiki1InfDetailRequest.HouseCallDate.RowNo,
                   yousiki1InfDetailRequest.HouseCallDate.Payload,
                   yousiki1InfDetailRequest.HouseCallDate.Value,
                   yousiki1InfDetailRequest.HouseCallDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.StartDate.PtId,
                   yousiki1InfDetailRequest.StartDate.SinYm,
                   yousiki1InfDetailRequest.StartDate.DataType,
                   yousiki1InfDetailRequest.StartDate.SeqNo,
                   yousiki1InfDetailRequest.StartDate.CodeNo,
                   yousiki1InfDetailRequest.StartDate.RowNo,
                   yousiki1InfDetailRequest.StartDate.Payload,
                   yousiki1InfDetailRequest.StartDate.Value,
                   yousiki1InfDetailRequest.StartDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MaximumNumberDate.PtId,
                   yousiki1InfDetailRequest.MaximumNumberDate.SinYm,
                   yousiki1InfDetailRequest.MaximumNumberDate.DataType,
                   yousiki1InfDetailRequest.MaximumNumberDate.SeqNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.CodeNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.RowNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.Payload,
                   yousiki1InfDetailRequest.MaximumNumberDate.Value,
                   yousiki1InfDetailRequest.MaximumNumberDate.IsDeleted
                   ));
        }

        var duringMonthMedicineInfModel = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.DuringMonthMedicineInfModel;

        result.Add(new Yousiki1InfDetailModel(
                   duringMonthMedicineInfModel.PtId,
                   duringMonthMedicineInfModel.SinYm,
                   duringMonthMedicineInfModel.DataType,
                   duringMonthMedicineInfModel.SeqNo,
                   duringMonthMedicineInfModel.CodeNo,
                   duringMonthMedicineInfModel.RowNo,
                   duringMonthMedicineInfModel.Payload,
                   duringMonthMedicineInfModel.Value,
                   duringMonthMedicineInfModel.IsDeleted
                   ));

        var finalMedicineDateModel = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.FinalMedicineDateModel;

        result.Add(new Yousiki1InfDetailModel(
                   finalMedicineDateModel.PtId,
                   finalMedicineDateModel.SinYm,
                   finalMedicineDateModel.DataType,
                   finalMedicineDateModel.SeqNo,
                   finalMedicineDateModel.CodeNo,
                   finalMedicineDateModel.RowNo,
                   finalMedicineDateModel.Payload,
                   finalMedicineDateModel.Value,
                   finalMedicineDateModel.IsDeleted
        ));

        var byomeiInfCommon = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.HospitalizationStatusInf.ByomeiInf;
        var valueModifier = "";

        foreach (var value in byomeiInfCommon.PrefixSuffixList)
        {
            valueModifier = valueModifier + value.Code;
        }

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.ValueSelect.PtId,
               byomeiInfCommon.ValueSelect.SinYm,
               byomeiInfCommon.ValueSelect.DataType,
               byomeiInfCommon.ValueSelect.SeqNo,
               byomeiInfCommon.ValueSelect.CodeNo,
               byomeiInfCommon.ValueSelect.RowNo,
               byomeiInfCommon.ValueSelect.Payload,
               byomeiInfCommon.ValueSelect.Value,
               byomeiInfCommon.ValueSelect.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.InjuryNameLast.PtId,
               byomeiInfCommon.InjuryNameLast.SinYm,
               byomeiInfCommon.InjuryNameLast.DataType,
               byomeiInfCommon.InjuryNameLast.SeqNo,
               byomeiInfCommon.InjuryNameLast.CodeNo,
               byomeiInfCommon.InjuryNameLast.RowNo,
               byomeiInfCommon.InjuryNameLast.Payload,
               byomeiInfCommon.FullByomei,
               byomeiInfCommon.InjuryNameLast.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.Icd10.PtId,
               byomeiInfCommon.Icd10.SinYm,
               byomeiInfCommon.Icd10.DataType,
               byomeiInfCommon.Icd10.SeqNo,
               byomeiInfCommon.Icd10.CodeNo,
               byomeiInfCommon.Icd10.RowNo,
               byomeiInfCommon.Icd10.Payload,
               byomeiInfCommon.Icd10.Value,
               byomeiInfCommon.Icd10.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.InjuryNameCode.PtId,
               byomeiInfCommon.InjuryNameCode.SinYm,
               byomeiInfCommon.InjuryNameCode.DataType,
               byomeiInfCommon.InjuryNameCode.SeqNo,
               byomeiInfCommon.InjuryNameCode.CodeNo,
               byomeiInfCommon.InjuryNameCode.RowNo,
               byomeiInfCommon.InjuryNameCode.Payload,
               byomeiInfCommon.ByomeiCd,
               byomeiInfCommon.InjuryNameCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.ModifierCode.PtId,
               byomeiInfCommon.ModifierCode.SinYm,
               byomeiInfCommon.ModifierCode.DataType,
               byomeiInfCommon.ModifierCode.SeqNo,
               byomeiInfCommon.ModifierCode.CodeNo,
               byomeiInfCommon.ModifierCode.RowNo,
               byomeiInfCommon.ModifierCode.Payload,
               valueModifier,
               byomeiInfCommon.ModifierCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.DateOfHospitalization.PtId,
               byomeiInfCommon.DateOfHospitalization.SinYm,
               byomeiInfCommon.DateOfHospitalization.DataType,
               byomeiInfCommon.DateOfHospitalization.SeqNo,
               byomeiInfCommon.DateOfHospitalization.CodeNo,
               byomeiInfCommon.DateOfHospitalization.RowNo,
               byomeiInfCommon.DateOfHospitalization.Payload,
               byomeiInfCommon.DateOfHospitalization.Value,
               byomeiInfCommon.DateOfHospitalization.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.DischargeDate.PtId,
               byomeiInfCommon.DischargeDate.SinYm,
               byomeiInfCommon.DischargeDate.DataType,
               byomeiInfCommon.DischargeDate.SeqNo,
               byomeiInfCommon.DischargeDate.CodeNo,
               byomeiInfCommon.DischargeDate.RowNo,
               byomeiInfCommon.DischargeDate.Payload,
               byomeiInfCommon.DischargeDate.Value,
               byomeiInfCommon.DischargeDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.Destination.PtId,
               byomeiInfCommon.Destination.SinYm,
               byomeiInfCommon.Destination.DataType,
               byomeiInfCommon.Destination.SeqNo,
               byomeiInfCommon.Destination.CodeNo,
               byomeiInfCommon.Destination.RowNo,
               byomeiInfCommon.Destination.Payload,
               byomeiInfCommon.Destination.Value,
               byomeiInfCommon.Destination.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.HouseCallDate.PtId,
               byomeiInfCommon.HouseCallDate.SinYm,
               byomeiInfCommon.HouseCallDate.DataType,
               byomeiInfCommon.HouseCallDate.SeqNo,
               byomeiInfCommon.HouseCallDate.CodeNo,
               byomeiInfCommon.HouseCallDate.RowNo,
               byomeiInfCommon.HouseCallDate.Payload,
               byomeiInfCommon.HouseCallDate.Value,
               byomeiInfCommon.HouseCallDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.MedicalInstitution.PtId,
               byomeiInfCommon.MedicalInstitution.SinYm,
               byomeiInfCommon.MedicalInstitution.DataType,
               byomeiInfCommon.MedicalInstitution.SeqNo,
               byomeiInfCommon.MedicalInstitution.CodeNo,
               byomeiInfCommon.MedicalInstitution.RowNo,
               byomeiInfCommon.MedicalInstitution.Payload,
               byomeiInfCommon.MedicalInstitution.Value,
               byomeiInfCommon.MedicalInstitution.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.StartDate.PtId,
               byomeiInfCommon.StartDate.SinYm,
               byomeiInfCommon.StartDate.DataType,
               byomeiInfCommon.StartDate.SeqNo,
               byomeiInfCommon.StartDate.CodeNo,
               byomeiInfCommon.StartDate.RowNo,
               byomeiInfCommon.StartDate.Payload,
               byomeiInfCommon.StartDate.Value,
               byomeiInfCommon.StartDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.OnsetDate.PtId,
               byomeiInfCommon.OnsetDate.SinYm,
               byomeiInfCommon.OnsetDate.DataType,
               byomeiInfCommon.OnsetDate.SeqNo,
               byomeiInfCommon.OnsetDate.CodeNo,
               byomeiInfCommon.OnsetDate.RowNo,
               byomeiInfCommon.OnsetDate.Payload,
               byomeiInfCommon.OnsetDate.Value,
               byomeiInfCommon.OnsetDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommon.MaximumNumberDate.PtId,
               byomeiInfCommon.MaximumNumberDate.SinYm,
               byomeiInfCommon.MaximumNumberDate.DataType,
               byomeiInfCommon.MaximumNumberDate.SeqNo,
               byomeiInfCommon.MaximumNumberDate.CodeNo,
               byomeiInfCommon.MaximumNumberDate.RowNo,
               byomeiInfCommon.MaximumNumberDate.Payload,
               byomeiInfCommon.MaximumNumberDate.Value,
               byomeiInfCommon.MaximumNumberDate.IsDeleted
               ));

        //

        var duringMonthMedicineInfFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.DuringMonthMedicineInfModel;

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

        var finalMedicineDateFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.FinalMedicineDateModel;

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

        var byomeiInfCommonFinalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.CommonRequest.FinalExaminationInf.ByomeiInf;
        var valueModifierFinalExaminationInf = "";

        foreach (var value in byomeiInfCommonFinalExaminationInf.PrefixSuffixList)
        {
            valueModifierFinalExaminationInf = valueModifierFinalExaminationInf + value.Code;
        }

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.ValueSelect.PtId,
               byomeiInfCommonFinalExaminationInf.ValueSelect.SinYm,
               byomeiInfCommonFinalExaminationInf.ValueSelect.DataType,
               byomeiInfCommonFinalExaminationInf.ValueSelect.SeqNo,
               byomeiInfCommonFinalExaminationInf.ValueSelect.CodeNo,
               byomeiInfCommonFinalExaminationInf.ValueSelect.RowNo,
               byomeiInfCommonFinalExaminationInf.ValueSelect.Payload,
               byomeiInfCommonFinalExaminationInf.ValueSelect.Value,
               byomeiInfCommonFinalExaminationInf.ValueSelect.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.PtId,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.SinYm,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.DataType,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.SeqNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.CodeNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.RowNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.Payload,
               byomeiInfCommonFinalExaminationInf.FullByomei,
               byomeiInfCommonFinalExaminationInf.InjuryNameLast.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.Icd10.PtId,
               byomeiInfCommonFinalExaminationInf.Icd10.SinYm,
               byomeiInfCommonFinalExaminationInf.Icd10.DataType,
               byomeiInfCommonFinalExaminationInf.Icd10.SeqNo,
               byomeiInfCommonFinalExaminationInf.Icd10.CodeNo,
               byomeiInfCommonFinalExaminationInf.Icd10.RowNo,
               byomeiInfCommonFinalExaminationInf.Icd10.Payload,
               byomeiInfCommonFinalExaminationInf.Icd10.Value,
               byomeiInfCommonFinalExaminationInf.Icd10.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.PtId,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.SinYm,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.DataType,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.SeqNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.CodeNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.RowNo,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.Payload,
               byomeiInfCommonFinalExaminationInf.ByomeiCd,
               byomeiInfCommonFinalExaminationInf.InjuryNameCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.ModifierCode.PtId,
               byomeiInfCommonFinalExaminationInf.ModifierCode.SinYm,
               byomeiInfCommonFinalExaminationInf.ModifierCode.DataType,
               byomeiInfCommonFinalExaminationInf.ModifierCode.SeqNo,
               byomeiInfCommonFinalExaminationInf.ModifierCode.CodeNo,
               byomeiInfCommonFinalExaminationInf.ModifierCode.RowNo,
               byomeiInfCommonFinalExaminationInf.ModifierCode.Payload,
               valueModifierFinalExaminationInf,
               byomeiInfCommonFinalExaminationInf.ModifierCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.PtId,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.SinYm,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.DataType,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.SeqNo,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.CodeNo,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.RowNo,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.Payload,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.Value,
               byomeiInfCommonFinalExaminationInf.DateOfHospitalization.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.DischargeDate.PtId,
               byomeiInfCommonFinalExaminationInf.DischargeDate.SinYm,
               byomeiInfCommonFinalExaminationInf.DischargeDate.DataType,
               byomeiInfCommonFinalExaminationInf.DischargeDate.SeqNo,
               byomeiInfCommonFinalExaminationInf.DischargeDate.CodeNo,
               byomeiInfCommonFinalExaminationInf.DischargeDate.RowNo,
               byomeiInfCommonFinalExaminationInf.DischargeDate.Payload,
               byomeiInfCommonFinalExaminationInf.DischargeDate.Value,
               byomeiInfCommonFinalExaminationInf.DischargeDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.Destination.PtId,
               byomeiInfCommonFinalExaminationInf.Destination.SinYm,
               byomeiInfCommonFinalExaminationInf.Destination.DataType,
               byomeiInfCommonFinalExaminationInf.Destination.SeqNo,
               byomeiInfCommonFinalExaminationInf.Destination.CodeNo,
               byomeiInfCommonFinalExaminationInf.Destination.RowNo,
               byomeiInfCommonFinalExaminationInf.Destination.Payload,
               byomeiInfCommonFinalExaminationInf.Destination.Value,
               byomeiInfCommonFinalExaminationInf.Destination.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.HouseCallDate.PtId,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.SinYm,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.DataType,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.SeqNo,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.CodeNo,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.RowNo,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.Payload,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.Value,
               byomeiInfCommonFinalExaminationInf.HouseCallDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.PtId,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.SinYm,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.DataType,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.SeqNo,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.CodeNo,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.RowNo,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.Payload,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.Value,
               byomeiInfCommonFinalExaminationInf.MedicalInstitution.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.StartDate.PtId,
               byomeiInfCommonFinalExaminationInf.StartDate.SinYm,
               byomeiInfCommonFinalExaminationInf.StartDate.DataType,
               byomeiInfCommonFinalExaminationInf.StartDate.SeqNo,
               byomeiInfCommonFinalExaminationInf.StartDate.CodeNo,
               byomeiInfCommonFinalExaminationInf.StartDate.RowNo,
               byomeiInfCommonFinalExaminationInf.StartDate.Payload,
               byomeiInfCommonFinalExaminationInf.StartDate.Value,
               byomeiInfCommonFinalExaminationInf.StartDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.OnsetDate.PtId,
               byomeiInfCommonFinalExaminationInf.OnsetDate.SinYm,
               byomeiInfCommonFinalExaminationInf.OnsetDate.DataType,
               byomeiInfCommonFinalExaminationInf.OnsetDate.SeqNo,
               byomeiInfCommonFinalExaminationInf.OnsetDate.CodeNo,
               byomeiInfCommonFinalExaminationInf.OnsetDate.RowNo,
               byomeiInfCommonFinalExaminationInf.OnsetDate.Payload,
               byomeiInfCommonFinalExaminationInf.OnsetDate.Value,
               byomeiInfCommonFinalExaminationInf.OnsetDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.PtId,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.SinYm,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.DataType,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.SeqNo,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.CodeNo,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.RowNo,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.Payload,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.Value,
               byomeiInfCommonFinalExaminationInf.MaximumNumberDate.IsDeleted
               ));
        #endregion

        //ConvertTabLivingHabit
        #region 
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.Yousiki1InfDetails)
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

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.OutpatientConsultationInfs)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ConsultationDate.PtId,
                   yousiki1InfDetailRequest.ConsultationDate.SinYm,
                   yousiki1InfDetailRequest.ConsultationDate.DataType,
                   yousiki1InfDetailRequest.ConsultationDate.SeqNo,
                   yousiki1InfDetailRequest.ConsultationDate.CodeNo,
                   yousiki1InfDetailRequest.ConsultationDate.RowNo,
                   yousiki1InfDetailRequest.ConsultationDate.Payload,
                   yousiki1InfDetailRequest.ConsultationDate.Value,
                   yousiki1InfDetailRequest.ConsultationDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.FirstVisit.PtId,
                   yousiki1InfDetailRequest.FirstVisit.SinYm,
                   yousiki1InfDetailRequest.FirstVisit.DataType,
                   yousiki1InfDetailRequest.FirstVisit.SeqNo,
                   yousiki1InfDetailRequest.FirstVisit.CodeNo,
                   yousiki1InfDetailRequest.FirstVisit.RowNo,
                   yousiki1InfDetailRequest.FirstVisit.Payload,
                   yousiki1InfDetailRequest.FirstVisit.Value,
                   yousiki1InfDetailRequest.FirstVisit.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.AppearanceReferral.PtId,
                   yousiki1InfDetailRequest.AppearanceReferral.SinYm,
                   yousiki1InfDetailRequest.AppearanceReferral.DataType,
                   yousiki1InfDetailRequest.AppearanceReferral.SeqNo,
                   yousiki1InfDetailRequest.AppearanceReferral.CodeNo,
                   yousiki1InfDetailRequest.AppearanceReferral.RowNo,
                   yousiki1InfDetailRequest.AppearanceReferral.Payload,
                   yousiki1InfDetailRequest.AppearanceReferral.Value,
                   yousiki1InfDetailRequest.AppearanceReferral.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DepartmentCode.PtId,
                   yousiki1InfDetailRequest.DepartmentCode.SinYm,
                   yousiki1InfDetailRequest.DepartmentCode.DataType,
                   yousiki1InfDetailRequest.DepartmentCode.SeqNo,
                   yousiki1InfDetailRequest.DepartmentCode.CodeNo,
                   yousiki1InfDetailRequest.DepartmentCode.RowNo,
                   yousiki1InfDetailRequest.DepartmentCode.Payload,
                   yousiki1InfDetailRequest.DepartmentCode.Value,
                   yousiki1InfDetailRequest.DepartmentCode.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.StrokeHistorys)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Type.PtId,
                   yousiki1InfDetailRequest.Type.SinYm,
                   yousiki1InfDetailRequest.Type.DataType,
                   yousiki1InfDetailRequest.Type.SeqNo,
                   yousiki1InfDetailRequest.Type.CodeNo,
                   yousiki1InfDetailRequest.Type.RowNo,
                   yousiki1InfDetailRequest.Type.Payload,
                   yousiki1InfDetailRequest.Type.Value,
                   yousiki1InfDetailRequest.Type.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.AcuteCoronaryHistorys)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Type.PtId,
                   yousiki1InfDetailRequest.Type.SinYm,
                   yousiki1InfDetailRequest.Type.DataType,
                   yousiki1InfDetailRequest.Type.SeqNo,
                   yousiki1InfDetailRequest.Type.CodeNo,
                   yousiki1InfDetailRequest.Type.RowNo,
                   yousiki1InfDetailRequest.Type.Payload,
                   yousiki1InfDetailRequest.Type.Value,
                   yousiki1InfDetailRequest.Type.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.LivingHabitRequest.AcuteAorticHistorys)
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
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));
        }
        #endregion

        //ConvertTabAtHome
        #region 
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.Yousiki1InfDetails)
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

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusVisits)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.SinDate.PtId,
                   yousiki1InfDetailRequest.SinDate.SinYm,
                   yousiki1InfDetailRequest.SinDate.DataType,
                   yousiki1InfDetailRequest.SinDate.SeqNo,
                   yousiki1InfDetailRequest.SinDate.CodeNo,
                   yousiki1InfDetailRequest.SinDate.RowNo,
                   yousiki1InfDetailRequest.SinDate.Payload,
                   yousiki1InfDetailRequest.SinDate.Value,
                   yousiki1InfDetailRequest.SinDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusVisitNursingList)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.SinDate.PtId,
                   yousiki1InfDetailRequest.SinDate.SinYm,
                   yousiki1InfDetailRequest.SinDate.DataType,
                   yousiki1InfDetailRequest.SinDate.SeqNo,
                   yousiki1InfDetailRequest.SinDate.CodeNo,
                   yousiki1InfDetailRequest.SinDate.RowNo,
                   yousiki1InfDetailRequest.SinDate.Payload,
                   yousiki1InfDetailRequest.SinDate.Value,
                   yousiki1InfDetailRequest.SinDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusEmergencyConsultations)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.EmergencyConsultationDay.PtId,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.SinYm,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.DataType,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.SeqNo,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.CodeNo,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.RowNo,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.Payload,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.Value,
                   yousiki1InfDetailRequest.EmergencyConsultationDay.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Destination.PtId,
                   yousiki1InfDetailRequest.Destination.SinYm,
                   yousiki1InfDetailRequest.Destination.DataType,
                   yousiki1InfDetailRequest.Destination.SeqNo,
                   yousiki1InfDetailRequest.Destination.CodeNo,
                   yousiki1InfDetailRequest.Destination.RowNo,
                   yousiki1InfDetailRequest.Destination.Payload,
                   yousiki1InfDetailRequest.Destination.Value,
                   yousiki1InfDetailRequest.Destination.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ConsultationRoute.PtId,
                   yousiki1InfDetailRequest.ConsultationRoute.SinYm,
                   yousiki1InfDetailRequest.ConsultationRoute.DataType,
                   yousiki1InfDetailRequest.ConsultationRoute.SeqNo,
                   yousiki1InfDetailRequest.ConsultationRoute.CodeNo,
                   yousiki1InfDetailRequest.ConsultationRoute.RowNo,
                   yousiki1InfDetailRequest.ConsultationRoute.Payload,
                   yousiki1InfDetailRequest.ConsultationRoute.Value,
                   yousiki1InfDetailRequest.ConsultationRoute.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OutCome.PtId,
                   yousiki1InfDetailRequest.OutCome.SinYm,
                   yousiki1InfDetailRequest.OutCome.DataType,
                   yousiki1InfDetailRequest.OutCome.SeqNo,
                   yousiki1InfDetailRequest.OutCome.CodeNo,
                   yousiki1InfDetailRequest.OutCome.RowNo,
                   yousiki1InfDetailRequest.OutCome.Payload,
                   yousiki1InfDetailRequest.OutCome.Value,
                   yousiki1InfDetailRequest.OutCome.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusShortTermAdmissions)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.AdmissionDate.PtId,
                   yousiki1InfDetailRequest.AdmissionDate.SinYm,
                   yousiki1InfDetailRequest.AdmissionDate.DataType,
                   yousiki1InfDetailRequest.AdmissionDate.SeqNo,
                   yousiki1InfDetailRequest.AdmissionDate.CodeNo,
                   yousiki1InfDetailRequest.AdmissionDate.RowNo,
                   yousiki1InfDetailRequest.AdmissionDate.Payload,
                   yousiki1InfDetailRequest.AdmissionDate.Value,
                   yousiki1InfDetailRequest.AdmissionDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DischargeDate.PtId,
                   yousiki1InfDetailRequest.DischargeDate.SinYm,
                   yousiki1InfDetailRequest.DischargeDate.DataType,
                   yousiki1InfDetailRequest.DischargeDate.SeqNo,
                   yousiki1InfDetailRequest.DischargeDate.CodeNo,
                   yousiki1InfDetailRequest.DischargeDate.RowNo,
                   yousiki1InfDetailRequest.DischargeDate.Payload,
                   yousiki1InfDetailRequest.DischargeDate.Value,
                   yousiki1InfDetailRequest.DischargeDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Service.PtId,
                   yousiki1InfDetailRequest.Service.SinYm,
                   yousiki1InfDetailRequest.Service.DataType,
                   yousiki1InfDetailRequest.Service.SeqNo,
                   yousiki1InfDetailRequest.Service.CodeNo,
                   yousiki1InfDetailRequest.Service.RowNo,
                   yousiki1InfDetailRequest.Service.Payload,
                   yousiki1InfDetailRequest.Service.Value,
                   yousiki1InfDetailRequest.Service.IsDeleted
                   ));
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

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.HospitalizationStatuss)
        {
            var valueModifierHospitalizationStatus = "";

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList)
            {
                valueModifierHospitalizationStatus = valueModifierHospitalizationStatus + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ValueSelect.PtId,
                   yousiki1InfDetailRequest.ValueSelect.SinYm,
                   yousiki1InfDetailRequest.ValueSelect.DataType,
                   yousiki1InfDetailRequest.ValueSelect.SeqNo,
                   yousiki1InfDetailRequest.ValueSelect.CodeNo,
                   yousiki1InfDetailRequest.ValueSelect.RowNo,
                   yousiki1InfDetailRequest.ValueSelect.Payload,
                   yousiki1InfDetailRequest.ValueSelect.Value,
                   yousiki1InfDetailRequest.ValueSelect.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameLast.PtId,
                   yousiki1InfDetailRequest.InjuryNameLast.SinYm,
                   yousiki1InfDetailRequest.InjuryNameLast.DataType,
                   yousiki1InfDetailRequest.InjuryNameLast.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameLast.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameLast.RowNo,
                   yousiki1InfDetailRequest.InjuryNameLast.Payload,
                   yousiki1InfDetailRequest.FullByomei,
                   yousiki1InfDetailRequest.InjuryNameLast.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Icd10.PtId,
                   yousiki1InfDetailRequest.Icd10.SinYm,
                   yousiki1InfDetailRequest.Icd10.DataType,
                   yousiki1InfDetailRequest.Icd10.SeqNo,
                   yousiki1InfDetailRequest.Icd10.CodeNo,
                   yousiki1InfDetailRequest.Icd10.RowNo,
                   yousiki1InfDetailRequest.Icd10.Payload,
                   yousiki1InfDetailRequest.Icd10.Value,
                   yousiki1InfDetailRequest.Icd10.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameCode.PtId,
                   yousiki1InfDetailRequest.InjuryNameCode.SinYm,
                   yousiki1InfDetailRequest.InjuryNameCode.DataType,
                   yousiki1InfDetailRequest.InjuryNameCode.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameCode.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameCode.RowNo,
                   yousiki1InfDetailRequest.InjuryNameCode.Payload,
                   yousiki1InfDetailRequest.ByomeiCd,
                   yousiki1InfDetailRequest.InjuryNameCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ModifierCode.PtId,
                   yousiki1InfDetailRequest.ModifierCode.SinYm,
                   yousiki1InfDetailRequest.ModifierCode.DataType,
                   yousiki1InfDetailRequest.ModifierCode.SeqNo,
                   yousiki1InfDetailRequest.ModifierCode.CodeNo,
                   yousiki1InfDetailRequest.ModifierCode.RowNo,
                   yousiki1InfDetailRequest.ModifierCode.Payload,
                   valueModifierHospitalizationStatus,
                   yousiki1InfDetailRequest.ModifierCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DateOfHospitalization.PtId,
                   yousiki1InfDetailRequest.DateOfHospitalization.SinYm,
                   yousiki1InfDetailRequest.DateOfHospitalization.DataType,
                   yousiki1InfDetailRequest.DateOfHospitalization.SeqNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.CodeNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.RowNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.Payload,
                   yousiki1InfDetailRequest.DateOfHospitalization.Value,
                   yousiki1InfDetailRequest.DateOfHospitalization.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DischargeDate.PtId,
                   yousiki1InfDetailRequest.DischargeDate.SinYm,
                   yousiki1InfDetailRequest.DischargeDate.DataType,
                   yousiki1InfDetailRequest.DischargeDate.SeqNo,
                   yousiki1InfDetailRequest.DischargeDate.CodeNo,
                   yousiki1InfDetailRequest.DischargeDate.RowNo,
                   yousiki1InfDetailRequest.DischargeDate.Payload,
                   yousiki1InfDetailRequest.DischargeDate.Value,
                   yousiki1InfDetailRequest.DischargeDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Destination.PtId,
                   yousiki1InfDetailRequest.Destination.SinYm,
                   yousiki1InfDetailRequest.Destination.DataType,
                   yousiki1InfDetailRequest.Destination.SeqNo,
                   yousiki1InfDetailRequest.Destination.CodeNo,
                   yousiki1InfDetailRequest.Destination.RowNo,
                   yousiki1InfDetailRequest.Destination.Payload,
                   yousiki1InfDetailRequest.Destination.Value,
                   yousiki1InfDetailRequest.Destination.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.HouseCallDate.PtId,
                   yousiki1InfDetailRequest.HouseCallDate.SinYm,
                   yousiki1InfDetailRequest.HouseCallDate.DataType,
                   yousiki1InfDetailRequest.HouseCallDate.SeqNo,
                   yousiki1InfDetailRequest.HouseCallDate.CodeNo,
                   yousiki1InfDetailRequest.HouseCallDate.RowNo,
                   yousiki1InfDetailRequest.HouseCallDate.Payload,
                   yousiki1InfDetailRequest.HouseCallDate.Value,
                   yousiki1InfDetailRequest.HouseCallDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.StartDate.PtId,
                   yousiki1InfDetailRequest.StartDate.SinYm,
                   yousiki1InfDetailRequest.StartDate.DataType,
                   yousiki1InfDetailRequest.StartDate.SeqNo,
                   yousiki1InfDetailRequest.StartDate.CodeNo,
                   yousiki1InfDetailRequest.StartDate.RowNo,
                   yousiki1InfDetailRequest.StartDate.Payload,
                   yousiki1InfDetailRequest.StartDate.Value,
                   yousiki1InfDetailRequest.StartDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MaximumNumberDate.PtId,
                   yousiki1InfDetailRequest.MaximumNumberDate.SinYm,
                   yousiki1InfDetailRequest.MaximumNumberDate.DataType,
                   yousiki1InfDetailRequest.MaximumNumberDate.SeqNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.CodeNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.RowNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.Payload,
                   yousiki1InfDetailRequest.MaximumNumberDate.Value,
                   yousiki1InfDetailRequest.MaximumNumberDate.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.StatusHomeVisits)
        {
            var valueModifierStatusHomeVisits = "";

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList)
            {
                valueModifierStatusHomeVisits = valueModifierStatusHomeVisits + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ValueSelect.PtId,
                   yousiki1InfDetailRequest.ValueSelect.SinYm,
                   yousiki1InfDetailRequest.ValueSelect.DataType,
                   yousiki1InfDetailRequest.ValueSelect.SeqNo,
                   yousiki1InfDetailRequest.ValueSelect.CodeNo,
                   yousiki1InfDetailRequest.ValueSelect.RowNo,
                   yousiki1InfDetailRequest.ValueSelect.Payload,
                   yousiki1InfDetailRequest.ValueSelect.Value,
                   yousiki1InfDetailRequest.ValueSelect.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameLast.PtId,
                   yousiki1InfDetailRequest.InjuryNameLast.SinYm,
                   yousiki1InfDetailRequest.InjuryNameLast.DataType,
                   yousiki1InfDetailRequest.InjuryNameLast.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameLast.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameLast.RowNo,
                   yousiki1InfDetailRequest.InjuryNameLast.Payload,
                   yousiki1InfDetailRequest.FullByomei,
                   yousiki1InfDetailRequest.InjuryNameLast.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Icd10.PtId,
                   yousiki1InfDetailRequest.Icd10.SinYm,
                   yousiki1InfDetailRequest.Icd10.DataType,
                   yousiki1InfDetailRequest.Icd10.SeqNo,
                   yousiki1InfDetailRequest.Icd10.CodeNo,
                   yousiki1InfDetailRequest.Icd10.RowNo,
                   yousiki1InfDetailRequest.Icd10.Payload,
                   yousiki1InfDetailRequest.Icd10.Value,
                   yousiki1InfDetailRequest.Icd10.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameCode.PtId,
                   yousiki1InfDetailRequest.InjuryNameCode.SinYm,
                   yousiki1InfDetailRequest.InjuryNameCode.DataType,
                   yousiki1InfDetailRequest.InjuryNameCode.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameCode.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameCode.RowNo,
                   yousiki1InfDetailRequest.InjuryNameCode.Payload,
                   yousiki1InfDetailRequest.ByomeiCd,
                   yousiki1InfDetailRequest.InjuryNameCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ModifierCode.PtId,
                   yousiki1InfDetailRequest.ModifierCode.SinYm,
                   yousiki1InfDetailRequest.ModifierCode.DataType,
                   yousiki1InfDetailRequest.ModifierCode.SeqNo,
                   yousiki1InfDetailRequest.ModifierCode.CodeNo,
                   yousiki1InfDetailRequest.ModifierCode.RowNo,
                   yousiki1InfDetailRequest.ModifierCode.Payload,
                   valueModifierStatusHomeVisits,
                   yousiki1InfDetailRequest.ModifierCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DateOfHospitalization.PtId,
                   yousiki1InfDetailRequest.DateOfHospitalization.SinYm,
                   yousiki1InfDetailRequest.DateOfHospitalization.DataType,
                   yousiki1InfDetailRequest.DateOfHospitalization.SeqNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.CodeNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.RowNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.Payload,
                   yousiki1InfDetailRequest.DateOfHospitalization.Value,
                   yousiki1InfDetailRequest.DateOfHospitalization.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DischargeDate.PtId,
                   yousiki1InfDetailRequest.DischargeDate.SinYm,
                   yousiki1InfDetailRequest.DischargeDate.DataType,
                   yousiki1InfDetailRequest.DischargeDate.SeqNo,
                   yousiki1InfDetailRequest.DischargeDate.CodeNo,
                   yousiki1InfDetailRequest.DischargeDate.RowNo,
                   yousiki1InfDetailRequest.DischargeDate.Payload,
                   yousiki1InfDetailRequest.DischargeDate.Value,
                   yousiki1InfDetailRequest.DischargeDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Destination.PtId,
                   yousiki1InfDetailRequest.Destination.SinYm,
                   yousiki1InfDetailRequest.Destination.DataType,
                   yousiki1InfDetailRequest.Destination.SeqNo,
                   yousiki1InfDetailRequest.Destination.CodeNo,
                   yousiki1InfDetailRequest.Destination.RowNo,
                   yousiki1InfDetailRequest.Destination.Payload,
                   yousiki1InfDetailRequest.Destination.Value,
                   yousiki1InfDetailRequest.Destination.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.HouseCallDate.PtId,
                   yousiki1InfDetailRequest.HouseCallDate.SinYm,
                   yousiki1InfDetailRequest.HouseCallDate.DataType,
                   yousiki1InfDetailRequest.HouseCallDate.SeqNo,
                   yousiki1InfDetailRequest.HouseCallDate.CodeNo,
                   yousiki1InfDetailRequest.HouseCallDate.RowNo,
                   yousiki1InfDetailRequest.HouseCallDate.Payload,
                   yousiki1InfDetailRequest.HouseCallDate.Value,
                   yousiki1InfDetailRequest.HouseCallDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.StartDate.PtId,
                   yousiki1InfDetailRequest.StartDate.SinYm,
                   yousiki1InfDetailRequest.StartDate.DataType,
                   yousiki1InfDetailRequest.StartDate.SeqNo,
                   yousiki1InfDetailRequest.StartDate.CodeNo,
                   yousiki1InfDetailRequest.StartDate.RowNo,
                   yousiki1InfDetailRequest.StartDate.Payload,
                   yousiki1InfDetailRequest.StartDate.Value,
                   yousiki1InfDetailRequest.StartDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MaximumNumberDate.PtId,
                   yousiki1InfDetailRequest.MaximumNumberDate.SinYm,
                   yousiki1InfDetailRequest.MaximumNumberDate.DataType,
                   yousiki1InfDetailRequest.MaximumNumberDate.SeqNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.CodeNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.RowNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.Payload,
                   yousiki1InfDetailRequest.MaximumNumberDate.Value,
                   yousiki1InfDetailRequest.MaximumNumberDate.IsDeleted
                   ));
        }

        var finalExaminationInf = request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.FinalExaminationInf;
        var valueModifierFinalExaminationInfAtHome = "";

        foreach (var value in finalExaminationInf.PrefixSuffixList)
        {
            valueModifierFinalExaminationInfAtHome = valueModifierFinalExaminationInfAtHome + value.Code;
        }

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.ValueSelect.PtId,
               finalExaminationInf.ValueSelect.SinYm,
               finalExaminationInf.ValueSelect.DataType,
               finalExaminationInf.ValueSelect.SeqNo,
               finalExaminationInf.ValueSelect.CodeNo,
               finalExaminationInf.ValueSelect.RowNo,
               finalExaminationInf.ValueSelect.Payload,
               finalExaminationInf.ValueSelect.Value,
               finalExaminationInf.ValueSelect.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.InjuryNameLast.PtId,
               finalExaminationInf.InjuryNameLast.SinYm,
               finalExaminationInf.InjuryNameLast.DataType,
               finalExaminationInf.InjuryNameLast.SeqNo,
               finalExaminationInf.InjuryNameLast.CodeNo,
               finalExaminationInf.InjuryNameLast.RowNo,
               finalExaminationInf.InjuryNameLast.Payload,
               finalExaminationInf.FullByomei,
               finalExaminationInf.InjuryNameLast.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.Icd10.PtId,
               finalExaminationInf.Icd10.SinYm,
               finalExaminationInf.Icd10.DataType,
               finalExaminationInf.Icd10.SeqNo,
               finalExaminationInf.Icd10.CodeNo,
               finalExaminationInf.Icd10.RowNo,
               finalExaminationInf.Icd10.Payload,
               finalExaminationInf.Icd10.Value,
               finalExaminationInf.Icd10.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.InjuryNameCode.PtId,
               finalExaminationInf.InjuryNameCode.SinYm,
               finalExaminationInf.InjuryNameCode.DataType,
               finalExaminationInf.InjuryNameCode.SeqNo,
               finalExaminationInf.InjuryNameCode.CodeNo,
               finalExaminationInf.InjuryNameCode.RowNo,
               finalExaminationInf.InjuryNameCode.Payload,
               finalExaminationInf.ByomeiCd,
               finalExaminationInf.InjuryNameCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.ModifierCode.PtId,
               finalExaminationInf.ModifierCode.SinYm,
               finalExaminationInf.ModifierCode.DataType,
               finalExaminationInf.ModifierCode.SeqNo,
               finalExaminationInf.ModifierCode.CodeNo,
               finalExaminationInf.ModifierCode.RowNo,
               finalExaminationInf.ModifierCode.Payload,
               valueModifierFinalExaminationInfAtHome,
               finalExaminationInf.ModifierCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.DateOfHospitalization.PtId,
               finalExaminationInf.DateOfHospitalization.SinYm,
               finalExaminationInf.DateOfHospitalization.DataType,
               finalExaminationInf.DateOfHospitalization.SeqNo,
               finalExaminationInf.DateOfHospitalization.CodeNo,
               finalExaminationInf.DateOfHospitalization.RowNo,
               finalExaminationInf.DateOfHospitalization.Payload,
               finalExaminationInf.DateOfHospitalization.Value,
               finalExaminationInf.DateOfHospitalization.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.DischargeDate.PtId,
               finalExaminationInf.DischargeDate.SinYm,
               finalExaminationInf.DischargeDate.DataType,
               finalExaminationInf.DischargeDate.SeqNo,
               finalExaminationInf.DischargeDate.CodeNo,
               finalExaminationInf.DischargeDate.RowNo,
               finalExaminationInf.DischargeDate.Payload,
               finalExaminationInf.DischargeDate.Value,
               finalExaminationInf.DischargeDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.Destination.PtId,
               finalExaminationInf.Destination.SinYm,
               finalExaminationInf.Destination.DataType,
               finalExaminationInf.Destination.SeqNo,
               finalExaminationInf.Destination.CodeNo,
               finalExaminationInf.Destination.RowNo,
               finalExaminationInf.Destination.Payload,
               finalExaminationInf.Destination.Value,
               finalExaminationInf.Destination.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.HouseCallDate.PtId,
               finalExaminationInf.HouseCallDate.SinYm,
               finalExaminationInf.HouseCallDate.DataType,
               finalExaminationInf.HouseCallDate.SeqNo,
               finalExaminationInf.HouseCallDate.CodeNo,
               finalExaminationInf.HouseCallDate.RowNo,
               finalExaminationInf.HouseCallDate.Payload,
               finalExaminationInf.HouseCallDate.Value,
               finalExaminationInf.HouseCallDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.MedicalInstitution.PtId,
               finalExaminationInf.MedicalInstitution.SinYm,
               finalExaminationInf.MedicalInstitution.DataType,
               finalExaminationInf.MedicalInstitution.SeqNo,
               finalExaminationInf.MedicalInstitution.CodeNo,
               finalExaminationInf.MedicalInstitution.RowNo,
               finalExaminationInf.MedicalInstitution.Payload,
               finalExaminationInf.MedicalInstitution.Value,
               finalExaminationInf.MedicalInstitution.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.StartDate.PtId,
               finalExaminationInf.StartDate.SinYm,
               finalExaminationInf.StartDate.DataType,
               finalExaminationInf.StartDate.SeqNo,
               finalExaminationInf.StartDate.CodeNo,
               finalExaminationInf.StartDate.RowNo,
               finalExaminationInf.StartDate.Payload,
               finalExaminationInf.StartDate.Value,
               finalExaminationInf.StartDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.OnsetDate.PtId,
               finalExaminationInf.OnsetDate.SinYm,
               finalExaminationInf.OnsetDate.DataType,
               finalExaminationInf.OnsetDate.SeqNo,
               finalExaminationInf.OnsetDate.CodeNo,
               finalExaminationInf.OnsetDate.RowNo,
               finalExaminationInf.OnsetDate.Payload,
               finalExaminationInf.OnsetDate.Value,
               finalExaminationInf.OnsetDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf.MaximumNumberDate.PtId,
               finalExaminationInf.MaximumNumberDate.SinYm,
               finalExaminationInf.MaximumNumberDate.DataType,
               finalExaminationInf.MaximumNumberDate.SeqNo,
               finalExaminationInf.MaximumNumberDate.CodeNo,
               finalExaminationInf.MaximumNumberDate.RowNo,
               finalExaminationInf.MaximumNumberDate.Payload,
               finalExaminationInf.MaximumNumberDate.Value,
               finalExaminationInf.MaximumNumberDate.IsDeleted
               ));

        var finalExaminationInf2 = request.Yousiki1Inf.TabYousikiRequest.AtHomeRequest.FinalExaminationInf2;
        var valueModifierFinalExaminationInf2 = "";

        foreach (var value in finalExaminationInf.PrefixSuffixList)
        {
            valueModifierFinalExaminationInf2 = valueModifierFinalExaminationInf2 + value.Code;
        }

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.ValueSelect.PtId,
               finalExaminationInf2.ValueSelect.SinYm,
               finalExaminationInf2.ValueSelect.DataType,
               finalExaminationInf2.ValueSelect.SeqNo,
               finalExaminationInf2.ValueSelect.CodeNo,
               finalExaminationInf2.ValueSelect.RowNo,
               finalExaminationInf2.ValueSelect.Payload,
               finalExaminationInf2.ValueSelect.Value,
               finalExaminationInf2.ValueSelect.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.InjuryNameLast.PtId,
               finalExaminationInf2.InjuryNameLast.SinYm,
               finalExaminationInf2.InjuryNameLast.DataType,
               finalExaminationInf2.InjuryNameLast.SeqNo,
               finalExaminationInf2.InjuryNameLast.CodeNo,
               finalExaminationInf2.InjuryNameLast.RowNo,
               finalExaminationInf2.InjuryNameLast.Payload,
               finalExaminationInf2.FullByomei,
               finalExaminationInf2.InjuryNameLast.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.Icd10.PtId,
               finalExaminationInf2.Icd10.SinYm,
               finalExaminationInf2.Icd10.DataType,
               finalExaminationInf2.Icd10.SeqNo,
               finalExaminationInf2.Icd10.CodeNo,
               finalExaminationInf2.Icd10.RowNo,
               finalExaminationInf2.Icd10.Payload,
               finalExaminationInf2.Icd10.Value,
               finalExaminationInf2.Icd10.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.InjuryNameCode.PtId,
               finalExaminationInf2.InjuryNameCode.SinYm,
               finalExaminationInf2.InjuryNameCode.DataType,
               finalExaminationInf2.InjuryNameCode.SeqNo,
               finalExaminationInf2.InjuryNameCode.CodeNo,
               finalExaminationInf2.InjuryNameCode.RowNo,
               finalExaminationInf2.InjuryNameCode.Payload,
               finalExaminationInf2.ByomeiCd,
               finalExaminationInf2.InjuryNameCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.ModifierCode.PtId,
               finalExaminationInf2.ModifierCode.SinYm,
               finalExaminationInf2.ModifierCode.DataType,
               finalExaminationInf2.ModifierCode.SeqNo,
               finalExaminationInf2.ModifierCode.CodeNo,
               finalExaminationInf2.ModifierCode.RowNo,
               finalExaminationInf2.ModifierCode.Payload,
               valueModifierFinalExaminationInf2,
               finalExaminationInf2.ModifierCode.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.DateOfHospitalization.PtId,
               finalExaminationInf2.DateOfHospitalization.SinYm,
               finalExaminationInf2.DateOfHospitalization.DataType,
               finalExaminationInf2.DateOfHospitalization.SeqNo,
               finalExaminationInf2.DateOfHospitalization.CodeNo,
               finalExaminationInf2.DateOfHospitalization.RowNo,
               finalExaminationInf2.DateOfHospitalization.Payload,
               finalExaminationInf2.DateOfHospitalization.Value,
               finalExaminationInf2.DateOfHospitalization.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.DischargeDate.PtId,
               finalExaminationInf2.DischargeDate.SinYm,
               finalExaminationInf2.DischargeDate.DataType,
               finalExaminationInf2.DischargeDate.SeqNo,
               finalExaminationInf2.DischargeDate.CodeNo,
               finalExaminationInf2.DischargeDate.RowNo,
               finalExaminationInf2.DischargeDate.Payload,
               finalExaminationInf2.DischargeDate.Value,
               finalExaminationInf2.DischargeDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.Destination.PtId,
               finalExaminationInf2.Destination.SinYm,
               finalExaminationInf2.Destination.DataType,
               finalExaminationInf2.Destination.SeqNo,
               finalExaminationInf2.Destination.CodeNo,
               finalExaminationInf2.Destination.RowNo,
               finalExaminationInf2.Destination.Payload,
               finalExaminationInf2.Destination.Value,
               finalExaminationInf2.Destination.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.HouseCallDate.PtId,
               finalExaminationInf2.HouseCallDate.SinYm,
               finalExaminationInf2.HouseCallDate.DataType,
               finalExaminationInf2.HouseCallDate.SeqNo,
               finalExaminationInf2.HouseCallDate.CodeNo,
               finalExaminationInf2.HouseCallDate.RowNo,
               finalExaminationInf2.HouseCallDate.Payload,
               finalExaminationInf2.HouseCallDate.Value,
               finalExaminationInf2.HouseCallDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.MedicalInstitution.PtId,
               finalExaminationInf2.MedicalInstitution.SinYm,
               finalExaminationInf2.MedicalInstitution.DataType,
               finalExaminationInf2.MedicalInstitution.SeqNo,
               finalExaminationInf2.MedicalInstitution.CodeNo,
               finalExaminationInf2.MedicalInstitution.RowNo,
               finalExaminationInf2.MedicalInstitution.Payload,
               finalExaminationInf2.MedicalInstitution.Value,
               finalExaminationInf2.MedicalInstitution.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.StartDate.PtId,
               finalExaminationInf2.StartDate.SinYm,
               finalExaminationInf2.StartDate.DataType,
               finalExaminationInf2.StartDate.SeqNo,
               finalExaminationInf2.StartDate.CodeNo,
               finalExaminationInf2.StartDate.RowNo,
               finalExaminationInf2.StartDate.Payload,
               finalExaminationInf2.StartDate.Value,
               finalExaminationInf2.StartDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.OnsetDate.PtId,
               finalExaminationInf2.OnsetDate.SinYm,
               finalExaminationInf2.OnsetDate.DataType,
               finalExaminationInf2.OnsetDate.SeqNo,
               finalExaminationInf2.OnsetDate.CodeNo,
               finalExaminationInf2.OnsetDate.RowNo,
               finalExaminationInf2.OnsetDate.Payload,
               finalExaminationInf2.OnsetDate.Value,
               finalExaminationInf2.OnsetDate.IsDeleted
               ));

        result.Add(new Yousiki1InfDetailModel(
               finalExaminationInf2.MaximumNumberDate.PtId,
               finalExaminationInf2.MaximumNumberDate.SinYm,
               finalExaminationInf2.MaximumNumberDate.DataType,
               finalExaminationInf2.MaximumNumberDate.SeqNo,
               finalExaminationInf2.MaximumNumberDate.CodeNo,
               finalExaminationInf2.MaximumNumberDate.RowNo,
               finalExaminationInf2.MaximumNumberDate.Payload,
               finalExaminationInf2.MaximumNumberDate.Value,
               finalExaminationInf2.MaximumNumberDate.IsDeleted
               ));
        #endregion

        //ConvertTabRehabilitation
        #region
        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.Yousiki1InfDetails)
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

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.OutpatientConsultations)
        {
            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ConsultationDate.PtId,
                   yousiki1InfDetailRequest.ConsultationDate.SinYm,
                   yousiki1InfDetailRequest.ConsultationDate.DataType,
                   yousiki1InfDetailRequest.ConsultationDate.SeqNo,
                   yousiki1InfDetailRequest.ConsultationDate.CodeNo,
                   yousiki1InfDetailRequest.ConsultationDate.RowNo,
                   yousiki1InfDetailRequest.ConsultationDate.Payload,
                   yousiki1InfDetailRequest.ConsultationDate.Value,
                   yousiki1InfDetailRequest.ConsultationDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.FirstVisit.PtId,
                   yousiki1InfDetailRequest.FirstVisit.SinYm,
                   yousiki1InfDetailRequest.FirstVisit.DataType,
                   yousiki1InfDetailRequest.FirstVisit.SeqNo,
                   yousiki1InfDetailRequest.FirstVisit.CodeNo,
                   yousiki1InfDetailRequest.FirstVisit.RowNo,
                   yousiki1InfDetailRequest.FirstVisit.Payload,
                   yousiki1InfDetailRequest.FirstVisit.Value,
                   yousiki1InfDetailRequest.FirstVisit.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Referral.PtId,
                   yousiki1InfDetailRequest.Referral.SinYm,
                   yousiki1InfDetailRequest.Referral.DataType,
                   yousiki1InfDetailRequest.Referral.SeqNo,
                   yousiki1InfDetailRequest.Referral.CodeNo,
                   yousiki1InfDetailRequest.Referral.RowNo,
                   yousiki1InfDetailRequest.Referral.Payload,
                   yousiki1InfDetailRequest.Referral.Value,
                   yousiki1InfDetailRequest.Referral.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DepartmentCode.PtId,
                   yousiki1InfDetailRequest.DepartmentCode.SinYm,
                   yousiki1InfDetailRequest.DepartmentCode.DataType,
                   yousiki1InfDetailRequest.DepartmentCode.SeqNo,
                   yousiki1InfDetailRequest.DepartmentCode.CodeNo,
                   yousiki1InfDetailRequest.DepartmentCode.RowNo,
                   yousiki1InfDetailRequest.DepartmentCode.Payload,
                   yousiki1InfDetailRequest.DepartmentCode.Value,
                   yousiki1InfDetailRequest.DepartmentCode.IsDeleted
                   ));
        }

        foreach (var yousiki1InfDetailRequest in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.ByomeiRehabilitations)
        {
            var valueModifierByomeiRehabilitations = "";

            foreach (var value in yousiki1InfDetailRequest.PrefixSuffixList)
            {
                valueModifierByomeiRehabilitations = valueModifierByomeiRehabilitations + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ValueSelect.PtId,
                   yousiki1InfDetailRequest.ValueSelect.SinYm,
                   yousiki1InfDetailRequest.ValueSelect.DataType,
                   yousiki1InfDetailRequest.ValueSelect.SeqNo,
                   yousiki1InfDetailRequest.ValueSelect.CodeNo,
                   yousiki1InfDetailRequest.ValueSelect.RowNo,
                   yousiki1InfDetailRequest.ValueSelect.Payload,
                   yousiki1InfDetailRequest.ValueSelect.Value,
                   yousiki1InfDetailRequest.ValueSelect.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameLast.PtId,
                   yousiki1InfDetailRequest.InjuryNameLast.SinYm,
                   yousiki1InfDetailRequest.InjuryNameLast.DataType,
                   yousiki1InfDetailRequest.InjuryNameLast.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameLast.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameLast.RowNo,
                   yousiki1InfDetailRequest.InjuryNameLast.Payload,
                   yousiki1InfDetailRequest.FullByomei,
                   yousiki1InfDetailRequest.InjuryNameLast.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Icd10.PtId,
                   yousiki1InfDetailRequest.Icd10.SinYm,
                   yousiki1InfDetailRequest.Icd10.DataType,
                   yousiki1InfDetailRequest.Icd10.SeqNo,
                   yousiki1InfDetailRequest.Icd10.CodeNo,
                   yousiki1InfDetailRequest.Icd10.RowNo,
                   yousiki1InfDetailRequest.Icd10.Payload,
                   yousiki1InfDetailRequest.Icd10.Value,
                   yousiki1InfDetailRequest.Icd10.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.InjuryNameCode.PtId,
                   yousiki1InfDetailRequest.InjuryNameCode.SinYm,
                   yousiki1InfDetailRequest.InjuryNameCode.DataType,
                   yousiki1InfDetailRequest.InjuryNameCode.SeqNo,
                   yousiki1InfDetailRequest.InjuryNameCode.CodeNo,
                   yousiki1InfDetailRequest.InjuryNameCode.RowNo,
                   yousiki1InfDetailRequest.InjuryNameCode.Payload,
                   yousiki1InfDetailRequest.ByomeiCd,
                   yousiki1InfDetailRequest.InjuryNameCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.ModifierCode.PtId,
                   yousiki1InfDetailRequest.ModifierCode.SinYm,
                   yousiki1InfDetailRequest.ModifierCode.DataType,
                   yousiki1InfDetailRequest.ModifierCode.SeqNo,
                   yousiki1InfDetailRequest.ModifierCode.CodeNo,
                   yousiki1InfDetailRequest.ModifierCode.RowNo,
                   yousiki1InfDetailRequest.ModifierCode.Payload,
                   valueModifierByomeiRehabilitations,
                   yousiki1InfDetailRequest.ModifierCode.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DateOfHospitalization.PtId,
                   yousiki1InfDetailRequest.DateOfHospitalization.SinYm,
                   yousiki1InfDetailRequest.DateOfHospitalization.DataType,
                   yousiki1InfDetailRequest.DateOfHospitalization.SeqNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.CodeNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.RowNo,
                   yousiki1InfDetailRequest.DateOfHospitalization.Payload,
                   yousiki1InfDetailRequest.DateOfHospitalization.Value,
                   yousiki1InfDetailRequest.DateOfHospitalization.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.DischargeDate.PtId,
                   yousiki1InfDetailRequest.DischargeDate.SinYm,
                   yousiki1InfDetailRequest.DischargeDate.DataType,
                   yousiki1InfDetailRequest.DischargeDate.SeqNo,
                   yousiki1InfDetailRequest.DischargeDate.CodeNo,
                   yousiki1InfDetailRequest.DischargeDate.RowNo,
                   yousiki1InfDetailRequest.DischargeDate.Payload,
                   yousiki1InfDetailRequest.DischargeDate.Value,
                   yousiki1InfDetailRequest.DischargeDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.Destination.PtId,
                   yousiki1InfDetailRequest.Destination.SinYm,
                   yousiki1InfDetailRequest.Destination.DataType,
                   yousiki1InfDetailRequest.Destination.SeqNo,
                   yousiki1InfDetailRequest.Destination.CodeNo,
                   yousiki1InfDetailRequest.Destination.RowNo,
                   yousiki1InfDetailRequest.Destination.Payload,
                   yousiki1InfDetailRequest.Destination.Value,
                   yousiki1InfDetailRequest.Destination.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.HouseCallDate.PtId,
                   yousiki1InfDetailRequest.HouseCallDate.SinYm,
                   yousiki1InfDetailRequest.HouseCallDate.DataType,
                   yousiki1InfDetailRequest.HouseCallDate.SeqNo,
                   yousiki1InfDetailRequest.HouseCallDate.CodeNo,
                   yousiki1InfDetailRequest.HouseCallDate.RowNo,
                   yousiki1InfDetailRequest.HouseCallDate.Payload,
                   yousiki1InfDetailRequest.HouseCallDate.Value,
                   yousiki1InfDetailRequest.HouseCallDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MedicalInstitution.PtId,
                   yousiki1InfDetailRequest.MedicalInstitution.SinYm,
                   yousiki1InfDetailRequest.MedicalInstitution.DataType,
                   yousiki1InfDetailRequest.MedicalInstitution.SeqNo,
                   yousiki1InfDetailRequest.MedicalInstitution.CodeNo,
                   yousiki1InfDetailRequest.MedicalInstitution.RowNo,
                   yousiki1InfDetailRequest.MedicalInstitution.Payload,
                   yousiki1InfDetailRequest.MedicalInstitution.Value,
                   yousiki1InfDetailRequest.MedicalInstitution.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.StartDate.PtId,
                   yousiki1InfDetailRequest.StartDate.SinYm,
                   yousiki1InfDetailRequest.StartDate.DataType,
                   yousiki1InfDetailRequest.StartDate.SeqNo,
                   yousiki1InfDetailRequest.StartDate.CodeNo,
                   yousiki1InfDetailRequest.StartDate.RowNo,
                   yousiki1InfDetailRequest.StartDate.Payload,
                   yousiki1InfDetailRequest.StartDate.Value,
                   yousiki1InfDetailRequest.StartDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.OnsetDate.PtId,
                   yousiki1InfDetailRequest.OnsetDate.SinYm,
                   yousiki1InfDetailRequest.OnsetDate.DataType,
                   yousiki1InfDetailRequest.OnsetDate.SeqNo,
                   yousiki1InfDetailRequest.OnsetDate.CodeNo,
                   yousiki1InfDetailRequest.OnsetDate.RowNo,
                   yousiki1InfDetailRequest.OnsetDate.Payload,
                   yousiki1InfDetailRequest.OnsetDate.Value,
                   yousiki1InfDetailRequest.OnsetDate.IsDeleted
                   ));

            result.Add(new Yousiki1InfDetailModel(
                   yousiki1InfDetailRequest.MaximumNumberDate.PtId,
                   yousiki1InfDetailRequest.MaximumNumberDate.SinYm,
                   yousiki1InfDetailRequest.MaximumNumberDate.DataType,
                   yousiki1InfDetailRequest.MaximumNumberDate.SeqNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.CodeNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.RowNo,
                   yousiki1InfDetailRequest.MaximumNumberDate.Payload,
                   yousiki1InfDetailRequest.MaximumNumberDate.Value,
                   yousiki1InfDetailRequest.MaximumNumberDate.IsDeleted
                   ));
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
        foreach (var item in request.Yousiki1Inf.TabYousikiRequest.RehabilitationRequest.FIMs)
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
    private void UpdateCreateYuIchiFileStatus(CreateYuIchiFileStatus status)
    {
        string result = "\n" + JsonSerializer.Serialize(status);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.Write(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.Flush();
    }
    #endregion
}