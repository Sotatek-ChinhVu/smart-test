using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Helper.Enum;
using UseCase.MstItem.GetSetDataTenMst;

namespace Interactor.MstItem
{
    public class GetSetDataTenMstInteractor : IGetSetDataTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetSetDataTenMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetSetDataTenMstOutputData Handle(GetSetDataTenMstInputData inputData)
        {
            try
            {
                if(inputData.HpId <= 0)
                {

                }

                if (string.IsNullOrEmpty(inputData.ItemSelected.ItemCd))
                {

                }

                var categoryList = TenMstMaintenanceUtil.InitCategoryList();
                ItemTypeEnums itemType = TenMstMaintenanceUtil.GetItemType(inputData.ItemSelected.ItemCd);
                FitlerCategoryListByItemType(itemType, ref categoryList);


                throw new NotImplementedException();
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }

        private void FitlerCategoryListByItemType(ItemTypeEnums itemType, ref List<CategoryItemModel> categoryList)
        {
            switch (itemType)
            {
                case ItemTypeEnums.JihiItem:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                       || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                       || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting
                                                       || item.CategoryItemEnums == CategoryItemEnums.Haihan
                                                       || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei)
                                      .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.TokuiTeki:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.Bui:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.KensaItem:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.UsageItem:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.UsageSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.SpecificMedicalMeterialItem:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.SpecialMaterialSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.Haihan)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.COCommentItem:
                case ItemTypeEnums.SpecialMedicineCommentItem:
                case ItemTypeEnums.Dami:
                case ItemTypeEnums.KonikaItem:
                case ItemTypeEnums.FCRItem:
                case ItemTypeEnums.Jibaiseki:
                case ItemTypeEnums.Shimadzu:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting)
                                        .ForEach(item => item.Visibility = true);
                    break;

