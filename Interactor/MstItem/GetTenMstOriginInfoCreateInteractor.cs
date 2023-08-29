using Domain.Models.MstItem;
using Entity.Tenant;
using Helper.Constants;
using Helper.Enum;
using System.Reflection;
using UseCase.MstItem.GetTenMstOriginInfoCreate;

namespace Interactor.MstItem
{
    public class GetTenMstOriginInfoCreateInteractor : IGetTenMstOriginInfoCreateInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public GetTenMstOriginInfoCreateInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetTenMstOriginInfoCreateOutputData Handle(GetTenMstOriginInfoCreateInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.InvalidHpId, string.Empty, 0, new());

                ItemTypeEnums[] listTypeItemValid = new[]
                {
                    ItemTypeEnums.UsageItem,
                    ItemTypeEnums.JihiItem,
                    ItemTypeEnums.SpecificMedicalMeterialItem,
                    ItemTypeEnums.COCommentItem,
                    ItemTypeEnums.SpecialMedicineCommentItem,
                    ItemTypeEnums.KonikaItem,
                    ItemTypeEnums.FCRItem,
                    ItemTypeEnums.CombinedContraindicationItem,
                    ItemTypeEnums.Jibaiseki,
                    ItemTypeEnums.Shimadzu,
                };

                if (!listTypeItemValid.Contains(inputData.Type))
                    return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.InvalidTypeItem, string.Empty, 0, new());

                string startWithstr = TenMstMaintenanceUtil.GetStartWithByItemType(inputData.Type);

                string itemCd = _mstItemRepository.GetMaxItemCdByTypeForAdd(startWithstr);
                int jihiSbt = 0;

                if (inputData.Type == ItemTypeEnums.JihiItem)
                    jihiSbt = _mstItemRepository.GetMinJihiSbtMst(inputData.HpId);

                var tenMst = AddNewTenMst(inputData.HpId, inputData.UserId, inputData.ItemCd, itemCd, jihiSbt, inputData.Type);

                return new GetTenMstOriginInfoCreateOutputData(GetTenMstOriginInfoCreateStatus.Successful, itemCd, jihiSbt, tenMst);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }

        private TenMstOriginModel AddNewTenMst(int hpId, int userId, string inputItemCd, string itemCd, int jihiSbt, ItemTypeEnums itemType)
        {
            int startDate = 0;
            int endDate = 99999999;
            int isAdopted = 1;
            string santeiItemCd = "9999999999";
            int createId = userId;
            bool isAddNew = true;

            string existedItemCd = string.Empty;
            string newItemCd = string.Empty;
            if (!string.IsNullOrEmpty(inputItemCd))
            {
                existedItemCd = inputItemCd;
            }
            else
            {
                newItemCd = itemCd;
            }

            if (string.IsNullOrEmpty(newItemCd) && string.IsNullOrEmpty(inputItemCd))
            {
                return new();
            }

           var finalItemCd = !string.IsNullOrEmpty(inputItemCd) ? existedItemCd : newItemCd;
            int sinKouiKbn = 0;
            string kanaName1 = string.Empty;
            string kanaName2 = string.Empty;
            string syukeiSaki = string.Empty;
            int tenId = 0;
            int isNodspRece = 0;
            int isNodspPaperRece = 0;
            string masterSbt = string.Empty;
            string receUnitName = string.Empty;
            string odrUnitName = string.Empty;
            int cmtKbn = 0;
            int defaultVal = 0;
            string kokuji1 = string.Empty;
            string kokuji2 = string.Empty;
            switch (itemType)
            {
                case ItemTypeEnums.JihiItem:
                    sinKouiKbn = 96;
                    kanaName1 = "ｼﾞﾋ";
                    syukeiSaki = "0";
                    tenId = 1;
                    isNodspRece = 1;
                    isNodspPaperRece = 1;
                    break;
                case ItemTypeEnums.SpecificMedicalMeterialItem:
                    masterSbt = "T";
                    sinKouiKbn = 80;
                    kanaName1 = "ﾄｸﾃｲ";
                    syukeiSaki = "0";
                    tenId = 1;
                    break;
                case ItemTypeEnums.UsageItem:
                    masterSbt = "Y";
                    sinKouiKbn = 21;
                    kanaName1 = "ﾖｳﾎｳ";
                    syukeiSaki = "0";
                    break;
                case ItemTypeEnums.SpecialMedicineCommentItem:
                    masterSbt = "C";
                    sinKouiKbn = 13;
                    kanaName1 = "ﾄｸﾔｸ";
                    syukeiSaki = "130";
                    cmtKbn = 5;
                    break;
                case ItemTypeEnums.COCommentItem:
                    masterSbt = "C";
                    sinKouiKbn = 99;
                    kanaName1 = "ｺﾒﾝﾄ";
                    syukeiSaki = "0";
                    break;
                case ItemTypeEnums.KonikaItem:
                    sinKouiKbn = 70;
                    kanaName1 = "ｺﾆｶﾌﾞｲ";
                    syukeiSaki = "700";
                    isNodspPaperRece = 1;
                    isNodspRece = 1;
                    jihiSbt = 1;
                    break;
                case ItemTypeEnums.FCRItem:
                    sinKouiKbn = 70;
                    kanaName1 = "FCR";
                    syukeiSaki = "700";
                    isNodspPaperRece = 1;
                    isNodspRece = 1;
                    jihiSbt = 1;
                    break;
                case ItemTypeEnums.Jibaiseki:
                    masterSbt = "S";
                    sinKouiKbn = 80;
                    kanaName1 = "ｼﾝﾀﾞﾝｼｮ料";
                    kanaName2 = "ｼﾞﾊﾞｲｾｷ";
                    receUnitName = "通";
                    odrUnitName = "通";
                    tenId = 1;
                    defaultVal = 1;
                    isAdopted = 1;
                    isNodspPaperRece = 1;
                    cmtKbn = 0;
                    kokuji1 = "1";
                    kokuji2 = "1";
                    syukeiSaki = "ZZ1";
                    break;
                case ItemTypeEnums.Shimadzu:
                    sinKouiKbn = 70;
                    isAdopted = 1;
                    kanaName1 = "SMZ";
                    syukeiSaki = "700";
                    isNodspPaperRece = 1;
                    isNodspRece = 1;
                    jihiSbt = 1;
                    santeiItemCd = "9999999999";
                    break;
            }

            return new TenMstOriginModel(hpId, startDate, endDate, isAdopted, santeiItemCd, createId, isAddNew, sinKouiKbn, kanaName1, kanaName2, syukeiSaki, tenId, isNodspRece, isNodspPaperRece, masterSbt, receUnitName, odrUnitName, cmtKbn, defaultVal, kokuji1, kokuji2, finalItemCd, jihiSbt);

        }
    }
}
