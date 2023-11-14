using Domain.Models.SystemConf;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemConf;
using EmrCloudApi.Requests.SystemConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf;
using UseCase.SystemConf.Get;
using UseCase.SystemConf.GetDrugCheckSetting;
using UseCase.SystemConf.GetPathAll;
using UseCase.SystemConf.GetSystemConfForPrint;
using UseCase.SystemConf.GetSystemConfList;
using UseCase.SystemConf.GetXmlPath;
using UseCase.SystemConf.SaveDrugCheckSetting;
using UseCase.SystemConf.SavePath;
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
        public ActionResult<Response<GetSystemSettingResponse>> GetList()
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
            var input = new SaveSystemSettingInputData(HpId, UserId, request.HpInfs, request.SystemConfMenus, request.SanteiInfs, request.KensaCenters);
            var output = _bus.Handle(input);

            var presenter = new SaveSystemSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSystemSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSystemConfListXmlPath)]
        public ActionResult<Response<GetSystemConfListXmlPathResponse>> GetSystemConfListXmlPath([FromQuery] GetSystemConfListXmlPathRequest request)
        {
            var input = new GetSystemConfListXmlPathInputData(HpId, request.GrpCd, request.Machine, request.IsKensaIrai);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfListXmlPathPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfListXmlPathResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetAllPath)]
        public ActionResult<Response<GetAllPathResponse>> GetAllPath()
        {
            var input = new GetPathAllInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetAllPathPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAllPathResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePath)]
        public ActionResult<Response<SavePathResponse>> SavePath([FromBody] SavePathRequest request)
        {
            var input = new SavePathInputData(HpId, UserId, request.SystemConfListXmlPathModels.Select(p => new SystemConfListXmlPathModel(
                                HpId,
                                p.GrpCd,
                                0,
                                p.SeqNo,
                                p.Machine,
                                p.Path,
                                string.Empty,
                                p.Biko,
                                0,
                                1,
                                UserId,
                                DateTime.MinValue
                            )).ToList());
            var output = _bus.Handle(input);

            var presenter = new SavePathPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SavePathResponse>>(presenter.Result);
        }
    }
}
