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

    [HttpPost(ApiPath.UpdateYosiki)]
    public ActionResult<Response<UpdateYosikiResponse>> UpdateYosiki([FromBody] UpdateYosikiRequest request)
    {
        List<Yousiki1InfDetailModel> totalRequest = new();
        var commonModelRequests = ConvertTabCommonModelToYousiki1InfDetail(request.CommonModelRequest);
        totalRequest.AddRange(commonModelRequests);
        var livingHabitModelRequests = ConvertTabLivingHabitModelToYousiki1InfDetail(request.LivingHabitModelRequest);
        totalRequest.AddRange(livingHabitModelRequests);
        var atHomeModelRequests = ConvertTabAtHomeModelToYousiki1InfDetail(request.AtHomeModelRequest, request.Yousiki1InfModel);
        totalRequest.AddRange(atHomeModelRequests);
        var rehabilitationModelRequests = ConvertTabRehabilitationModelToYousiki1InfDetail(request.RehabilitationModelRequest, request.Yousiki1InfModel);
        totalRequest.AddRange(rehabilitationModelRequests);


        var input = new UpdateYosikiInputData(HpId, UserId, ConvertModelToYousiki1Inf(request.Yousiki1InfModel), totalRequest, request.DataTypes, request.IsTemporarySave);
        var output = _bus.Handle(input);
        var presenter = new UpdateYosikiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<UpdateYosikiResponse>>(presenter.Result);
    }

    private List<Yousiki1InfDetailModel> ConvertTabRehabilitationModelToYousiki1InfDetail(RehabilitationModelRequest items, UpdateYosikiInfRequestItem yosikiInfRequestItem)
    {
        List<Yousiki1InfDetailModel> result = new();
        var barthelIndexValue = "";
        foreach (var item in items.BarthelIndexLists)
        {
            barthelIndexValue = barthelIndexValue + item.StatusValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            yosikiInfRequestItem.PtId,
            yosikiInfRequestItem.SinYm,
            3,
            yosikiInfRequestItem.SeqNo,
            "RPADL01",
            0,
            1,
            barthelIndexValue
            )
            );

        var fimListValue = "";
        foreach (var item in items.FimLists)
        {
            fimListValue = fimListValue + item.StatusValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            yosikiInfRequestItem.PtId,
            yosikiInfRequestItem.SinYm,
            3,
            yosikiInfRequestItem.SeqNo,
            "RPADL01",
            0,
            2,
            fimListValue
            )
            );

        foreach (var item in items.OutpatientConsultationList)
        {
            foreach (var outpatientConsultationModel in item.OutpatientConsultationModel)
            {
                result.Add(new Yousiki1InfDetailModel(
                outpatientConsultationModel.PtId,
                outpatientConsultationModel.SinYm,
                outpatientConsultationModel.DataType,
                outpatientConsultationModel.SeqNo,
                outpatientConsultationModel.CodeNo,
                outpatientConsultationModel.RowNo,
                outpatientConsultationModel.Payload,
                outpatientConsultationModel.Value,
                outpatientConsultationModel.IsDeleted
            ));
            }
        }

        foreach (var item in items.ByomeiRehabilitationList)
        {
            foreach (var byomeiRehabilitationItem in item.CommonForm1ModelRequestItems)
            {
                result.Add(new Yousiki1InfDetailModel(
                byomeiRehabilitationItem.PtId,
                byomeiRehabilitationItem.SinYm,
                byomeiRehabilitationItem.DataType,
                byomeiRehabilitationItem.SeqNo,
                byomeiRehabilitationItem.CodeNo,
                byomeiRehabilitationItem.RowNo,
                byomeiRehabilitationItem.Payload,
                byomeiRehabilitationItem.Value,
                byomeiRehabilitationItem.IsDeleted
            ));
            }

            foreach (var diagnosticInjuryListRequestItem in item.DiagnosticInjuryListRequestItems)
            {
                var valueModifierCode = "";

                foreach (var value in diagnosticInjuryListRequestItem.PrefixSuffixRequests)
                {
                    valueModifierCode = valueModifierCode + value.Code;
                }

                result.Add(new Yousiki1InfDetailModel(
                    diagnosticInjuryListRequestItem.InjuryNameLast.PtId,
                    diagnosticInjuryListRequestItem.InjuryNameLast.SinYm,
                    diagnosticInjuryListRequestItem.InjuryNameLast.DataType,
                    diagnosticInjuryListRequestItem.InjuryNameLast.SeqNo,
                    diagnosticInjuryListRequestItem.InjuryNameLast.CodeNo,
                    diagnosticInjuryListRequestItem.InjuryNameLast.RowNo,
                    diagnosticInjuryListRequestItem.InjuryNameLast.Payload,
                    diagnosticInjuryListRequestItem.FullByomeiRequest,
                    diagnosticInjuryListRequestItem.InjuryNameLast.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    diagnosticInjuryListRequestItem.InjuryNameCode.PtId,
                    diagnosticInjuryListRequestItem.InjuryNameCode.SinYm,
                    diagnosticInjuryListRequestItem.InjuryNameCode.DataType,
                    diagnosticInjuryListRequestItem.InjuryNameCode.SeqNo,
                    diagnosticInjuryListRequestItem.InjuryNameCode.CodeNo,
                    diagnosticInjuryListRequestItem.InjuryNameCode.RowNo,
                    diagnosticInjuryListRequestItem.InjuryNameCode.Payload,
                    diagnosticInjuryListRequestItem.ByomeiCdRequest,
                    diagnosticInjuryListRequestItem.InjuryNameCode.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    diagnosticInjuryListRequestItem.ModifierCode.PtId,
                    diagnosticInjuryListRequestItem.ModifierCode.SinYm,
                    diagnosticInjuryListRequestItem.ModifierCode.DataType,
                    diagnosticInjuryListRequestItem.ModifierCode.SeqNo,
                    diagnosticInjuryListRequestItem.ModifierCode.CodeNo,
                    diagnosticInjuryListRequestItem.ModifierCode.RowNo,
                    diagnosticInjuryListRequestItem.ModifierCode.Payload,
                    valueModifierCode,
                    diagnosticInjuryListRequestItem.ModifierCode.IsDeleted)
                    );
            }

            foreach (var hospitalizationStatusListRequestItem in item.HospitalizationStatusInf)
            {
                var valueModifierCode = "";

                foreach (var value in hospitalizationStatusListRequestItem.PrefixSuffixRequests)
                {
                    valueModifierCode = valueModifierCode + value.Code;
                }

                result.Add(new Yousiki1InfDetailModel(
                    hospitalizationStatusListRequestItem.InjuryNameLast.PtId,
                    hospitalizationStatusListRequestItem.InjuryNameLast.SinYm,
                    hospitalizationStatusListRequestItem.InjuryNameLast.DataType,
                    hospitalizationStatusListRequestItem.InjuryNameLast.SeqNo,
                    hospitalizationStatusListRequestItem.InjuryNameLast.CodeNo,
                    hospitalizationStatusListRequestItem.InjuryNameLast.RowNo,
                    hospitalizationStatusListRequestItem.InjuryNameLast.Payload,
                    hospitalizationStatusListRequestItem.FullByomeiRequest,
                    hospitalizationStatusListRequestItem.InjuryNameLast.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    hospitalizationStatusListRequestItem.InjuryNameCode.PtId,
                    hospitalizationStatusListRequestItem.InjuryNameCode.SinYm,
                    hospitalizationStatusListRequestItem.InjuryNameCode.DataType,
                    hospitalizationStatusListRequestItem.InjuryNameCode.SeqNo,
                    hospitalizationStatusListRequestItem.InjuryNameCode.CodeNo,
                    hospitalizationStatusListRequestItem.InjuryNameCode.RowNo,
                    hospitalizationStatusListRequestItem.InjuryNameCode.Payload,
                    hospitalizationStatusListRequestItem.ByomeiCdRequest,
                    hospitalizationStatusListRequestItem.InjuryNameCode.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    hospitalizationStatusListRequestItem.ModifierCode.PtId,
                    hospitalizationStatusListRequestItem.ModifierCode.SinYm,
                    hospitalizationStatusListRequestItem.ModifierCode.DataType,
                    hospitalizationStatusListRequestItem.ModifierCode.SeqNo,
                    hospitalizationStatusListRequestItem.ModifierCode.CodeNo,
                    hospitalizationStatusListRequestItem.ModifierCode.RowNo,
                    hospitalizationStatusListRequestItem.ModifierCode.Payload,
                    valueModifierCode,
                    hospitalizationStatusListRequestItem.ModifierCode.IsDeleted)
                    );
            }

            foreach (var finalExaminationInfRequestItem in item.FinalExaminationInf)
            {
                var valueModifierCode = "";

                foreach (var value in finalExaminationInfRequestItem.PrefixSuffixRequests)
                {
                    valueModifierCode = valueModifierCode + value.Code;
                }

                result.Add(new Yousiki1InfDetailModel(
                    finalExaminationInfRequestItem.InjuryNameLast.PtId,
                    finalExaminationInfRequestItem.InjuryNameLast.SinYm,
                    finalExaminationInfRequestItem.InjuryNameLast.DataType,
                    finalExaminationInfRequestItem.InjuryNameLast.SeqNo,
                    finalExaminationInfRequestItem.InjuryNameLast.CodeNo,
                    finalExaminationInfRequestItem.InjuryNameLast.RowNo,
                    finalExaminationInfRequestItem.InjuryNameLast.Payload,
                    finalExaminationInfRequestItem.FullByomeiRequest,
                    finalExaminationInfRequestItem.InjuryNameLast.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    finalExaminationInfRequestItem.InjuryNameCode.PtId,
                    finalExaminationInfRequestItem.InjuryNameCode.SinYm,
                    finalExaminationInfRequestItem.InjuryNameCode.DataType,
                    finalExaminationInfRequestItem.InjuryNameCode.SeqNo,
                    finalExaminationInfRequestItem.InjuryNameCode.CodeNo,
                    finalExaminationInfRequestItem.InjuryNameCode.RowNo,
                    finalExaminationInfRequestItem.InjuryNameCode.Payload,
                    finalExaminationInfRequestItem.ByomeiCdRequest,
                    finalExaminationInfRequestItem.InjuryNameCode.IsDeleted));

                result.Add(new Yousiki1InfDetailModel(
                    finalExaminationInfRequestItem.ModifierCode.PtId,
                    finalExaminationInfRequestItem.ModifierCode.SinYm,
                    finalExaminationInfRequestItem.ModifierCode.DataType,
                    finalExaminationInfRequestItem.ModifierCode.SeqNo,
                    finalExaminationInfRequestItem.ModifierCode.CodeNo,
                    finalExaminationInfRequestItem.ModifierCode.RowNo,
                    finalExaminationInfRequestItem.ModifierCode.Payload,
                    valueModifierCode,
                    finalExaminationInfRequestItem.ModifierCode.IsDeleted)
                    );
            }
        }

        return result;
    }

    private List<Yousiki1InfDetailModel> ConvertTabAtHomeModelToYousiki1InfDetail(AtHomeModelRequest items, UpdateYosikiInfRequestItem yosikiInfRequestItem)
    {
        List<Yousiki1InfDetailModel> result = new();

        foreach (var finalExaminationInfRequestItem in items.FinalExaminationInf)
        {
            var valueModifierCode = "";

            foreach (var value in finalExaminationInfRequestItem.PrefixSuffixRequests)
            {
                valueModifierCode = valueModifierCode + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.InjuryNameLast.PtId,
                finalExaminationInfRequestItem.InjuryNameLast.SinYm,
                finalExaminationInfRequestItem.InjuryNameLast.DataType,
                finalExaminationInfRequestItem.InjuryNameLast.SeqNo,
                finalExaminationInfRequestItem.InjuryNameLast.CodeNo,
                finalExaminationInfRequestItem.InjuryNameLast.RowNo,
                finalExaminationInfRequestItem.InjuryNameLast.Payload,
                finalExaminationInfRequestItem.FullByomeiRequest,
                finalExaminationInfRequestItem.InjuryNameLast.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.InjuryNameCode.PtId,
                finalExaminationInfRequestItem.InjuryNameCode.SinYm,
                finalExaminationInfRequestItem.InjuryNameCode.DataType,
                finalExaminationInfRequestItem.InjuryNameCode.SeqNo,
                finalExaminationInfRequestItem.InjuryNameCode.CodeNo,
                finalExaminationInfRequestItem.InjuryNameCode.RowNo,
                finalExaminationInfRequestItem.InjuryNameCode.Payload,
                finalExaminationInfRequestItem.ByomeiCdRequest,
                finalExaminationInfRequestItem.InjuryNameCode.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.ModifierCode.PtId,
                finalExaminationInfRequestItem.ModifierCode.SinYm,
                finalExaminationInfRequestItem.ModifierCode.DataType,
                finalExaminationInfRequestItem.ModifierCode.SeqNo,
                finalExaminationInfRequestItem.ModifierCode.CodeNo,
                finalExaminationInfRequestItem.ModifierCode.RowNo,
                finalExaminationInfRequestItem.ModifierCode.Payload,
                valueModifierCode,
                finalExaminationInfRequestItem.ModifierCode.IsDeleted)
                );
        }

        foreach (var finalExaminationInfRequestItem in items.FinalExaminationInf2)
        {
            var valueModifierCode = "";

            foreach (var value in finalExaminationInfRequestItem.PrefixSuffixRequests)
            {
                valueModifierCode = valueModifierCode + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.InjuryNameLast.PtId,
                finalExaminationInfRequestItem.InjuryNameLast.SinYm,
                finalExaminationInfRequestItem.InjuryNameLast.DataType,
                finalExaminationInfRequestItem.InjuryNameLast.SeqNo,
                finalExaminationInfRequestItem.InjuryNameLast.CodeNo,
                finalExaminationInfRequestItem.InjuryNameLast.RowNo,
                finalExaminationInfRequestItem.InjuryNameLast.Payload,
                finalExaminationInfRequestItem.FullByomeiRequest,
                finalExaminationInfRequestItem.InjuryNameLast.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.InjuryNameCode.PtId,
                finalExaminationInfRequestItem.InjuryNameCode.SinYm,
                finalExaminationInfRequestItem.InjuryNameCode.DataType,
                finalExaminationInfRequestItem.InjuryNameCode.SeqNo,
                finalExaminationInfRequestItem.InjuryNameCode.CodeNo,
                finalExaminationInfRequestItem.InjuryNameCode.RowNo,
                finalExaminationInfRequestItem.InjuryNameCode.Payload,
                finalExaminationInfRequestItem.ByomeiCdRequest,
                finalExaminationInfRequestItem.InjuryNameCode.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                finalExaminationInfRequestItem.ModifierCode.PtId,
                finalExaminationInfRequestItem.ModifierCode.SinYm,
                finalExaminationInfRequestItem.ModifierCode.DataType,
                finalExaminationInfRequestItem.ModifierCode.SeqNo,
                finalExaminationInfRequestItem.ModifierCode.CodeNo,
                finalExaminationInfRequestItem.ModifierCode.RowNo,
                finalExaminationInfRequestItem.ModifierCode.Payload,
                valueModifierCode,
                finalExaminationInfRequestItem.ModifierCode.IsDeleted)
                );
        }

        foreach (var item in items.HospitalizationStatusList)
        {

            var valueModifierCode = "";

            foreach (var value in item.PrefixSuffixRequests)
            {
                valueModifierCode = valueModifierCode + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
            item.InjuryNameLast.PtId,
            item.InjuryNameLast.SinYm,
            item.InjuryNameLast.DataType,
            item.InjuryNameLast.SeqNo,
            item.InjuryNameLast.CodeNo,
            item.InjuryNameLast.RowNo,
            item.InjuryNameLast.Payload,
            item.FullByomeiRequest,
            item.InjuryNameLast.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.InjuryNameCode.PtId,
                item.InjuryNameCode.SinYm,
                item.InjuryNameCode.DataType,
                item.InjuryNameCode.SeqNo,
                item.InjuryNameCode.CodeNo,
                item.InjuryNameCode.RowNo,
                item.InjuryNameCode.Payload,
                item.ByomeiCdRequest,
                item.InjuryNameCode.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.ModifierCode.PtId,
                item.ModifierCode.SinYm,
                item.ModifierCode.DataType,
                item.ModifierCode.SeqNo,
                item.ModifierCode.CodeNo,
                item.ModifierCode.RowNo,
                item.ModifierCode.Payload,
                valueModifierCode,
                item.ModifierCode.IsDeleted)
                );
        }

        foreach (var item in items.StatusVisitList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.StatusVisitNursingList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.StatusEmergencyConsultationList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.StatusShortTermAdmissionList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.StatusHomeVisitList)
        {
            var valueModifierCode = "";

            foreach (var value in item.PrefixSuffixRequests)
            {
                valueModifierCode = valueModifierCode + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
            item.InjuryNameLast.PtId,
            item.InjuryNameLast.SinYm,
            item.InjuryNameLast.DataType,
            item.InjuryNameLast.SeqNo,
            item.InjuryNameLast.CodeNo,
            item.InjuryNameLast.RowNo,
            item.InjuryNameLast.Payload,
            item.FullByomeiRequest,
            item.InjuryNameLast.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.InjuryNameCode.PtId,
                item.InjuryNameCode.SinYm,
                item.InjuryNameCode.DataType,
                item.InjuryNameCode.SeqNo,
                item.InjuryNameCode.CodeNo,
                item.InjuryNameCode.RowNo,
                item.InjuryNameCode.Payload,
                item.ByomeiCdRequest,
                item.InjuryNameCode.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.ModifierCode.PtId,
                item.ModifierCode.SinYm,
                item.ModifierCode.DataType,
                item.ModifierCode.SeqNo,
                item.ModifierCode.CodeNo,
                item.ModifierCode.RowNo,
                item.ModifierCode.Payload,
                valueModifierCode,
                item.ModifierCode.IsDeleted)
                );
        }

        var patientSitutationListValue = "";

        foreach (var patientSitutation in items.PatientSitutationListRequestItems)
        {
            patientSitutationListValue = patientSitutationListValue + patientSitutation.SituationValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            yosikiInfRequestItem.PtId,
            yosikiInfRequestItem.SinYm,
            2,
            yosikiInfRequestItem.SeqNo,
            "HPS0001",
            0,
            1,
            patientSitutationListValue));

        var barthelIndexListRequestItemsValue = "";

        foreach (var barthelIndexListRequestItem in items.BarthelIndexListRequestItems)
        {
            barthelIndexListRequestItemsValue = barthelIndexListRequestItemsValue + barthelIndexListRequestItem.StatusValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            yosikiInfRequestItem.PtId,
            yosikiInfRequestItem.SinYm,
            2,
            yosikiInfRequestItem.SeqNo,
            "HPS0002",
            0,
            1,
            patientSitutationListValue));

        var statusNurtritionListValue = "";

        foreach (var statusNurtritionList in items.StatusNurtritionListRequestIItems)
        {
            statusNurtritionListValue = statusNurtritionListValue + statusNurtritionList.PointValue;
        }

        result.Add(new Yousiki1InfDetailModel(
            yosikiInfRequestItem.PtId,
            yosikiInfRequestItem.SinYm,
            2,
            yosikiInfRequestItem.SeqNo,
            "HPS0006",
            0,
            3,
            patientSitutationListValue));

        return result;
    }

    private List<Yousiki1InfDetailModel> ConvertTabLivingHabitModelToYousiki1InfDetail(LivingHabitModelRequest items)
    {
        List<Yousiki1InfDetailModel> result = new();

        foreach (var item in items.OutpatientConsultationInfList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.StrokeHistoryList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.AcuteCoronaryHistoryList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.AcuteAorticHistoryList)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        return result;
    }

    private List<Yousiki1InfDetailModel> ConvertTabCommonModelToYousiki1InfDetail(CommonForm1ModelRequest items)
    {
        List<Yousiki1InfDetailModel> result = new();

        foreach (var item in items.CommonForm1ModelRequestItems)
        {
            result.Add(new Yousiki1InfDetailModel(
                item.PtId,
                item.SinYm,
                item.DataType,
                item.SeqNo,
                item.CodeNo,
                item.RowNo,
                item.Payload,
                item.Value,
                item.IsDeleted
            ));
        }

        foreach (var item in items.DiagnosticInjuryListRequestItems)
        {
            var valueModifierCode = "";

            foreach (var value in item.PrefixSuffixRequests)
            {
                valueModifierCode = valueModifierCode + value.Code;
            }

            result.Add(new Yousiki1InfDetailModel(
                item.InjuryNameLast.PtId,
                item.InjuryNameLast.SinYm,
                item.InjuryNameLast.DataType,
                item.InjuryNameLast.SeqNo,
                item.InjuryNameLast.CodeNo,
                item.InjuryNameLast.RowNo,
                item.InjuryNameLast.Payload,
                item.FullByomeiRequest,
                item.InjuryNameLast.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.InjuryNameCode.PtId,
                item.InjuryNameCode.SinYm,
                item.InjuryNameCode.DataType,
                item.InjuryNameCode.SeqNo,
                item.InjuryNameCode.CodeNo,
                item.InjuryNameCode.RowNo,
                item.InjuryNameCode.Payload,
                item.ByomeiCdRequest,
                item.InjuryNameCode.IsDeleted));

            result.Add(new Yousiki1InfDetailModel(
                item.ModifierCode.PtId,
                item.ModifierCode.SinYm,
                item.ModifierCode.DataType,
                item.ModifierCode.SeqNo,
                item.ModifierCode.CodeNo,
                item.ModifierCode.RowNo,
                item.ModifierCode.Payload,
                valueModifierCode,
                item.ModifierCode.IsDeleted)
                );
        }

        return result;
    }

    private Yousiki1InfModel ConvertModelToYousiki1Inf(UpdateYosikiInfRequestItem item)
    {
        return new Yousiki1InfModel(HpId, item.PtId, item.SinYm, item.DataType, item.SeqNo, item.IsDeleted, item.IsDeleted);
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