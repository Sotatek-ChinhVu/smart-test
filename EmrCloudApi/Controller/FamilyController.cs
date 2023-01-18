using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Family;
using EmrCloudApi.Requests.Family;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Family.GetListFamily;
using UseCase.Family.SaveListFamily;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class FamilyController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public FamilyController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetListFamilyResponse>> GetList([FromQuery] GetListFamilyRequest request)
    {
        var input = new GetListFamilyInputData(HpId, request.PtId, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetListFamilyPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListFamilyResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveListFamilyResponse>> SaveList([FromBody] SaveListFamilyRequest request)
    {
        var listFamilyInputData = ConvertToFamilyInputItem(request.ListFamily);
        var input = new SaveListFamilyInputData(HpId, UserId, request.PtId, listFamilyInputData);
        var output = _bus.Handle(input);

        var presenter = new SaveListFamilyPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListFamilyResponse>>(presenter.Result);
    }

    private List<FamilyInputItem> ConvertToFamilyInputItem(List<FamilyRequestItem> listFamilyRequest)
    {
        var result = listFamilyRequest.Select(family => new FamilyInputItem(
                                                                                family.FamilyId,
                                                                                family.PtId,
                                                                                family.ZokugaraCd,
                                                                                family.FamilyPtId,
                                                                                family.Name,
                                                                                family.KanaName,
                                                                                family.Sex,
                                                                                family.Birthday,
                                                                                family.IsDead,
                                                                                family.IsSeparated,
                                                                                family.Biko,
                                                                                family.SortNo,
                                                                                family.IsDeleted,
                                                                                family.ListPtFamilyReki.Select(reki => new FamilyRekiInputItem(
                                                                                                                                                    reki.Id,
                                                                                                                                                    reki.ByomeiCd,
                                                                                                                                                    reki.Byomei,
                                                                                                                                                    reki.Cmt,
                                                                                                                                                    reki.SortNo,
                                                                                                                                                    reki.IsDeleted
                                                                                                                                                )).ToList()
                                                                           )).ToList();
        return result;
    }
}
