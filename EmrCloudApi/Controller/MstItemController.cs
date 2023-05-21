using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MstItem;
using EmrCloudApi.Requests.MstItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses.MstItem.DiseaseSearch;
using EmrCloudApi.Services;
using Helper.Extension;
using Helper.Mapping;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MstItem.CheckIsTenMstUsed;
using UseCase.MstItem.DeleteOrRecoverTenMst;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetAdoptedItemList;
using UseCase.MstItem.GetCmtCheckMstList;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.GetJihiSbtMstList;
using UseCase.MstItem.GetListDrugImage;
using UseCase.MstItem.GetListTenMstOrigin;
using UseCase.MstItem.GetRenkeiMst;
using UseCase.MstItem.GetSelectiveComment;
using UseCase.MstItem.GetSetDataTenMst;
using UseCase.MstItem.GetTenMstListByItemType;
using UseCase.MstItem.GetTenMstOriginInfoCreate;
using UseCase.MstItem.SaveSetDataTenMst;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.SearchTenMstItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.MstItem.UpdateAdoptedItemList;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class MstItemController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public MstItemController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.GetDosageDrugList)]
        public ActionResult<Response<GetDosageDrugListResponse>> GetDosageDrugList([FromBody] GetDosageDrugListRequest request)
        {
            var input = new GetDosageDrugListInputData(request.YjCds);
            var output = _bus.Handle(input);

            var presenter = new GetDosageDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDosageDrugListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetFoodAlrgy)]
        public ActionResult<Response<GetFoodAlrgyMasterDataResponse>> GetFoodAlrgy()
        {
            var input = new GetFoodAlrgyInputData();
            var output = _bus.Handle(input);

            var presenter = new FoodAlrgyMasterDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetFoodAlrgyMasterDataResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchOTC)]
        public ActionResult<Response<SearchOTCResponse>> SearchOTC([FromBody] SearchOTCRequest request)
        {
            var input = new SearchOTCInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchOTCPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchOTCResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchSupplement)]
        public ActionResult<Response<SearchSupplementResponse>> SearchSupplement([FromBody] SearchSupplementRequest request)
        {
            var input = new SearchSupplementInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchSupplementPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchSupplementResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchTenItem)]
        public ActionResult<Response<SearchTenItemResponse>> SearchTenItem([FromBody] SearchTenItemRequest request)
        {
            var input = new SearchTenItemInputData(request.Keyword, request.KouiKbn, request.SinDate, request.PageIndex, request.PageCount, request.GenericOrSameItem, request.YJCd, HpId, request.PointFrom, request.PointTo, request.IsRosai, request.IsMirai, request.IsExpired, request.ItemCodeStartWith, request.IsMasterSearch, request.IsSearch831SuffixOnly, request.IsSearchSanteiItem, request.SearchFollowUsage, request.IsDeleted, request.KouiKbns, request.DrugKbns, request.MasterSBT);
            var output = _bus.Handle(input);
            var presenter = new SearchTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedInputItem)]
        public ActionResult<Response<UpdateAdoptedTenItemResponse>> UpdateAdoptedInputItem([FromBody] UpdateAdoptedTenItemRequest request)
        {
            var input = new UpdateAdoptedTenItemInputData(request.ValueAdopted, request.ItemCdInputItem, request.StartDateInputItem, HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.DiseaseSearch)]
        public ActionResult<Response<DiseaseSearchResponse>> DiseaseSearch([FromQuery] DiseaseSearchRequest request)
        {
            var input = new DiseaseSearchInputData(request.IsPrefix, request.IsByomei, request.IsSuffix, request.IsMisaiyou, request.Sindate, request.Keyword, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new DiseaseSearchPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<DiseaseSearchResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedByomei)]
        public ActionResult<Response<UpdateAdoptedTenItemResponse>> UpdateAdoptedByomei([FromBody] UpdateAdoptedByomeiRequest request)
        {
            var input = new UpdateAdoptedByomeiInputData(HpId, request.ByomeiCd, UserId);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedByomeiPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.SearchPostCode)]
        public ActionResult<Response<SearchPostCodeRespone>> GetList([FromQuery] SearchPostCodeRequest request)
        {
            var input = new SearchPostCodeInputData(HpId, request.PostCode1, request.PostCode2, request.Address, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);
            var presenter = new SearchPostCodePresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.FindTenMst)]
        public ActionResult<Response<FindtenMstResponse>> FindTenMst([FromQuery] FindTenMstRequest request)
        {
            var input = new FindTenMstInputData(HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new FindTenMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.GetAdoptedItemList)]
        public ActionResult<Response<GetAdoptedItemListResponse>> GetAdoptedItemList([FromBody] GetAdoptedItemListRequest request)
        {
            var input = new GetAdoptedItemListInputData(request.ItemCds, request.SinDate, HpId);
            var output = _bus.Handle(input);
            var presenter = new GetAdoptedItemListPresenter();
            presenter.Complete(output);

            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedItemList)]
        public ActionResult<Response<UpdateAdoptedItemListResponse>> UpdateAdoptedItemList([FromBody] UpdateAdoptedItemListRequest request)
        {
            var input = new UpdateAdoptedItemListInputData(request.ValueAdopted, request.ItemCds, request.SinDate, HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedItemListPresenter();
            presenter.Complete(output);

            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetSelectiveComment)]
        public ActionResult<Response<GetSelectiveCommentResponse>> GetSelectiveComment([FromQuery] GetSelectiveCommentRequest request)
        {
            var input = new GetSelectiveCommentInputData(HpId, request.ItemCds.Trim().Split(",").ToList(), request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetSelectiveCommentPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.GetCmtCheckMstList)]
        public ActionResult<Response<GetCmtCheckMstListResponse>> GetCmtCheckMstList([FromBody] GetCmtCheckMstListRequest request)
        {
            var input = new GetCmtCheckMstListInputData(HpId, UserId, request.ItemCds);
            var output = _bus.Handle(input);
            var presenter = new GetCmtCheckMstListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetListTenMstOrigin)]
        public ActionResult<Response<GetListTenMstOriginResponse>> GetListTenMstOrigin([FromQuery] GetListTenMstOriginRequest request)
        {
            var input = new GetListTenMstOriginInputData(request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new GetListTenMstOriginPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListTenMstOriginResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTenMstOriginInfoCreate)]
        public ActionResult<Response<GetTenMstOriginInfoCreateResponse>> GetTenMstOriginInfoCreate([FromQuery] GetTenMstOriginInfoCreateRequest request)
        {
            var input = new GetTenMstOriginInfoCreateInputData(request.Type, HpId);
            var output = _bus.Handle(input);
            var presenter = new GetTenMstOriginInfoCreatePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenMstOriginInfoCreateResponse>>(presenter.Result);
        }

        /// <summary>
        /// only pass ItemMst Is Modified;
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(ApiPath.DeleteOrRecoverTenMst)]
        public ActionResult<Response<DeleteOrRecoverTenMstResponse>> DeleteOrRecoverTenMst([FromBody] DeleteOrRecoverTenMstRequest request)
        {
            var input = new DeleteOrRecoverTenMstInputData(request.ItemCd, request.SelectedTenMstModelName, request.Mode, Mapper.Map<TenMstOriginModelDto, TenMstOriginModel>(request.TenMsts), UserId, HpId, request.ConfirmDeleteIfModeIsDeleted);
            var output = _bus.Handle(input);
            var presenter = new DeleteOrRecoverTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<DeleteOrRecoverTenMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSetDataTenMst)]
        public ActionResult<Response<GetSetDataTenMstResponse>> GetSetDataTenMstOrigin([FromQuery] GetSetDataTenMstRequest request)
        {
            var input = new GetSetDataTenMstInputData(HpId,
                                                     request.SinDate,
                                                     request.ItemCd,
                                                     request.JiCd ?? string.Empty,
                                                     request.IpnNameCd ?? string.Empty,
                                                     request.SanteiItemCd ?? string.Empty,
                                                     request.AgekasanCd1Note ?? string.Empty,
                                                     request.AgekasanCd2Note ?? string.Empty,
                                                     request.AgekasanCd3Note ?? string.Empty,
                                                     request.AgekasanCd4Note ?? string.Empty);
            var output = _bus.Handle(input);
            var presenter = new GetSetDataTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetSetDataTenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveSetDataTenMst)]
        public ActionResult<Response<SaveSetDataTenMstResponse>> SaveSetDataTenMst([FromBody] SaveSetDataTenMstRequest request)
        {
            List<TenMstOriginModel> tenOrigins = Mapper.Map<TenMstOriginModelDto, TenMstOriginModel>(request.TenOrigins);

            BasicSettingTabModel basicSettingTab = new BasicSettingTabModel(Mapper.Map<CmtKbnMstModelDto, CmtKbnMstModel>(request.CmtKbnMstModels));

            IjiSettingTabModel ijiSettingTab = ObjectExtension.CreateInstance<IjiSettingTabModel>();

            PrecriptionSettingTabModel precriptionSettingTab = new PrecriptionSettingTabModel(Mapper.Map<M10DayLimitModelDto, M10DayLimitModel>(request.M10DayLimitModels),
                                                                                              Mapper.Map<IpnMinYakkaMstModelDto, IpnMinYakkaMstModel>(request.IpnMinYakkaMsts),
                                                                                              Mapper.Map<DrugDayLimitModelDto, DrugDayLimitModel>(request.DrugDayLimits),
                                                                                              Mapper.Map(request.DosageMst, new DosageMstModel()),
                                                                                              Mapper.Map(request.IpnNameMst, new IpnNameMstModel()));

            UsageSettingTabModel usageSettingTab = ObjectExtension.CreateInstance<UsageSettingTabModel>();

            DrugInfomationTabModel drugInfomationTab = new DrugInfomationTabModel(Mapper.Map<DrugInfModelDto, DrugInfModel>(request.DrugInfs),
                                                                                  Mapper.Map(request.ZaiImage, new PiImageModel()),
                                                                                  Mapper.Map(request.HouImage, new PiImageModel()));

            TeikyoByomeiTabModel teikyoByomeiTab = new TeikyoByomeiTabModel(Mapper.Map<TeikyoByomeiModelDto, TeikyoByomeiModel>(request.TeikyoByomeis),
                                                                            Mapper.Map(request.TekiouByomeiMstExcluded, new TekiouByomeiMstExcludedModel()));

            SanteiKaishuTabModel santeiKaishuTab = new SanteiKaishuTabModel(Mapper.Map<DensiSanteiKaisuModelDto, DensiSanteiKaisuModel>(request.DensiSanteiKaisus));

            HaihanTabModel haihanTab = new HaihanTabModel(Mapper.Map<DensiHaihanModelDto, DensiHaihanModel>(request.DensiHaihanModel1s),
                                                          Mapper.Map<DensiHaihanModelDto, DensiHaihanModel>(request.DensiHaihanModel2s),
                                                          Mapper.Map<DensiHaihanModelDto, DensiHaihanModel>(request.DensiHaihanModel3s));

            HoukatsuTabModel houkatsuTab = new HoukatsuTabModel(Mapper.Map<DensiHoukatuModelDto, DensiHoukatuModel>(request.ListDensiHoukatuModels),
                                                                Mapper.Map<DensiHoukatuGrpModelDto, DensiHoukatuGrpModel>(request.ListDensiHoukatuGrpModels),
                                                                Mapper.Map<DensiHoukatuModelDto, DensiHoukatuModel>(request.ListDensiHoukatuMaster));

            CombinedContraindicationTabModel combinedContraindicationTab = new CombinedContraindicationTabModel(Mapper.Map<CombinedContraindicationModelDto, CombinedContraindicationModel>(request.CombinedContraindications));

            SetDataTenMstOriginModel setData = new SetDataTenMstOriginModel(basicSettingTab, ijiSettingTab, precriptionSettingTab, usageSettingTab, drugInfomationTab, teikyoByomeiTab, santeiKaishuTab, haihanTab, houkatsuTab, combinedContraindicationTab);

            var input = new SaveSetDataTenMstInputData(HpId, UserId, request.ItemCd, tenOrigins, setData);
            var output = _bus.Handle(input);
            var presenter = new SaveSetDataTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveSetDataTenMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListDrugImage)]
        public ActionResult<Response<GetListDrugImageResponse>> GetListDrugImage([FromQuery] GetListDrugImageRequest request)
        {
            var input = new GetListDrugImageInputData(request.Type, request.YjCd);
            var output = _bus.Handle(input);
            var presenter = new GetListDrugImagePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListDrugImageResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetRenkeiMst)]
        public ActionResult<Response<GetRenkeiMstResponse>> GetRenkeiMst([FromQuery] int renkeiId)
        {
            var input = new GetRenkeiMstInputData(HpId, renkeiId);
            var output = _bus.Handle(input);
            var presenter = new GetRenkeiMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRenkeiMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.CheckIsTenMstUsed)]
        public ActionResult<Response<CheckIsTenMstUsedResponse>> CheckIsTenMstUsed([FromQuery] CheckIsTenMstUsedRequest request)
        {
            var input = new CheckIsTenMstUsedInputData(HpId, request.ItemCd, request.StartDate, request.EndDate);
            var output = _bus.Handle(input);
            var presenter = new CheckIsTenMstUsedPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckIsTenMstUsedResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetJihiMstList)]
        public ActionResult<Response<GetJihiMstsResponse>> GetJihiMstList()
        {
            var input = new GetJihiSbtMstListInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetJihiMstsPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetJihiMstsResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTenMstListByItemType)]
        public ActionResult<Response<GetTenMstListByItemTypeResponse>> GetTenMstListByItemType([FromQuery] GetTenMstListByItemTypeRequest request)
        {
            var input = new GetTenMstListByItemTypeInputData(HpId, request.ItemType, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetTenMstListByItemTypePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenMstListByItemTypeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchTenMstItem)]
        public ActionResult<Response<SearchTenMstItemResponse>> SearchTenMstItem([FromBody] SearchTenMstItemRequest request)
        {
            var input = new SearchTenMstItemInputData(HpId, request.PageIndex, request.PageCount, request.Keyword, request.PointFrom,
                request.PointTo, request.KouiKbn, request.OriKouiKbn, request.KouiKbns, request.IncludeRosai, request.IncludeMisai,
                request.SinDate, request.ItemCodeStartWith, request.IsIncludeUsage, request.OnlyUsage, request.YJCode, request.IsMasterSearch,
                request.IsExpiredSearchIfNoData, request.IsAllowSearchDeletedItem, request.IsExpired, request.IsDeleted, request.DrugKbns,
                request.IsSearchSanteiItem, request.IsSearchKenSaItem, request.ItemFilter, request.IsSearch831SuffixOnly, request.IsSearchSuggestion);
            var output = _bus.Handle(input);
            var presenter = new SearchTenMstItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
