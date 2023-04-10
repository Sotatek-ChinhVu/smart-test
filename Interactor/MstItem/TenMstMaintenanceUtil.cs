using Domain.Models.MstItem;
using Helper.Enum;

namespace Interactor.MstItem
{
    public class TenMstMaintenanceUtil
    {
        public static string GetStartWithByItemType(ItemTypeEnums itemType, string itemCd = "")
        {
            string startWithstr = string.Empty;
            switch (itemType)
            {
                case ItemTypeEnums.JihiItem:
                    startWithstr = "J";
                    break;
                case ItemTypeEnums.UsageItem:
                    startWithstr = "Y";
                    break;
                case ItemTypeEnums.SpecificMedicalMeterialItem:
                    startWithstr = "Z";
                    break;
                case ItemTypeEnums.COCommentItem:
                    startWithstr = "CO";
                    break;
                case ItemTypeEnums.SpecialMedicineCommentItem:
                    startWithstr = "W";
                    break;
                case ItemTypeEnums.KonikaItem:
                    startWithstr = "KONI";
                    break;
                case ItemTypeEnums.FCRItem:
                    startWithstr = "FCR";
                    break;
                case ItemTypeEnums.Jibaiseki:
                    startWithstr = "S";
                    break;
                case ItemTypeEnums.CommentItem:
                    startWithstr = "8";
                    break;
                case ItemTypeEnums.Shimadzu:
                    startWithstr = "SMZ";
                    break;
            }
            return startWithstr;
        }

        public static bool IsUserCreateItem(ItemTypeEnums itemType)
        {
            if (itemType == ItemTypeEnums.JihiItem
                || itemType == ItemTypeEnums.UsageItem
                || itemType == ItemTypeEnums.SpecificMedicalMeterialItem
                || itemType == ItemTypeEnums.COCommentItem
                || itemType == ItemTypeEnums.SpecialMedicineCommentItem
                || itemType == ItemTypeEnums.KonikaItem
                || itemType == ItemTypeEnums.FCRItem
                || itemType == ItemTypeEnums.Jibaiseki
                || itemType == ItemTypeEnums.Shimadzu)
            {
                return true;
            }
            return false;
        }

        public static ItemTypeEnums GetItemType(TenMstOriginModel model)
        {
            if (model.ItemCd.StartsWith("J"))
            {
                return ItemTypeEnums.JihiItem;
            }
            else if (model.ItemCd.StartsWith("Z"))
            {
                return ItemTypeEnums.SpecificMedicalMeterialItem;
            }
            else if (model.ItemCd.StartsWith("Y"))
            {
                return ItemTypeEnums.UsageItem;
            }
            else if (model.ItemCd.StartsWith("W"))
            {
                return ItemTypeEnums.SpecialMedicineCommentItem;
            }
            else if (model.ItemCd.StartsWith("CO"))
            {
                return ItemTypeEnums.COCommentItem;
            }
            else if (model.ItemCd.StartsWith("KONI"))
            {
                return ItemTypeEnums.KonikaItem;
            }
            else if (model.ItemCd.StartsWith("FCR"))
            {
                return ItemTypeEnums.FCRItem;
            }
            else if (model.ItemCd.StartsWith("KN"))
            {
                return ItemTypeEnums.KensaItem;
            }
            else if (model.ItemCd.StartsWith("IGE"))
            {
                return ItemTypeEnums.TokuiTeki;
            }
            else if (model.ItemCd.StartsWith("SMZ"))
            {
                return ItemTypeEnums.Shimadzu;
            }
            else if (model.ItemCd.StartsWith("S"))
            {
                return ItemTypeEnums.Jibaiseki;
            }
            else if (model.ItemCd.StartsWith("X"))
            {
                return ItemTypeEnums.Dami;
            }
            else if (model.ItemCd.StartsWith("1") && model.ItemCd.Length == 9)
            {
                return ItemTypeEnums.ShinryoKoi;
            }
            else if (model.ItemCd.StartsWith("6") && model.ItemCd.Length == 9)
            {
                return ItemTypeEnums.Yakuzai;
            }
            else if (model.ItemCd.StartsWith("7") && model.ItemCd.Length == 9)
            {
                return ItemTypeEnums.Tokuzai;
            }
            else if (model.ItemCd.Length == 4)
            {
                return ItemTypeEnums.Bui;
            }
            else if (model.ItemCd.StartsWith("8") && model.ItemCd.Length == 9)
            {
                return ItemTypeEnums.CommentItem;
            }
            return ItemTypeEnums.Other;
        }
    }
}
