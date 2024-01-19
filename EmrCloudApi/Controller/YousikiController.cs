using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Yousiki;
using EmrCloudApi.Requests.Yousiki;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Yousiki.AddYousiki;
using UseCase.Yousiki.GetVisitingInfs;
using UseCase.Yousiki.GetYousiki1InfDetails;
using UseCase.Yousiki.GetHistoryYousiki;
using UseCase.Yousiki.GetKacodeYousikiMstDict;
using UseCase.Yousiki.GetYousiki1InfModel;
using UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;
using UseCase.Yousiki.DeleteYousikiInf;
using UseCase.Yousiki.CreateYuIchiFile;
using Helper.Messaging;
using CreateYuIchiFileStatus = Helper.Messaging.Data.CreateYuIchiFileStatus;
using System.Text;
using System.Text.Json;

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
        var input = new DeleteYousikiInfInputData(HpId, UserId, request.SinYm, request.PtId, request.DataType);
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

    #region private function
    private void UpdateCreateYuIchiFileStatus(CreateYuIchiFileStatus status)
    {
        string result = "\n" + JsonSerializer.Serialize(status);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.FlushAsync();
    }
    #endregion
}