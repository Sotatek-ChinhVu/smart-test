using Domain.Models.ContainerMaster;
using Domain.Models.KensaIrai;
using Domain.Models.MaterialMaster;
using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Presenters.MstItem;
using EmrCloudApi.Requests.ListSetMst;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Requests.MstItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses.MstItem.DiseaseNameMstSearch;
using EmrCloudApi.Responses.MstItem.DiseaseSearch;
using EmrCloudApi.Services;
using Helper.Extension;
using Helper.Mapping;
using Interactor.MstItem;
using Microsoft.AspNetCore.Mvc;
using UseCase.ContainerMasterUpdate;
using UseCase.Core.Sync;
using UseCase.ListSetMst.UpdateListSetMst;
using UseCase.IsUsingKensa;
using UseCase.MstItem.CheckIsTenMstUsed;
using UseCase.MstItem.CompareTenMst;
using UseCase.MstItem.ConvertStringChkJISKj;
using UseCase.MstItem.DeleteOrRecoverTenMst;
using UseCase.MstItem.DiseaseNameMstSearch;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.ExistUsedKensaItemCd;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetAdoptedItemList;
using UseCase.MstItem.GetAllCmtCheckMst;
using UseCase.MstItem.GetCmtCheckMstList;
using UseCase.MstItem.GetDefaultPrecautions;
using UseCase.MstItem.GetDiseaseList;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetDrugAction;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.GetJihiSbtMstList;
using UseCase.MstItem.GetListByomeiSetGenerationMst;
using UseCase.MstItem.GetListDrugImage;
using UseCase.MstItem.GetListResultKensaMst;
using UseCase.MstItem.GetListKensaIjiSetting;
using UseCase.MstItem.GetListSetGenerationMst;
using UseCase.MstItem.GetListTenMstOrigin;
using UseCase.MstItem.GetListYohoSetMstModelByUserID;
using UseCase.MstItem.GetListUser;
using UseCase.MstItem.GetParrentKensaMst;
using UseCase.MstItem.GetRenkeiConf;
using UseCase.MstItem.GetRenkeiMst;
using UseCase.MstItem.GetSelectiveComment;
using UseCase.MstItem.GetSetDataTenMst;
using UseCase.MstItem.GetSetNameMnt;
using UseCase.MstItem.GetSingleDoseMstAndMedicineUnitList;
using UseCase.MstItem.GetTeikyoByomei;
using UseCase.MstItem.GetTenMstList;
using UseCase.MstItem.GetTenMstListByItemType;
using UseCase.MstItem.GetTenMstOriginInfoCreate;
using UseCase.MstItem.GetTreeByomeiSet;
using UseCase.MstItem.GetTreeListSet;
using UseCase.MstItem.IsKensaItemOrdering;
using UseCase.MstItem.IsUsingKensa;
using UseCase.MstItem.SaveAddressMst;
using UseCase.MstItem.SaveRenkei;
using UseCase.MstItem.SaveCompareTenMst;
using UseCase.MstItem.SaveSetDataTenMst;
using UseCase.MstItem.SaveSetNameMnt;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.SearchTenMstItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.MstItem.UpdateAdoptedItemList;
using UseCase.MstItem.UpdateByomeiMst;
using UseCase.MstItem.UpdateCmtCheckMst;
using UseCase.MstItem.UpdateJihiSbtMst;
using UseCase.MstItem.UpdateKensaStdMst;
using UseCase.MstItem.UpdateSingleDoseMst;
using UseCase.MstItem.UploadImageDrugInf;
using UseCase.UpdateKensaMst;
using UseCase.UpsertMaterialMaster;
using Domain.Models.OrdInfDetails;
using UseCase.MstItem.UpdateYohoSetMst;
using UseCase.MstItem.GetTenMstByCode;
using UseCase.MstItem.GetByomeiByCode;
using UseCase.MstItem.GetRenkeiTiming;
using UseCase.MstItem.CheckJihiSbtExistsInTenMst;

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
            var input = new GetDosageDrugListInputData(HpId, request.YjCds);
            var output = _bus.Handle(input);

            var presenter = new GetDosageDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDosageDrugListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetFoodAlrgy)]
        public ActionResult<Response<GetFoodAlrgyMasterDataResponse>> GetFoodAlrgy()
        {
            var input = new GetFoodAlrgyInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new FoodAlrgyMasterDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetFoodAlrgyMasterDataResponse>>(presenter.Result);
        }
        
        [HttpPost(ApiPath.SearchOTC)]
        public ActionResult<Response<SearchOTCResponse>> SearchOTC([FromBody] SearchOTCRequest request)
        {
            var input = new SearchOTCInputData(HpId, request.SearchValue, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchOTCPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchOTCResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchSupplement)]
        public ActionResult<Response<SearchSupplementResponse>> SearchSupplement([FromBody] SearchSupplementRequest request)
        {
            var input = new SearchSupplementInputData(HpId, request.SearchValue, request.PageIndex, request.PageSize);
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
            var input = new DiseaseSearchInputData(request.IsPrefix, request.IsByomei, request.IsSuffix, request.IsMisaiyou, request.Sindate, request.Keyword, request.PageIndex, request.PageSize, request.IsHasFreeByomei);
            var output = _bus.Handle(input);

            var presenter = new DiseaseSearchPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<DiseaseSearchResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.ParrentKensaMst)]
        public ActionResult<Response<GetParrentKensaMstListResponse>> GetParrentKensaMst([FromQuery] GetParrentKensaMstRequest request)
        {
            var input = new GetParrentKensaMstInputData(HpId, request.KeyWord, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetParrentKensaMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetParrentKensaMstListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetDiseaseList)]
        public ActionResult<Response<DiseaseSearchResponse>> GetDiseaseList([FromBody] GetDiseaseListRequest request)
        {
            var input = new GetDiseaseListInputData(request.ItemCdList);
            var output = _bus.Handle(input);

            var presenter = new GetDiseaseListPresenter();
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

        [HttpPost(ApiPath.ContainerMasterUpdate)]
        public ActionResult<Response<ContainerMasterUpdateResponse>> ContainerMasterUpdate([FromBody] ContainerMasterUpdateRequest request)
        {
            var upsertUserList = request.ContainerMasterList.Select(u => ContainerMasterRequestToModel(u)).ToList();
            var input = new ContainerMasterUpdateInputData(HpId, UserId, upsertUserList);
            var output = _bus.Handle(input);

            var presenter = new ContainerMasterUpdatePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ContainerMasterUpdateResponse>>(presenter.Result);
        }

        private static ContainerMasterModel ContainerMasterRequestToModel(ContainerMasterRequest containerMaster)
        {
            return
                new ContainerMasterModel
                (
                    containerMaster.ContainerCd,
                    containerMaster.ContainerName,
                    containerMaster.ContainerModelStatus
                );
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

        [HttpPost(ApiPath.UpsertMaterialMaster)]
        public ActionResult<Response<UpsertMaterialMasterResponse>> UpsertMaterialMaster([FromBody] UpsertMaterialMasterRequest request)
        {
            var upsertUserList = request.MaterialMasterList.Select(u => MaterialMasterRequestToModel(u)).ToList();
            var input = new UpsertMaterialMasterInputData(HpId, UserId, upsertUserList);
            var output = _bus.Handle(input);

            var presenter = new UpsertMaterialMasterPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertMaterialMasterResponse>>(presenter.Result);
        }

        private static MaterialMasterModel MaterialMasterRequestToModel(MaterialMasterRequest MaterialMaster)
        {
            return
                new MaterialMasterModel
                (
                    MaterialMaster.MaterialCd,
                    MaterialMaster.MaterialName,
                    MaterialMaster.MaterialModelStatus
                );
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
            var input = new GetTenMstOriginInfoCreateInputData(request.Type, HpId, UserId, request.ItemCd);
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

        [HttpPost(ApiPath.UpdateKensaMst)]
        public ActionResult<Response<UpdateKensaMstResponse>> UpdateKensaMst(UpdateKensaMstRequest request)
        {
            var input = new UpdateKensaMstInputData(HpId, UserId, request.KensaMstItems.Select(x => KensaMstItemsRequestToModel(x)).ToList(), request.ChildKensaMstItems.Select(x => ChildKensaMstItemsRequestToModel(x)).ToList(), request.TenMstItems.Select(x => TenMstItemsRequestToModel(x)).ToList(), request.TenMstListGenDate.Select(x => TenMstItemsRequestToModel(x)).ToList());
            var output = _bus.Handle(input);
            var presenter = new UpdateKensaMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateKensaMstResponse>>(presenter.Result);
        }

        private static TenItemModel TenMstItemsRequestToModel(TenMstInputItem tenMstItemModel)
        {
            return
                new TenItemModel
                (
                    tenMstItemModel.SinKouiKbn,
                    tenMstItemModel.MasterSbt,
                    tenMstItemModel.ItemCd,
                    tenMstItemModel.KensaItemCd,
                    tenMstItemModel.KensaItemSeqNo,
                    tenMstItemModel.Ten,
                    tenMstItemModel.Name,
                    tenMstItemModel.ReceName,
                    tenMstItemModel.KanaName1,
                    tenMstItemModel.KanaName2,
                    tenMstItemModel.KanaName3,
                    tenMstItemModel.KanaName4,
                    tenMstItemModel.KanaName5,
                    tenMstItemModel.KanaName6,
                    tenMstItemModel.KanaName7,
                    tenMstItemModel.StartDate,
                    tenMstItemModel.EndDate,
                    tenMstItemModel.DefaultValue,
                    tenMstItemModel.OdrUnitName,
                    tenMstItemModel.SanteiItemCd,
                    tenMstItemModel.SanteigaiKbn,
                    tenMstItemModel.IsNoSearch,
                    tenMstItemModel.IsDeleted
                );
        }

        private static KensaMstModel KensaMstItemsRequestToModel(KensaMstInputItem kensaMstItem)
        {
            return
                new KensaMstModel
                (
                    kensaMstItem.KensaItemCd,
                    kensaMstItem.KensaItemSeqNo,
                    kensaMstItem.CenterCd,
                    kensaMstItem.KensaName,
                    kensaMstItem.KensaKana,
                    kensaMstItem.Unit,
                    kensaMstItem.MaterialCd,
                    kensaMstItem.ContainerCd,
                    kensaMstItem.MaleStd,
                    kensaMstItem.MaleStdLow,
                    kensaMstItem.MaleStdHigh,
                    kensaMstItem.FemaleStd,
                    kensaMstItem.FemaleStdLow,
                    kensaMstItem.FemaleStdHigh,
                    kensaMstItem.Formula,
                    kensaMstItem.Digit,
                    kensaMstItem.OyaItemCd,
                    kensaMstItem.OyaItemSeqNo,
                    kensaMstItem.SortNo,
                    kensaMstItem.CenterItemCd1,
                    kensaMstItem.CenterItemCd2,
                    kensaMstItem.IsDeleted
                );
        }

        private static KensaMstModel ChildKensaMstItemsRequestToModel(ChildKensaMstInputItem kensaMstItem)
        {
            return
                new KensaMstModel
                (
                    kensaMstItem.KensaItemCd,
                    kensaMstItem.KensaItemSeqNo,
                    kensaMstItem.CenterCd,
                    kensaMstItem.KensaName,
                    kensaMstItem.KensaKana,
                    kensaMstItem.Unit,
                    kensaMstItem.MaterialCd,
                    kensaMstItem.ContainerCd,
                    kensaMstItem.MaleStd,
                    kensaMstItem.MaleStdLow,
                    kensaMstItem.MaleStdHigh,
                    kensaMstItem.FemaleStd,
                    kensaMstItem.FemaleStdLow,
                    kensaMstItem.FemaleStdHigh,
                    kensaMstItem.Formula,
                    kensaMstItem.Digit,
                    kensaMstItem.OyaItemCd,
                    kensaMstItem.OyaItemSeqNo,
                    kensaMstItem.SortNo,
                    kensaMstItem.CenterItemCd1,
                    kensaMstItem.CenterItemCd2,
                    kensaMstItem.IsDeleted
                );
        }

        [HttpPost(ApiPath.UpdateKensaStdMst)]
        public ActionResult<Response<UpdateKensaStdMstResponse>> UpdateKensaStdMst(UpdateKensaStdMstRequest request)
        {
            var input = new UpdateKensaStdMstInputData(HpId, UserId, request.KensaMstItems.Select(x => kensaStdMstItemsRequestToModel(x)).ToList());
            var output = _bus.Handle(input);

            var presenter = new UpdateKensaStdMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateKensaStdMstResponse>>(presenter.Result);
        }

        private static KensaStdMstModel kensaStdMstItemsRequestToModel(UpdateKensaStdMstInputItem stdMstInputItem)
        {
            return
                new KensaStdMstModel
                (
                    stdMstInputItem.KensaItemcd,
                    stdMstInputItem.MaleStd,
                    stdMstInputItem.MaleStdLow,
                    stdMstInputItem.MaleStdHigh,
                    stdMstInputItem.FemaleStd,
                    stdMstInputItem.FemaleStdLow,
                    stdMstInputItem.FemaleStdHigh,
                    stdMstInputItem.StartDate,
                    stdMstInputItem.IsModified,
                    stdMstInputItem.IsDeleted,
                    stdMstInputItem.CreateId
                );
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
            foreach (var tenOrigin in tenOrigins)
            {
                tenOrigin.ChangeHpId(HpId);
            }

            BasicSettingTabModel basicSettingTab = new BasicSettingTabModel(Mapper.Map<CmtKbnMstModelDto, CmtKbnMstModel>(request.CmtKbnMstModels));

            PrecriptionSettingTabModel precriptionSettingTab = new PrecriptionSettingTabModel(Mapper.Map<M10DayLimitModelDto, M10DayLimitModel>(request.M10DayLimitModels),
                                                                                              Mapper.Map<IpnMinYakkaMstModelDto, IpnMinYakkaMstModel>(request.IpnMinYakkaMsts),
                                                                                              Mapper.Map<DrugDayLimitModelDto, DrugDayLimitModel>(request.DrugDayLimits),
                                                                                              Mapper.Map(request.DosageMst, new DosageMstModel()),
                                                                                              Mapper.Map(request.IpnNameMst, new IpnNameMstModel()));

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

            SetDataTenMstOriginModel setData = new SetDataTenMstOriginModel(basicSettingTab, request.IjiSettingTabModel, precriptionSettingTab, request.UsageSettingTabModel, drugInfomationTab, teikyoByomeiTab, santeiKaishuTab, haihanTab, houkatsuTab, combinedContraindicationTab);

            var input = new SaveSetDataTenMstInputData(HpId, UserId, request.ItemCd, tenOrigins, setData);
            var output = _bus.Handle(input);
            var presenter = new SaveSetDataTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveSetDataTenMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListDrugImage)]
        public ActionResult<Response<GetListDrugImageResponse>> GetListDrugImage([FromQuery] GetListDrugImageRequest request)
        {
            var input = new GetListDrugImageInputData(request.Type, request.YjCd, request.SelectedImage);
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
                request.IsSearchSanteiItem, request.IsSearchKenSaItem, request.ItemFilter, request.IsSearch831SuffixOnly, request.IsSearchSuggestion,
                request.IsSearchGazoDensibaitaiHozon, request.SortCol, request.SortType);
            var output = _bus.Handle(input);
            var presenter = new SearchTenMstItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertStringChkJISKj)]
        public ActionResult<Response<ConvertStringChkJISKjResponse>> ConvertStringChkJISKj([FromBody] ConvertStringChkJISKjRequest request)
        {
            var input = new ConvertStringChkJISKjInputData(request.InputList);
            var output = _bus.Handle(input);
            var presenter = new ConvertStringChkJISKjPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetTeikyoByomei)]
        public ActionResult<Response<GetTeikyoByomeiResponse>> GetTeikyoByomei([FromQuery] GetTeikyoByomeiRequest request)
        {
            var input = new GetTeikyoByomeiInputData(HpId, request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new GetTeikyoByomeiPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetDrugAction)]
        public ActionResult<Response<GetDrugActionResponse>> GetDrugAction([FromQuery] GetDrugActionRequest request)
        {
            var input = new GetDrugActionInputData(HpId, request.YjCd);
            var output = _bus.Handle(input);
            var presenter = new GetDrugActionPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDrugActionResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetList)]
        public ActionResult<Response<GetTenMstListResponse>> GetTenMstList([FromBody] GetTenMstListRequest request)
        {
            var input = new GetTenMstListInputData(HpId, request.SinDate, request.ItemCdList);
            var output = _bus.Handle(input);
            var presenter = new GetTenMstListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenMstListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultPrecautions)]
        public ActionResult<Response<GetDefaultPrecautionsResponse>> GetDefaultPrecautions([FromQuery] GetDefaultPrecautionsRequest request)
        {
            var input = new GetDefaultPrecautionsInputData(HpId, request.YjCd);
            var output = _bus.Handle(input);
            var presenter = new GetDefaultPrecautionsPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDefaultPrecautionsResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UploadImageDrugInf)]
        public ActionResult<Response<UploadImageDrugInfResponse>> UploadImageDrugInf([FromQuery] UploadImageDrugInfRequest request)
        {
            if (!request.IsDeleted && Request.ContentLength > 30000000)
            {
                return Ok(new Response<UploadTemplateToCategoryResponse>()
                {
                    Message = "Invalid file size!",
                    Status = (int)UploadImageDrugInfStatus.InvalidSizeFile
                });
            }
            var input = new UploadImageDrugInfInputData(request.Type, request.YjCd, request.IsDeleted, Request.Body);
            var output = _bus.Handle(input);
            var presenter = new UploadImageDrugInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UploadImageDrugInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.DiseaseNameMstSearch)]
        public ActionResult<Response<DiseaseNameMstSearchResponse>> DiseaseNameMstSearch([FromBody] DiseaseNameMstSearchRequest request)
        {
            var input = new DiseaseNameMstSearchInputData(HpId, request.Keyword, request.ChkByoKbn0, request.ChkByoKbn1, request.ChkSaiKbn, request.ChkMiSaiKbn, request.ChkSidoKbn, request.ChkToku, request.ChkHiToku1, request.ChkHiToku2, request.ChkTenkan, request.ChkTokuTenkan, request.ChkNanbyo, request.PageIndex, request.PageSize, request.IsCheckPage);
            var output = _bus.Handle(input);

            var presenter = new DiseaseNameMstSearchPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<DiseaseNameMstSearchResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetAllCmtCheckMst)]
        public ActionResult<Response<GetAllCmtCheckMstResponse>> GetAllCmtCheckMst([FromQuery] GetAllCmtCheckMstRequest request)
        {
            var input = new GetAllCmtCheckMstInputData(HpId, request.SinDay);
            var output = _bus.Handle(input);
            var presenter = new GetAllCmtCheckMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateCmtCheckMst)]
        public ActionResult<Response<UpdateCmtCheckMstResponse>> UpdateCmtCheckMst([FromBody] UpdateCmtCheckMstRequest request)
        {
            var input = new UpdateCmtCheckMstInputData(UserId, HpId, request.ListItemCmt);
            var output = _bus.Handle(input);
            var presenter = new UpdateCmtCheckMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.SaveAddressMst)]
        public ActionResult<Response<SaveAddressMstResponse>> SaveAddressMst([FromBody] SaveAddressMstRequest request)
        {
            var input = new SaveAddressMstInputData(HpId, UserId, request.PostCodeMsts);
            var output = _bus.Handle(input);
            var presenter = new SaveAddressMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveAddressMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSingleDoseMstAndMedicineUnitList)]
        public ActionResult<Response<GetSingleDoseMstAndMedicineUnitResponse>> GetSingleDoseMstAndMedicineUnitList()
        {
            var input = new GetSingleDoseMstAndMedicineUnitListInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetSingleDoseMstAndMedicineUnitListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetSingleDoseMstAndMedicineUnitResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateSingleDoseMst)]
        public ActionResult<Response<UpdateSingleDoseMstResponse>> UpdateSingleDoseMst(UpdateSingleDoseMstRequest request)
        {
            var input = new UpdateSingleDoseMstInputData(HpId, UserId, request.SingleDoseMsts);
            var output = _bus.Handle(input);
            var presenter = new UpdateSingleDoseMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateSingleDoseMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateByomeiMst)]
        public ActionResult<Response<UpdateByomeiMstResponse>> UpdateByomeiMst([FromBody] UpdateByomeiMstRequest request)
        {
            var input = new UpdateByomeiMstInputData(UserId, HpId, request.ListData);
            var output = _bus.Handle(input);
            var presenter = new UpdateByomeiMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.F17Common)]
        public ActionResult<Response<F17CommonResponse>> F17Common([FromBody] F17CommonRequest request)
        {
            var input = new F17CommonInputData(HpId, request.KensaStdItemCd, request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new F17CommonPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<F17CommonResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.IsUsingKensa)]
        public ActionResult<Response<IsUsingKensaResponse>> IsUsingKensa([FromBody] IsUsingKensaRequest request)
        {
            var input = new IsUsingKensaInputData(HpId, request.KensaItemCd, request.ItemCds);
            var output = _bus.Handle(input);
            var presenter = new IsUsingKensaPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<IsUsingKensaResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.IsKensaItemOrdering)]
        public ActionResult<Response<IsKensaItemOrderingResponse>> IsKensaItemOrdering([FromQuery] IsKensaItemOrderingRequest request)
        {
            var input = new IsKensaItemOrderingInputData(HpId, request.TenItemCd);
            var output = _bus.Handle(input);
            var presenter = new IsKensaItemOrderingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<IsKensaItemOrderingResponse>>(presenter.Result);
        }
        [HttpGet(ApiPath.GetSetNameMnt)]
        public ActionResult<Response<GetSetNameMntResponse>> GetSetNameMnt([FromQuery] GetSetNameMntRequest request)
        {
            var input = new GetSetNameMntInputData(HpId, request.SetKbnChecked1, request.SetKbnChecked2, request.SetKbnChecked3, request.SetKbnChecked4, request.SetKbnChecked5, request.SetKbnChecked6, request.SetKbnChecked7,
                    request.SetKbnChecked8, request.SetKbnChecked9, request.SetKbnChecked10, request.JihiChecked, request.KihonChecked, request.TokuChecked, request.YohoChecked, request.DiffChecked);
            var output = _bus.Handle(input);
            var presenter = new GetSetNameMntPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetSetNameMntResponse>>(presenter.Result);
        }
        [HttpGet(ApiPath.GetListKensaIjiSetting)]
        public ActionResult<Response<GetListKensaIjiSettingResponse>> GetListKensaIjiSetting([FromQuery] GetListKensaIjiSettingRequest request)
        {
            var input = new GetListKensaIjiSettingInputData(HpId, request.KeyWords, request.IsValid, request.IsExpired);
            var output = _bus.Handle(input);
            var presenter = new GetListKensaIjiSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListKensaIjiSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTreeListSet)]
        public ActionResult<Response<GetTreeListSetMstResponse>> GetTreeListSet([FromQuery] GetTreeListSetRequest request)
        {
            var input = new GetTreeListSetInputData(HpId, request.SinDate, request.SetKbn);
            var output = _bus.Handle(input);

            var presenter = new GetTreeListSetMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetTreeListSetMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateJihiSbtMst)]
        public ActionResult<Response<UpdateJihiSbtMstResponse>> UpdateJihiSbtMst(UpdateJihiMstRequest request)
        {
            var input = new UpdateJihiSbtMstInputData(HpId, UserId, request.JihiSbtMsts);
            var output = _bus.Handle(input);
            var presenter = new UpdateJihiSbtMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateJihiSbtMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTreeByomeiSet)]
        public ActionResult<Response<GetTreeByomeiSetResponse>> GetTreeByomeiSet([FromQuery] GetTreeByomeiSetRequest request)
        {
            var input = new GetTreeByomeiSetInputData(HpId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetTreeByomeiSetPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTreeByomeiSetResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListSetGeneration)]
        public ActionResult<Response<GetListSetGenerationMstResponse>> GetListSetGeneration()
        {
            var input = new GetListSetGenerationMstInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetListSetGenerationMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListSetGenerationMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListByomeiSetGeneration)]
        public ActionResult<Response<GetListByomeiSetGenerationMstResponse>> GetListByomeiSetGeneration()
        {
            var input = new GetListByomeiSetGenerationMstInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetListByomeiSetGenerationMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListByomeiSetGenerationMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchCompareTenMst)]
        public ActionResult<Response<CompareTenMstResponse>> SearchCompareTenMst([FromBody] CompareTenMstRequest request)
        {
            var input = new CompareTenMstInputData(request.Actions, request.Comparison, request.SinDate, HpId);
            var output = _bus.Handle(input);
            var presenter = new CompareTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CompareTenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveCompareTenMst)]
        public ActionResult<Response<SaveCompareTenMstResponse>> SaveCompareTenMst([FromBody] SaveCompareTenMstRequest request)
        {
            var input = new SaveCompareTenMstInputData(request.ListData, request.Comparions, UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveCompareTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveCompareTenMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListKensaMst)]
        public ActionResult<Response<GetListKensaMstResponse>> GetListKensaMst([FromQuery] GetListKensaMstRequest request)
        {
            var input = new GetListKensaMstInputData(HpId, request.Keyword, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);
            var presenter = new GetListKensaMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListKensaMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListYohoSetMstModelByUserID)]
        public ActionResult<Response<GetListYohoSetMstModelByUserIDResponse>> GetListYohoSetMstModelByUserID([FromQuery] GetListYohoSetMstModelByUserIDRequest request)
        {
            var input = new GetListYohoSetMstModelByUserIDInputData(HpId, UserId, request.SinDate, request.UserId);
            var output = _bus.Handle(input);
            var presenter = new GetListYohoSetMstModelByUserIDPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListYohoSetMstModelByUserIDResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateYohoSetMst)]
        public ActionResult<Response<UpdateYohoSetMstResponse>> UpdateYohoSetMst(UpdateYohoSetMstRequest request)
        {
            var input = new UpdateYohoSetMstInputData(HpId, UserId, request.YohoSetMsts.Select(i => YohoSetMstRequestToModel(i)).ToList());
            var output = _bus.Handle(input);
            var presenter = new UpdateYohoSetMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateYohoSetMstResponse>>(presenter.Result);
        }
        private static YohoSetMstModel YohoSetMstRequestToModel(YohoSetMstRequest yohoSetMst)
        {
            return
                new YohoSetMstModel
                (
                    yohoSetMst.HpId,
                    yohoSetMst.SetId,
                    yohoSetMst.UserId,
                    yohoSetMst.SortNo,
                    yohoSetMst.ItemCd,
                    yohoSetMst.IsDeleted,
                    yohoSetMst.Itemname,
                    yohoSetMst.IsModified
                );
        }

        [HttpGet(ApiPath.GetListUser)]
        public ActionResult<Response<GetListUserResponse>> GetListUser([FromQuery] GetListUserRequest request)
        {
            var input = new GetListUserInputData(HpId, UserId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetListUserPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListUserResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetRenkeiConf)]
        public ActionResult<Response<GetRenkeiConfResponse>> GetRenkeiConf([FromQuery] GetRenkeiConfRequest request)
        {
            var input = new GetRenkeiConfInputData(HpId, request.RenkeiSbt, request.NotLoadMst);
            var output = _bus.Handle(input);
            var presenter = new GetRenkeiConfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRenkeiConfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveRenkei)]
        public ActionResult<Response<SaveRenkeiResponse>> SaveRenkei([FromBody] SaveRenkeiRequest request)
        {
            var renkeiTabList = GetRenkeiTabList(request);
            var input = new SaveRenkeiInputData(HpId, UserId, renkeiTabList);
            var output = _bus.Handle(input);
            var presenter = new SaveRenkeiPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveRenkeiResponse>>(presenter.Result);
        }

        private List<(int renkeiSbt, List<RenkeiConfModel> renkeiConfList)> GetRenkeiTabList(SaveRenkeiRequest request)
        {
            List<(int renkeiSbt, List<RenkeiConfModel> renkeiConfList)> result = new();
            foreach (var item in request.RenkeiTabList)
            {
                var renkeiConfList = item.RenkeiConfList.Select(conf =>
                    new RenkeiConfModel(
                        conf.Id,
                        conf.RenkeiId,
                        conf.Param,
                        conf.PtNumLength,
                        conf.TemplateId,
                        conf.IsInvalid,
                        conf.Biko,
                        conf.SortNo,
                        conf.IsDeleted,
                        conf.RenkeiPathConfModelList.Select(path =>
                            new RenkeiPathConfModel(
                                path.Id,
                                conf.RenkeiId,
                                0,
                                path.EdaNo,
                                path.Path,
                                path.Machine,
                                path.CharCd,
                                path.WorkPath,
                                path.Interval,
                                path.Param,
                                path.User,
                                path.PassWord,
                                path.IsInvalid,
                                path.Biko,
                                path.IsDeleted
                            )).ToList(),
                        conf.RenkeiTimingModelList.Select(timming =>
                            new RenkeiTimingModel(
                                timming.Id,
                                string.Empty,
                                conf.RenkeiId,
                                0,
                                timming.EventCd,
                                timming.IsInvalid,
                                timming.IsDeleted
                            )).ToList()
                    )).ToList();
                result.Add(new(item.RenkeiSbt, renkeiConfList));
            }
            return result;
        }

        [HttpGet(ApiPath.ExistUsedKensaItemCd)]
        public ActionResult<Response<ExistUsedKensaItemCdResponse>> ExistUsedKensaItemCd([FromQuery] ExistUsedKensaItemCdRequest request)
        {
            var input = new ExistUsedKensaItemCdInputData(HpId, request.KensaItemCd, request.KensaSeqNo);
            var output = _bus.Handle(input);
            var presenter = new ExistUsedKensaItemCdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ExistUsedKensaItemCdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTenMstByCode)]
        public ActionResult<Response<GetTenMstByCodeResponse>> GetTenMstByCode([FromQuery] GetTenMstByCodeRequest request)
        {
            var input = new GetTenMstByCodeInputData(request.ItemCd, request.SetKbn, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetTenMstByCodePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenMstByCodeResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetByomeiByCode)]
        public ActionResult<Response<GetByomeiByCodeResponse>> GetByomeiByCode([FromQuery] GetByomeiByCodeRequest request)
        {
            var input = new GetByomeiByCodeInputData(request.ByomeiCd);
            var output = _bus.Handle(input);
            var presenter = new GetByomeiByCodePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetByomeiByCodeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveSetNameMnt)]
        public ActionResult<Response<SaveSetNameMntResponse>> SaveSetNameMnt([FromBody] SaveSetNameMntRequest request)
        {
            var input = new SaveSetNameMntInputData(ConvertToSetNameMntModelList(request), HpId, UserId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new SaveSetNameMntPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveSetNameMntResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetRenkeiTiming)]
        public ActionResult<Response<GetRenkeiTimingResponse>> GetRenkeiTiming([FromQuery] GetRenkeiTimingRequest request)
        {
            var input = new GetRenkeiTimingInputData(HpId, request.RenkeiId);
            var output = _bus.Handle(input);
            var presenter = new GetRenkeiTimingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRenkeiTimingResponse>>(presenter.Result);
        }

        private List<SetNameMntModel> ConvertToSetNameMntModelList(SaveSetNameMntRequest request)
        {
            var result = request.ListData.Select(item => new SetNameMntModel(
                                                             item.IsSet,
                                                             item.SetFlag,
                                                             item.ItemCd,
                                                             item.ItemNameTenMst,
                                                             item.ItemNameTenMstBinding,
                                                             item.SetCd,
                                                             item.RowNo,
                                                             item.RpNo,
                                                             item.RpEdaNo,
                                                             item.SetKbn,
                                                             item.SetKbnEdaNo))
                                         .ToList();
            return result;
        }

        [HttpGet(ApiPath.CheckJihiSbtExistsInTenMst)]
        public ActionResult<Response<CheckJihiSbtExistsInTenMstResponse>> CheckJihiSbtExistsInTenMst([FromQuery] int jihiSbt)
        {
            var input = new CheckJihiSbtExistsInTenMstInputData(jihiSbt);
            var output = _bus.Handle(input);
            var presenter = new CheckJihiSbtExistsInTenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckJihiSbtExistsInTenMstResponse>>(presenter.Result);
        }
    }
}
