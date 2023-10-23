using Domain.Models.MstItem;
using Helper.Enum;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.MstItem.SaveSetDataTenMst;

namespace Interactor.MstItem
{
    public class SaveSetDataTenMstInteractor : ISaveSetDataTenMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveSetDataTenMstInteractor(ITenantProvider tenantProvider, IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveSetDataTenMstOutputData Handle(SaveSetDataTenMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus.InvalidHpId);

                if (inputData.UserId <= 0)
                    return new SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus.InvalidUserId);

                if (string.IsNullOrEmpty(inputData.ItemCd))
                    return new SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus.InvalidItemCd);

                List<CategoryItemModel> categoryList = TenMstMaintenanceUtil.InitCategoryList();
                ItemTypeEnums itemType = TenMstMaintenanceUtil.GetItemType(inputData.ItemCd);
                FitlerCategoryListByItemType(itemType, ref categoryList);

                IEnumerable<CategoryItemEnums> listAct = categoryList.FindAll(item => item.Visibility).Select(x => x.CategoryItemEnums);

                bool res = _mstItemRepository.SaveTenMstOriginSetData(listAct, inputData.ItemCd, inputData.TenOrigins, inputData.SetData, inputData.UserId, inputData.HpId);
                //if (res)
                return new SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus.Successful);
                //else
                //    return new SaveSetDataTenMstOutputData(SaveSetDataTenMstStatus.Failed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
                _loggingHandler.Dispose();
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
    }
}
