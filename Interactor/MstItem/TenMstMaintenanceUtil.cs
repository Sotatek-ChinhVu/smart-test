using DocumentFormat.OpenXml.Drawing.Diagrams;
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

        public static ItemTypeEnums GetItemType(string itemCd)
        {
            if (itemCd.StartsWith("J"))
            {
                return ItemTypeEnums.JihiItem;
            }
            else if (itemCd.StartsWith("Z"))
            {
                return ItemTypeEnums.SpecificMedicalMeterialItem;
            }
            else if (itemCd.StartsWith("Y"))
            {
                return ItemTypeEnums.UsageItem;
            }
            else if (itemCd.StartsWith("W"))
            {
                return ItemTypeEnums.SpecialMedicineCommentItem;
            }
            else if (itemCd.StartsWith("CO"))
            {
                return ItemTypeEnums.COCommentItem;
            }
            else if (itemCd.StartsWith("KONI"))
            {
                return ItemTypeEnums.KonikaItem;
            }
            else if (itemCd.StartsWith("FCR"))
            {
                return ItemTypeEnums.FCRItem;
            }
            else if (itemCd.StartsWith("KN"))
            {
                return ItemTypeEnums.KensaItem;
            }
            else if (itemCd.StartsWith("IGE"))
            {
                return ItemTypeEnums.TokuiTeki;
            }
            else if (itemCd.StartsWith("SMZ"))
            {
                return ItemTypeEnums.Shimadzu;
            }
            else if (itemCd.StartsWith("S"))
            {
                return ItemTypeEnums.Jibaiseki;
            }
            else if (itemCd.StartsWith("X"))
            {
                return ItemTypeEnums.Dami;
            }
            else if (itemCd.StartsWith("1") && itemCd.Length == 9)
            {
                return ItemTypeEnums.ShinryoKoi;
            }
            else if (itemCd.StartsWith("6") && itemCd.Length == 9)
            {
                return ItemTypeEnums.Yakuzai;
            }
            else if (itemCd.StartsWith("7") && itemCd.Length == 9)
            {
                return ItemTypeEnums.Tokuzai;
            }
            else if (itemCd.Length == 4)
            {
                return ItemTypeEnums.Bui;
            }
            else if (itemCd.StartsWith("8") && itemCd.Length == 9)
            {
                return ItemTypeEnums.CommentItem;
            }
            return ItemTypeEnums.Other;
        }

        public static List<CategoryItemModel> InitCategoryList()
        {
            return new List<CategoryItemModel>()
            {
                new CategoryItemModel()
                {
                    CategoryDisplayName = "基本設定",
                    CategoryItemEnums = CategoryItemEnums.BasicSetting,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "医事設定",
                    CategoryItemEnums = CategoryItemEnums.IjiSetting,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "処方設定",
                    CategoryItemEnums = CategoryItemEnums.PrecriptionSetting,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "用法設定",
                    CategoryItemEnums = CategoryItemEnums.UsageSetting,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "特材設定",
                    CategoryItemEnums = CategoryItemEnums.SpecialMaterialSetting,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "薬剤情報関連",
                    CategoryItemEnums = CategoryItemEnums.DrugInfomation,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "適応病名",
                    CategoryItemEnums = CategoryItemEnums.TeikyoByomei,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "算定回数",
                    CategoryItemEnums = CategoryItemEnums.SanteiKaishu,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "背反",
                    CategoryItemEnums = CategoryItemEnums.Haihan,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "包括",
                    CategoryItemEnums = CategoryItemEnums.Houkatsu,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "併用禁忌",
                    CategoryItemEnums = CategoryItemEnums.CombinedContraindication,
                    Visibility = false
                },
                new CategoryItemModel()
                {
                    CategoryDisplayName = "連携設定",
                    CategoryItemEnums = CategoryItemEnums.RenkeiSetting,
                    Visibility = false
                }
            };
        }
    }

    public class CategoryItemModel
    {
        public string CategoryDisplayName { get; set; } = string.Empty;

        public CategoryItemEnums CategoryItemEnums { get; set; }

        public bool Visibility { get; set; }
    }
}