                case ItemTypeEnums.CommentItem:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.ShinryoKoi:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei
                                                         || item.CategoryItemEnums == CategoryItemEnums.SanteiKaishu
                                                         || item.CategoryItemEnums == CategoryItemEnums.Haihan
                                                         || item.CategoryItemEnums == CategoryItemEnums.Houkatsu
                                                         || item.CategoryItemEnums == CategoryItemEnums.CombinedContraindication
                                                         || item.CategoryItemEnums == CategoryItemEnums.RenkeiSetting)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.Yakuzai:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.IjiSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.PrecriptionSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.DrugInfomation
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei
                                                         || item.CategoryItemEnums == CategoryItemEnums.CombinedContraindication)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.Tokuzai:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.SpecialMaterialSetting
                                                         || item.CategoryItemEnums == CategoryItemEnums.TeikyoByomei
                                                         || item.CategoryItemEnums == CategoryItemEnums.CombinedContraindication)
                                        .ForEach(item => item.Visibility = true);
                    break;
                case ItemTypeEnums.Other:
                    categoryList.FindAll(item => item.CategoryItemEnums == CategoryItemEnums.BasicSetting)
                                       .ForEach(item => item.Visibility = true);
                    break;
            }
        }


        private void ReloadSubVMSelectedItem(TenMstOriginModel tenMstOrigin, List<CategoryItemModel> listCategories, int hpId, int sinDate)
        {
            List<CategoryItemModel> visibleList = listCategories.FindAll(item => item.Visibility);

            BasicSettingTabModel basicSettingTab = Activator.CreateInstance<BasicSettingTabModel>();
            IjiSettingTabModel ijiSettingTab = Activator.CreateInstance<IjiSettingTabModel>();
            PrecriptionSettingTabModel precriptionSettingTab = Activator.CreateInstance<PrecriptionSettingTabModel>();
            UsageSettingTabModel usageSettingTab = Activator.CreateInstance<UsageSettingTabModel>();

            // Update function
            foreach (CategoryItemModel category in visibleList)
            {
                switch (category.CategoryItemEnums)
                {
                    case CategoryItemEnums.BasicSetting:
                        basicSettingTab = new BasicSettingTabModel(_mstItemRepository.GetListCmtKbnMstModelByItemCd(hpId, tenMstOrigin.ItemCd));
                        break;
                    case CategoryItemEnums.IjiSetting:
                        ijiSettingTab = new IjiSettingTabModel(LoadIjiSetting(ref tenMstOrigin, hpId, sinDate));
                        break;
                    case CategoryItemEnums.PrecriptionSetting:
                        precriptionSettingTab = LoadPrecriptionSetting(tenMstOrigin, hpId, sinDate);
                        break;
                    case CategoryItemEnums.UsageSetting:
                        usageSettingTab = new UsageSettingTabModel(_mstItemRepository.GetYohoInfMstPrefixByItemCd(tenMstOrigin.ItemCd));
                        break;
                    case CategoryItemEnums.SpecialMaterialSetting:
                        break;
                    case CategoryItemEnums.DrugInfomation:
                        DrugInfomationRelateVM.LoadData(selectedModel);
                        if (!IsEnableViewSetting) DrugInfomationRelateVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.TeikyoByomei:
                        TeikyoByomeiVM.LoadData(selectedModel, IsEditMode);
                        if (!IsEnableViewSetting) TeikyoByomeiVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.SanteiKaishu:
                        SanteiKaishuVM.LoadData(selectedModel, IsEditMode);
                        if (!IsEnableViewSetting) SanteiKaishuVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.Haihan:
                        HaihanVM.LoadData(selectedModel, IsEditMode);
                        if (!IsEnableViewSetting) HaihanVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.Houkatsu:
                        HokatsuVM.LoadData(selectedModel, SinDate);
                        if (!IsEnableViewSetting) HokatsuVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.CombinedContraindication:
                        CombinedContraindicationVM.LoadData(selectedModel, IsEditMode, SinDate);
                        if (!IsEnableViewSetting) CombinedContraindicationVM.IsEnableViewSetting = false;
                        break;
                    case CategoryItemEnums.RenkeiSetting:
                        RenkeiSettingVM.LoadData(selectedModel, IsEditMode);
                        if (!IsEnableViewSetting) RenkeiSettingVM.IsEnableViewSetting = false;
                        break;
                }
            }
            HeaderTenMstVM.LoadData(selectedModel, IsEditMode);
            if (!IsEnableViewSetting) HeaderTenMstVM.IsEnableViewSetting = false;
            selectedModel.PropertyChanged -= SelectedItemModel_PropertyChanged;
            selectedModel.PropertyChanged += SelectedItemModel_PropertyChanged;
        }

        private string LoadIjiSetting(ref TenMstOriginModel tenMstOrigin, int hpId, int sinDate)
        {
            tenMstOrigin.SetAgekasanCd1Note(_mstItemRepository.GetTenMstOriginModel(hpId, tenMstOrigin.AgekasanCd1, sinDate).Name);
            tenMstOrigin.SetAgekasanCd2Note(_mstItemRepository.GetTenMstOriginModel(hpId, tenMstOrigin.AgekasanCd2, sinDate).Name);
            tenMstOrigin.SetAgekasanCd3Note(_mstItemRepository.GetTenMstOriginModel(hpId, tenMstOrigin.AgekasanCd3, sinDate).Name);
            tenMstOrigin.SetAgekasanCd4Note(_mstItemRepository.GetTenMstOriginModel(hpId, tenMstOrigin.AgekasanCd4, sinDate).Name);

            if (!string.IsNullOrEmpty(tenMstOrigin.SanteiItemCd) && tenMstOrigin.SanteiItemCd != "9999999999")
            {
                return _mstItemRepository.GetTenMstName(hpId, tenMstOrigin.SanteiItemCd);
            }

            return string.Empty;
        }

        private PrecriptionSettingTabModel LoadPrecriptionSetting(TenMstOriginModel tenMstOrigin, int hpId, int sinDate)
        {
            List<M10DayLimitModel> m10DayLimits = _mstItemRepository.GetM10DayLimitModels(tenMstOrigin.YjCd);

            List<IpnMinYakkaMstModel> ipnMinYakkaMsts = _mstItemRepository.GetIpnMinYakkaMstModels(hpId, tenMstOrigin.IpnNameCd);

            List<DrugDayLimitModel> drugDayLimits = _mstItemRepository.GetDrugDayLimitModels(hpId, tenMstOrigin.ItemCd);

            DosageMstModel dosageMst = _mstItemRepository.GetDosageMstModel(hpId, tenMstOrigin.ItemCd);

            IpnNameMstModel ipnNameMst = _mstItemRepository.GetIpnNameMstModel(hpId, tenMstOrigin.IpnNameCd, sinDate);

            return new PrecriptionSettingTabModel(m10DayLimits, ipnMinYakkaMsts, drugDayLimits, dosageMst, ipnNameMst);
        }
    }
}
