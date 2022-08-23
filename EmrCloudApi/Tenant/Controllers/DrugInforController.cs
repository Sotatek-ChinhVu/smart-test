using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.DrugInfor;
using EmrCloudApi.Tenant.Requests.DrugInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugInfor;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugInfor.Get;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public DrugInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetDrugInforResponse>> GetDiseaseListMedicalExamination([FromQuery] GetDrugInforRequest request)
        {
            var input = new GetDrugInforInputData(request.HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugInforResponse>>(presenter.Result);
        }
    }
}
