namespace Domain.Types
{
    public interface IOdrInfDetailModel
    {
        int HpId { get; }

        long RpNo { get; }

        long RpEdaNo { get; }

        int RowNo { get; }

        int SinKouiKbn { get; }

        string ItemCd { get; }

        string ItemName { get; }

        double Suryo { get; }

        string UnitName { get; }

        int UnitSbt { get; }

        double TermVal { get; }

        int KohatuKbn { get; }

        int SyohoKbn { get; }

        int SyohoLimitKbn { get; }

        int DrugKbn { get; }

        int YohoKbn { get; }

        string Kokuji1 { get; }

        string Kokuji2 { get; }

        int IsNodspRece { get; }

        string IpnCd { get; }

        string IpnName { get; }

        string Bunkatu { get; }

        string CmtName { get; }

        string CmtOpt { get; }

        string FontColor { get; }

        int CommentNewline { get; }

        #region Exposed properties
        bool IsEmpty { get; }

        double Price { get; }

        string DisplayedQuantity { get; }

        string DisplayedUnit { get; }

        int CmtCol1 { get; }

        double Ten { get; }

        string MasterSbt { get; }

        bool IsSpecialItem { get; }

        bool IsStandardUsage { get; }

        bool IsSuppUsage { get; }

        bool IsInjectionUsage { get; }

        bool IsDrugUsage { get; }

        bool IsDrug { get; }

        bool IsInjection { get; }

        bool Is830Cmt { get; }

        bool Is831Cmt { get; }

        bool Is840Cmt { get; }

        bool Is842Cmt { get; }

        bool Is850Cmt { get; }

        bool Is851Cmt { get; }

        bool Is852Cmt { get; }

        bool Is853Cmt { get; }

        bool Is880Cmt { get; }

        bool IsShohoBiko { get; }

        bool IsShohoComment { get; }
        #endregion

