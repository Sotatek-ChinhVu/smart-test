using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemConf;
using EmrCloudApi.Requests.SystemConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;
using UseCase.SystemConf.Get;
using UseCase.SystemConf.GetDrugCheckSetting;
using UseCase.SystemConf.GetSystemConfForPrint;
using UseCase.SystemConf.GetSystemConfList;
using UseCase.SystemConf.SaveDrugCheckSetting;
using UseCase.SystemConf.SaveSystemSetting;
using UseCase.SystemConf.SystemSetting;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SystemConfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemConfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetSystemConfResponse>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            var input = new GetSystemConfInputData(HpId, request.GrpCd, request.GrpEdaNo);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSystemConfListResponse>> GetSystemConfList()
        {
            var input = new GetSystemConfListInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSystemConfForPrint)]
        public ActionResult<Response<GetSystemConfForPrintResponse>> GetSystemConfForPrint()
        {
            var input = new GetSystemConfForPrintInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfForPrintPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfForPrintResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDrugCheckSetting)]
        public ActionResult<Response<GetDrugCheckSettingResponse>> DrugCheckSetting()
        {
            var input = new GetDrugCheckSettingInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetDrugCheckSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugCheckSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSystemSetting)]
        public ActionResult<Response<GetSystemSettingResponse>> GetList([FromQuery] GetSystemSettingRequest request)
        {
            var input = new GetSystemSettingInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveDrugCheckSetting)]
        public ActionResult<Response<SaveDrugCheckSettingResponse>> SaveDrugCheckSetting([FromBody] SaveDrugCheckSettingRequest request)
        {
            var input = new SaveDrugCheckSettingInputData(HpId, UserId, ConvertToDrugCheckSettingItem(request));
            var output = _bus.Handle(input);

            var presenter = new SaveDrugCheckSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveDrugCheckSettingResponse>>(presenter.Result);
        }

        private DrugCheckSettingItem ConvertToDrugCheckSettingItem(SaveDrugCheckSettingRequest request)
        {
            return new DrugCheckSettingItem(
                       request.CheckDrugSameName,
                       request.StrainCheckSeibun,
                       request.StrainCheckPurodoragu,
                       request.StrainCheckRuiji,
                       request.StrainCheckKeito,
                       request.AgentCheckSetting,
                       request.DosageDrinkingDrugSetting,
                       request.DosageDrugAsOrderSetting,
                       request.DosageOtherDrugSetting,
                       request.DosageRatioSetting,
                       request.AllergyMedicineSeibun,
                       request.AllergyMedicinePurodoragu,
                       request.AllergyMedicineRuiji,
                       request.AllergyMedicineKeito,
                       request.FoodAllergyLevelSetting,
                       request.DiseaseLevelSetting,
                       request.KinkiLevelSetting,
                       request.DosageMinCheckSetting,
                       request.AgeLevelSetting);
        }

        [HttpPost(ApiPath.SaveSystemSetting)]
        public ActionResult<Response<SaveSystemSettingResponse>> SaveSystemSetting([FromBody] SaveSystemSettingRequest request)
        {
            var input = new SaveSystemSettingInputData(HpId, UserId, request.HpInfs, request.SystemConfMenus, request.IsUpdateHpInfo, request.IsUpdateSystemGenerationConf);
            var output = _bus.Handle(input);

            var presenter = new SaveSystemSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSystemSettingResponse>>(presenter.Result);
        }
    }
}
