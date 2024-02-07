using Domain.Models.MstItem;
using Domain.Models.OrdInf;
using Domain.Models.TodayOdr;
using Helper.Enum;
using Helper.Extension;
using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization;
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
                if (inputData.HpId <= 0)
                    return new GetSetDataTenMstOutputData(GetSetDataTenMstStatus.InvalidHpId, ObjectExtension.CreateInstance<SetDataTenMstOriginModel>());

                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new GetSetDataTenMstOutputData(GetSetDataTenMstStatus.InvalidItemCd, ObjectExtension.CreateInstance<SetDataTenMstOriginModel>());

                var categoryList = TenMstMaintenanceUtil.InitCategoryList();
                ItemTypeEnums itemType = TenMstMaintenanceUtil.GetItemType(inputData.ItemCd);
                FitlerCategoryListByItemType(itemType, ref categoryList);

                SetDataTenMstOriginModel result = ReloadSubVMSelectedItem(inputData.ItemCd,
                                                                          inputData.JiCd,
                                                                          inputData.IpnNameCd,
                                                                          inputData.SanteiItemCd,
                                                                          inputData.AgekasanCd1Note,
                                                                          inputData.AgekasanCd2Note,
                                                                          inputData.AgekasanCd3Note,
                                                                          inputData.AgekasanCd4Note,
                                                                          categoryList,
                                                                          inputData.HpId,
                                                                          inputData.SinDate);

                return new GetSetDataTenMstOutputData(GetSetDataTenMstStatus.Successful, result);
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


        private SetDataTenMstOriginModel ReloadSubVMSelectedItem(string itemCd, string jiCd, string ipnNameCd, string santeiItemCd, string agekasanCd1, string agekasanCd2, string agekasanCd3, string agekasanCd4, List<CategoryItemModel> listCategories, int hpId, int sinDate)
        {
            List<CategoryItemModel> visibleList = listCategories.FindAll(item => item.Visibility);

            BasicSettingTabModel basicSettingTab = ObjectExtension.CreateInstance<BasicSettingTabModel>();
            IjiSettingTabModel ijiSettingTab = ObjectExtension.CreateInstance<IjiSettingTabModel>();
            PrecriptionSettingTabModel precriptionSettingTab = ObjectExtension.CreateInstance<PrecriptionSettingTabModel>();
            UsageSettingTabModel usageSettingTab = ObjectExtension.CreateInstance<UsageSettingTabModel>();
            DrugInfomationTabModel drugInfomationTab = ObjectExtension.CreateInstance<DrugInfomationTabModel>();
            TeikyoByomeiTabModel teikyoByomeiTab = ObjectExtension.CreateInstance<TeikyoByomeiTabModel>();
            SanteiKaishuTabModel santeiKaishuTab = ObjectExtension.CreateInstance<SanteiKaishuTabModel>();
            HaihanTabModel haihanTab = ObjectExtension.CreateInstance<HaihanTabModel>();
            HoukatsuTabModel houkatsuTab = ObjectExtension.CreateInstance<HoukatsuTabModel>();
            CombinedContraindicationTabModel combinedContraindicationTab = ObjectExtension.CreateInstance<CombinedContraindicationTabModel>();

            // Update function
            foreach (CategoryItemModel category in visibleList)
            {
                switch (category.CategoryItemEnums)
                {
                    case CategoryItemEnums.BasicSetting:
                        basicSettingTab = new BasicSettingTabModel(_mstItemRepository.GetListCmtKbnMstModelByItemCd(hpId, itemCd));
                        break;
                    case CategoryItemEnums.IjiSetting:
                        ijiSettingTab = LoadIjiSetting(santeiItemCd, agekasanCd1, agekasanCd2, agekasanCd3, agekasanCd4, hpId, sinDate);
                        break;
                    case CategoryItemEnums.PrecriptionSetting:
                        precriptionSettingTab = LoadPrecriptionSetting(itemCd, jiCd, ipnNameCd, hpId, sinDate);
                        break;
                    case CategoryItemEnums.UsageSetting:
                        usageSettingTab = new UsageSettingTabModel(_mstItemRepository.GetYohoInfMstPrefixByItemCd(itemCd));
                        break;
                    case CategoryItemEnums.SpecialMaterialSetting:
                        break;
                    case CategoryItemEnums.DrugInfomation:
                        drugInfomationTab = LoadDrugInfomation(itemCd, hpId);
                        break;
                    case CategoryItemEnums.TeikyoByomei:
                        teikyoByomeiTab = LoadTeikyoByomeiTab(itemCd, hpId);
                        break;
                    case CategoryItemEnums.SanteiKaishu:
                        santeiKaishuTab = LoadSanteiKaishuTab(itemCd, hpId);
                        break;
                    case CategoryItemEnums.Haihan:
                        haihanTab = LoadHaihanTab(itemCd, hpId);
                        break;
                    case CategoryItemEnums.Houkatsu:
                        houkatsuTab = LoadHoukatsuTab(itemCd, hpId, sinDate);
                        break;
                    case CategoryItemEnums.CombinedContraindication:
                        combinedContraindicationTab = LoadCombinedContraindication(itemCd, sinDate);
                        break;
                    case CategoryItemEnums.RenkeiSetting:
                        break;
                }
            }

            return new SetDataTenMstOriginModel(basicSettingTab,
                                                ijiSettingTab,
                                                precriptionSettingTab,
                                                usageSettingTab,
                                                drugInfomationTab,
                                                teikyoByomeiTab,
                                                santeiKaishuTab,
                                                haihanTab,
                                                houkatsuTab,
                                                combinedContraindicationTab);
        }

        private IjiSettingTabModel LoadIjiSetting(string santeiItemCd, string agekasanCd1, string agekasanCd2, string agekasanCd3, string agekasanCd4, int hpId, int sinDate)
        {
            string searchItemName = string.Empty;
            string agekasanCd1Note = _mstItemRepository.GetTenMstOriginModel(hpId, agekasanCd1, sinDate).Name;
            string agekasanCd2Note = _mstItemRepository.GetTenMstOriginModel(hpId, agekasanCd2, sinDate).Name;
            string agekasanCd3Note = _mstItemRepository.GetTenMstOriginModel(hpId, agekasanCd3, sinDate).Name;
            string agekasanCd4Note = _mstItemRepository.GetTenMstOriginModel(hpId, agekasanCd4, sinDate).Name;

            if (!string.IsNullOrEmpty(santeiItemCd) && santeiItemCd != "9999999999")
            {
                searchItemName = _mstItemRepository.GetTenMstName(hpId, santeiItemCd);
            }
            return new IjiSettingTabModel(searchItemName, agekasanCd1Note, agekasanCd2Note, agekasanCd3Note, agekasanCd4Note);
        }

        private PrecriptionSettingTabModel LoadPrecriptionSetting(string itemCd, string yiCd, string ipnNameCd, int hpId, int sinDate)
        {
            List<M10DayLimitModel> m10DayLimits = _mstItemRepository.GetM10DayLimitModels(hpId, yiCd);
            List<IpnMinYakkaMstModel> ipnMinYakkaMsts = _mstItemRepository.GetIpnMinYakkaMstModels(hpId, ipnNameCd);
            List<DrugDayLimitModel> drugDayLimits = _mstItemRepository.GetDrugDayLimitModels(hpId, itemCd);

            DosageMstModel dosageMst = _mstItemRepository.GetDosageMstModel(hpId, itemCd);
            IpnNameMstModel ipnNameMst = _mstItemRepository.GetIpnNameMstModel(hpId, ipnNameCd, sinDate);

            return new PrecriptionSettingTabModel(m10DayLimits, ipnMinYakkaMsts, drugDayLimits, dosageMst, ipnNameMst);
        }

        private DrugInfomationTabModel LoadDrugInfomation(string itemCd, int hpId)
        {
            List<DrugInfModel> drugInfs = _mstItemRepository.GetDrugInfByItemCd(hpId, itemCd);

            PiImageModel zaiImage = _mstItemRepository.GetImagePiByItemCd(hpId, itemCd, 0);
            PiImageModel houImage = _mstItemRepository.GetImagePiByItemCd(hpId, itemCd, 1);

            return new DrugInfomationTabModel(drugInfs, zaiImage, houImage);
        }


        private TeikyoByomeiTabModel LoadTeikyoByomeiTab(string itemCd, int hpId)
        {
            List<TeikyoByomeiModel> teikyoByomeis = _mstItemRepository.GetTeikyoByomeiModel(hpId, itemCd);

            TekiouByomeiMstExcludedModel tekiouByomeiMstExcluded = _mstItemRepository.GetTekiouByomeiMstExcludedModelByItemCd(hpId, itemCd);

            return new TeikyoByomeiTabModel(teikyoByomeis, tekiouByomeiMstExcluded);
        }


        private SanteiKaishuTabModel LoadSanteiKaishuTab(string itemCd, int hpId)
        {
            List<DensiSanteiKaisuModel> listDensiSanteis = _mstItemRepository.GetDensiSanteiKaisuByItemCd(hpId, itemCd);
            return new SanteiKaishuTabModel(listDensiSanteis);
        }

        private HaihanTabModel LoadHaihanTab(string itemCd, int hpId)
        {
            List<DensiHaihanModel> densiHaihanModel1s = _mstItemRepository.GetDensiHaihans(hpId, itemCd, 2);
            List<DensiHaihanModel> densiHaihanModel2s = _mstItemRepository.GetDensiHaihans(hpId, itemCd, 1);
            List<DensiHaihanModel> densiHaihanModel3s = _mstItemRepository.GetDensiHaihans(hpId, itemCd, 3);

            return new HaihanTabModel(densiHaihanModel1s, densiHaihanModel2s, densiHaihanModel3s);
        }

        private HoukatsuTabModel LoadHoukatsuTab(string itemCd, int hpId, int sinDate)
        {
            List<DensiHoukatuModel> listDensiHoukatuModels = _mstItemRepository.GetListDensiHoukatuByItemCd(hpId, itemCd, sinDate);
            List<DensiHoukatuGrpModel> listDensiHoukatuGrpModels = _mstItemRepository.GetListDensiHoukatuGrpByItemCd(hpId, itemCd, sinDate);

            List<string> listGroupNo = listDensiHoukatuModels.GroupBy(x => x.HoukatuGrpNo).Select(x => x.First()).Select(x => x.HoukatuGrpNo).ToList();
            List<DensiHoukatuModel> listDensiHoukatuMasters = _mstItemRepository.GetListDensiHoukatuMaster(hpId, listGroupNo);

            return new HoukatsuTabModel(listDensiHoukatuModels, listDensiHoukatuGrpModels, listDensiHoukatuMasters);
        }

        private CombinedContraindicationTabModel LoadCombinedContraindication(string itemCd, int sinDate)
        {
            List<CombinedContraindicationModel> combinedContraindications = _mstItemRepository.GetContraindicationModelList(sinDate, itemCd).OrderBy(x => x.BCd).ToList();
            return new CombinedContraindicationTabModel(combinedContraindications);
        }
    }
}