        public TodayOrdValidationStatus Validation(int flag)
        {
            #region Validate common

            if (RowNo <= 0)
            {
                return TodayOrdValidationStatus.InvalidRowNo;
            }
            if (SinKouiKbn < 0)
            {
                return TodayOrdValidationStatus.InvalidSinKouiKbn;
            }
            if (ItemCd.Length > 10)
            {
                return TodayOrdValidationStatus.InvalidItemCd;
            }
            if (ItemName.Length > 240)
            {
                return TodayOrdValidationStatus.InvalidItemName;
            }
            if (Suryo < 0 || (ItemCd == ItemCdConst.JikanKihon && !(Suryo >= 0 && Suryo <= 7)) || (ItemCd == ItemCdConst.SyosaiKihon && !(Suryo >= 0 && Suryo <= 8)))
            {
                return TodayOrdValidationStatus.InvalidSuryo;
            }
            if (UnitName.Length > 24)
            {
                return TodayOrdValidationStatus.InvalidUnitName;
            }
            if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
            {
                return TodayOrdValidationStatus.InvalidSyohoKbn;
            }
            if (!(YohoKbn >= 0 && YohoKbn <= 2))
            {
                return TodayOrdValidationStatus.InvalidYohoKbn;
            }
            if (Bunkatu.Length > 10)
            {
                return TodayOrdValidationStatus.InvalidBunkatuLength;
            }
            if (CmtName.Length > 240)
            {
                return TodayOrdValidationStatus.InvalidCmtName;
            }
            if (CmtOpt.Length > 38)
            {
                return TodayOrdValidationStatus.InvalidCmtOpt;
            }

            if (flag == 0)
            {
                if (HpId <= 0)
                {
                    return TodayOrdValidationStatus.InvalidHpId;
                }
                if (RaiinNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRaiinNo;
                }
                if (RpNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRpNo;
                }
                if (RpEdaNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRpEdaNo;
                }
                if (RowNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRowNo;
                }
                if (PtId <= 0)
                {
                    return TodayOrdValidationStatus.InvalidPtId;
                }
                if (SinDate <= 0)
                {
                    return TodayOrdValidationStatus.InvalidSinDate;
                }
                if (SinKouiKbn < 0)
                {
                    return TodayOrdValidationStatus.InvalidSinKouiKbn;
                }
                if (ItemCd.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidItemCd;
                }
                if (ItemName.Length > 240)
                {
                    return TodayOrdValidationStatus.InvalidItemName;
                }
                if (Suryo < 0)
                {
                    return TodayOrdValidationStatus.InvalidSuryo;
                }
                if (UnitName.Length > 24)
                {
                    return TodayOrdValidationStatus.InvalidUnitName;
                }
                if (UnitSbt != 0 && UnitSbt != 1 && UnitSbt != 2)
                {
                    return TodayOrdValidationStatus.InvalidUnitSbt;
                }
                if (TermVal < 0)
                {
                    return TodayOrdValidationStatus.InvalidTermVal;
                }
                if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
                {
                    return TodayOrdValidationStatus.InvalidSyohoKbn;
                }
                if (!(SyohoLimitKbn >= 0 && SyohoLimitKbn <= 3))
                {
                    return TodayOrdValidationStatus.InvalidSyohoLimitKbn;
                }
                if (!(YohoKbn >= 0 && YohoKbn <= 2))
                {
                    return TodayOrdValidationStatus.InvalidYohoKbn;
                }
                if (!(IsNodspRece >= 0 && IsNodspRece <= 1))
                {
                    return TodayOrdValidationStatus.InvalidIsNodspRece;
                }
                if (IpnCd.Length > 12)
                {
                    return TodayOrdValidationStatus.InvalidIpnCd;
                }
                if (IpnName.Length > 120)
                {
                    return TodayOrdValidationStatus.InvalidIpnName;
                }
                if (!(JissiKbn >= 0 && JissiKbn <= 1))
                {
                    return TodayOrdValidationStatus.InvalidJissiKbn;
                }
                if (JissiId < 0)
                {
                    return TodayOrdValidationStatus.InvalidJissiId;
                }
                if (JissiMachine.Length > 60)
                {
                    return TodayOrdValidationStatus.InvalidJissiMachine;
                }
                if (ReqCd.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidReqCd;
                }
                if (Bunkatu.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidBunkatuLength;
                }
                if (CmtName.Length > 240)
                {
                    return TodayOrdValidationStatus.InvalidCmtName;
                }
                if (CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmtOpt;
                }
                if (FontColor.Length > 8)
                {
                    return TodayOrdValidationStatus.InvalidFontColor;
                }
                if (!(CommentNewline >= 0 && CommentNewline <= 1))
                {
                    return TodayOrdValidationStatus.InvalidCommentNewline;
                }
            }
            #endregion

            #region Validate business

            if ((!string.IsNullOrEmpty(UnitName) && Suryo == 0) || (string.IsNullOrEmpty(UnitName) && Suryo > 0))
            {
                return TodayOrdValidationStatus.InvalidSuryo;
            }
            if (!KohatuKbns.ContainsValue(KohatuKbn))
            {
                return TodayOrdValidationStatus.InvalidKohatuKbn;
            }

            if (!DrugKbns.ContainsValue(DrugKbn))
            {
                return TodayOrdValidationStatus.InvalidDrugKbn;
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit))
            {
                if (string.IsNullOrEmpty(DisplayedQuantity))
                {
                    return TodayOrdValidationStatus.InvalidQuantityUnit;
                }
                else if (Suryo > 0 && (Price > 0 && Suryo * Price > 999999999))
                {
                    return TodayOrdValidationStatus.InvalidPrice;
                }
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit) && (YohoKbn == 1 && Suryo > 999))
            {
                return TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull;
            }

            if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu && (Suryo == 0 && string.IsNullOrWhiteSpace(Bunkatu)))
            {
                return TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu;
            }

            if (ItemCd == ItemCdConst.Con_Refill && Suryo > RefillSetting)
            {
                return TodayOrdValidationStatus.InvalidSuryoOfReffill;
            }

            if (Is840Cmt && (CmtCol1 > 0 && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))))
            {
                return TodayOrdValidationStatus.InvalidCmt840;
            }

            if (Is842Cmt)
            {
                if (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt842;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38;
                }
            }

            if (Is830Cmt)
            {
                string fullSpace = @"　";

                if (string.IsNullOrEmpty(CmtOpt) && CmtOpt != fullSpace)
                {
                    return TodayOrdValidationStatus.InvalidCmt830CmtOpt;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38;
                }
            }

            if (Is831Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return TodayOrdValidationStatus.InvalidCmt831;
            }

            if (Is850Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt850(CmtOpt, ItemName);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    if (CmtName.Contains('日'))
                    {
                        return TodayOrdValidationStatus.InvalidCmt850Date;
                    }
                    else
                    {
                        return TodayOrdValidationStatus.InvalidCmt850OtherDate;
                    }
                }
            }

            if (Is851Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt851(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt851;
                }
            }

            if (Is852Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt852(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt852;
                }
            }

            if (Is853Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt853(CmtOpt, SinDate);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt853;
                }
            }

            if (Is880Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return TodayOrdValidationStatus.InvalidCmt880;
            }
            #endregion

            return TodayOrdValidationStatus.Valid;
        }
    }
}
