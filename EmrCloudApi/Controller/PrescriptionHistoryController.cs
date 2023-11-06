using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugInfor.GetSinrekiFilterMstList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class PrescriptionHistoryController : AuthorizeControllerBase
{

    private readonly UseCaseBus _bus;
    public PrescriptionHistoryController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetSinrekiFilterMstList)]
    public ActionResult<Response<GetSinrekiFilterMstListResponse>> GetList()
    {
        var input = new GetSinrekiFilterMstListInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetSinrekiFilterMstListPresenter();
        presenter.Complete(output);

        var result = Ok(presenter.Result);
        return result;
    }
}
