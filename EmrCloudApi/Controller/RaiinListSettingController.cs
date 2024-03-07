using Domain.Models.RaiinListMst;
using Domain.Models.RaiinListSetting;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.RaiinListSetting;
using EmrCloudApi.Requests.RaiinListSetting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinListSetting;
using Helper.Mapping;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinListSetting.GetDocCategory;
using UseCase.RaiinListSetting.GetFilingcategory;
using UseCase.RaiinListSetting.GetRaiiinListSetting;
using UseCase.RaiinListSetting.SaveRaiinListSetting;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class RaiinListSettingController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;

        public RaiinListSettingController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "DocCategoryRaiin")]
        public ActionResult<Response<GetDocCategoryRaiinResponse>> GetDocCategoryRaiin()
        {
            var input = new GetDocCategoryRaiinInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetDocCategoryRaiinPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDocCategoryRaiinResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "Filingcategory")]
        public ActionResult<Response<GetFilingcategoryResponse>> GetFilingcategory()
        {
            var input = new GetFilingcategoryInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetFilingcategoryPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetFilingcategoryResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "RaiinListSetting")]
        public ActionResult<Response<GetRaiiinListSettingResponse>> GetRaiinListSetting()
        {
            var input = new GetRaiiinListSettingInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetRaiiinListSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRaiiinListSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save + "RaiinListSetting")]
        public ActionResult<Response<SaveRaiinListSettingResponse>> SaveRaiinListSetting([FromBody] SaveRaiinListSettingRequest request)
        {
            var inputModel = request.RaiinListSettings
                            .Select(x => new RaiinListMstModel(x.GrpId,
                                                              x.GrpName,
                                                              x.SortNo,
                                                              x.IsDeleted,
                                                              x.RaiinListDetailsList.Select(d => new RaiinListDetailModel(d.GrpId,
                                                                                                                          d.KbnCd,
                                                                                                                          d.SortNo,
                                                                                                                          d.KbnName,
                                                                                                                          d.ColorCd,
                                                                                                                          d.IsDeleted,
                                                                                                                          d.IsOnlySwapSortNo,
                                                                                                                          d.RaiinListDoc.Select(doc => new RaiinListDocModel(doc.HpId,
                                                                                                                                                                             doc.GrpId,
                                                                                                                                                                             doc.KbnCd,
                                                                                                                                                                             doc.SeqNo,
                                                                                                                                                                             doc.CategoryCd,
                                                                                                                                                                             doc.CategoryName,
                                                                                                                                                                             doc.IsDeleted)).ToList(),
                                                                                                                          d.RaiinListItem.Select(item => new RaiinListItemModel(item.HpId,
                                                                                                                                                                                item.GrpId,
                                                                                                                                                                                item.KbnCd,
                                                                                                                                                                                item.ItemCd,
                                                                                                                                                                                item.SeqNo,
                                                                                                                                                                                item.InputName,
                                                                                                                                                                                item.IsExclude,
                                                                                                                                                                                item.IsAddNew,
                                                                                                                                                                                item.IsDeleted)).ToList(),
                                                                                                                          d.RaiinListFile.Select(file => new RaiinListFileModel(file.HpId,
                                                                                                                                                                                file.GrpId,
                                                                                                                                                                                file.KbnCd,
                                                                                                                                                                                file.CategoryCd,
                                                                                                                                                                                file.CategoryName,
                                                                                                                                                                                file.SeqNo,
                                                                                                                                                                                file.IsDeleted)).ToList(),
                                                                                                                          new KouiKbnCollectionModel(Mapper.Map(d.KouCollection.IKanModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ZaitakuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.NaifukuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TonpukuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.GaiyoModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.HikaKinchuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JochuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TentekiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.TachuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JikochuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ShochiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ShujutsuModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.MasuiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.KentaiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SeitaiModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SonohokaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.GazoModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.RihaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.SeishinModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.HoshaModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.ByoriModel, new RaiinListKouiModel()),
                                                                                                                                                     Mapper.Map(d.KouCollection.JihiModel, new RaiinListKouiModel())))).ToList()

                                                              )).ToList();

            var input = new SaveRaiinListSettingInputData(HpId, inputModel, UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveRaiinListSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveRaiinListSettingResponse>>(presenter.Result);
        }
    }
}
