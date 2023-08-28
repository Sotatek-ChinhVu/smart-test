using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.PatientManagement;
using EmrCloudApi.Requests.PatientManagement;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientManagement;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.PatientManagement;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class PatientManagementController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public PatientManagementController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.SearchPtInfs)]
        public ActionResult<Response<SearchPtInfsResponse>> GetList([FromBody] SearchPtInfsRequest request)
        {
            var input = new SearchPtInfsInputData(HpId, request.OutputOrder, request.PageIndex, request.PageCount, request.CoSta9000PtConf, request.CoSta9000HokenConf,
                                                   request.CoSta9000ByomeiConf, request.CoSta9000RaiinConf, request.CoSta9000SinConf, request.CoSta9000KarteConf, request.CoSta9000KensaConf);
            var output = _bus.Handle(input);

            var presenter = new SearchPtInfsPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchPtInfsResponse>>(presenter.Result);
        }
    }
}
