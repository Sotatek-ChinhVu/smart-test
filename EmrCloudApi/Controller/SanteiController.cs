using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Santei;
using EmrCloudApi.Requests.Santei;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Santei;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Santei.GetListSanteiInf;
using UseCase.Santei.SaveListSanteiInf;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class SanteiController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public SanteiController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetListSanteiInfResponse>> GetList([FromQuery] GetListSanteiInfRequest request)
    {
        var input = new GetListSanteiInfInputData(HpId, request.PtId, request.SinDate, request.HokenPid);
        var output = _bus.Handle(input);

        var presenter = new GetListSanteiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListSanteiInfResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveListSanteiInfResponse>> SaveListSanteiInf([FromBody] SaveListSanteiInfRequest request)
    {
        var listSanteiInfs = ConvertToSanteiInfInputData(request.ListSanteiInfs);
        var input = new SaveListSanteiInfInputData(HpId, UserId, request.PtId, request.SinDate, request.HokenPid, listSanteiInfs);
        var output = _bus.Handle(input);

        var presenter = new SaveListSanteiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListSanteiInfResponse>>(presenter.Result);
    }

    private List<SanteiInfInputItem> ConvertToSanteiInfInputData(List<Requests.Santei.SanteiInfDto> listSanteiInfs)
    {
        return listSanteiInfs.Select(santeiInf =>
                                            new SanteiInfInputItem(
                                                    santeiInf.Id,
                                                    santeiInf.ItemCd,
                                                    santeiInf.AlertDays,
                                                    santeiInf.AlertTerm,
                                                    santeiInf.IsDeleted,
                                                    santeiInf.ListSanteInfDetails
                                                        .Select(santeiInfDeatail =>
                                                                    new SanteiInfDetailInputItem(
                                                                            santeiInfDeatail.Id,
                                                                            santeiInfDeatail.EndDate,
                                                                            santeiInfDeatail.KisanSbt,
                                                                            santeiInfDeatail.KisanDate,
                                                                            santeiInfDeatail.Byomei,
                                                                            santeiInfDeatail.HosokuComment,
                                                                            santeiInfDeatail.Comment,
                                                                            santeiInfDeatail.IsDeleted
                                                         )).ToList()
                                                )).ToList();
    }
}
