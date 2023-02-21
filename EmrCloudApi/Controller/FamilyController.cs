using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Family;
using EmrCloudApi.Requests.Family;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Family;
using UseCase.Family.GetFamilyList;
using UseCase.Family.GetFamilyReverserList;
using UseCase.Family.SaveFamilyList;

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
    public ActionResult<Response<GetFamilyListResponse>> GetList([FromQuery] GetFamilyListRequest request)
    {
        var input = new GetFamilyListInputData(HpId, request.PtId, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetFamilyListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetFamilyListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.GetFamilyReverserList)]
    public ActionResult<Response<GetFamilyReverserListResponse>> GetListFamilyReverser([FromBody] GetFamilyReverserListRequest request)
    {
        var input = new GetFamilyReverserListInputData(HpId, request.FamilyPtId, request.DicPtInf);
        var output = _bus.Handle(input);

        var presenter = new GetFamilyReverserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetFamilyReverserListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveFamilyListResponse>> SaveList([FromBody] SaveFamilyListRequest request)
    {
        var listFamilyInputData = ConvertToFamilyInputItem(request.FamilyList);
        var input = new SaveFamilyListInputData(HpId, UserId, request.PtId, listFamilyInputData);
        var output = _bus.Handle(input);

        var presenter = new SaveFamilyListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveFamilyListResponse>>(presenter.Result);
    }

    private List<FamilyItem> ConvertToFamilyInputItem(List<FamilyRequestItem> listFamilyRequest)
    {
        var result = listFamilyRequest.Select(family => new FamilyItem(
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
                                                                                family.PtFamilyRekiList.Select(reki => new FamilyRekiItem(
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
