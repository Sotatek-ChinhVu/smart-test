using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.GetSuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace Interactor.SuperSetDetail;

public class GetSuperSetDetailInteractor : IGetSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetSuperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetSuperSetDetailOutputData Handle(GetSuperSetDetailInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidHpId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSetCd);
            }
            else if (inputData.Sindate <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSindate);
            }
            var result = _superSetDetailRepository.GetSuperSetDetail(inputData.HpId, inputData.SetCd, inputData.Sindate);
            return new GetSuperSetDetailOutputData(ConvertSuperSetDetailToItem(result), GetSuperSetDetailListStatus.Successed);
        }
        catch
        {
            return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.Failed);
        }
    }

    private List<SetByomeiItem> ConvertSetByomeiToItem(List<SetByomeiModel> setByomeiModels)
    {
        return setByomeiModels.Select(s => new SetByomeiItem(
                s.Id,
                s.IsSyobyoKbn,
                s.SikkanKbn,
                s.NanByoCd,
                s.FullByomei,
                s.IsSuspected,
                s.IsDspRece,
                s.IsDspKarte,
                s.ByomeiCmt,
                s.ByomeiCd,
                s.PrefixSuffixList
            )).ToList();
    }

    private List<SetOrderInfItem> ConvertSetOrderInfToItem(List<SetOrderInfModel> setOrderInfModels)
    {
        return setOrderInfModels.Select(o => new SetOrderInfItem(
                o.Id,
                o.HpId,
                o.SetCd,
                o.RpNo,
                o.RpEdaNo,
                o.OdrKouiKbn,
                o.RpName,
                o.InoutKbn,
                o.SikyuKbn,
                o.SyohoSbt,
                o.SanteiKbn,
                o.TosekiKbn,
                o.DaysCnt,
                o.SortNo,
                o.IsDeleted,
                o.CreateDate,
                o.CreateId,
                o.CreateName,
                o.GroupKoui.Value,
                o.IsSelfInjection,
                o.IsDrug,
                o.IsInjection,
                o.IsKensa,
                o.IsShohoComment,
                o.IsShohoBiko,
                o.IsShohosenComment,
                o.OrdInfDetails.Select(
                    od => new SetOrderInfDetailItem(
                         od.HpId,
                         od.SetCd,
                         od.RpNo,
                         od.RpEdaNo,
                         od.RowNo,
                         od.SinKouiKbn,
                         od.ItemCd,
                         od.ItemName,
                         od.DisplayItemName,
                         od.Suryo,
                         od.UnitName,
                         od.UnitSBT,
                         od.TermVal,
                         od.KohatuKbn,
                         od.SyohoKbn,
                         od.SyohoLimitKbn,
                         od.DrugKbn,
                         od.YohoKbn,
                         od.Kokuji1,
                         od.Kokuji2,
                         od.IsNodspRece,
                         od.IpnCd,
                         od.IpnName,
                         od.Bunkatu,
                         od.CmtName,
                         od.CmtOpt,
                         od.FontColor,
                         od.CommentNewline,
                         od.MasterSbt,
                         od.InOutKbn,
                         od.Yakka,
                         od.IsGetPriceInYakka,
                         od.Ten,
                         od.BunkatuKoui,
                         od.KensaGaichu,
                         od.OdrTermVal,
                         od.CnvTermVal,
                         od.YjCd,
                         od.Price,
                         od.IsSpecialItem,
                         od.IsYoho,
                         od.IsKensa,
                         od.Is820Cmt,
                         od.Is830Cmt,
                         od.Is831Cmt,
                         od.Is850Cmt,
                         od.Is851Cmt,
                         od.Is852Cmt,
                         od.Is840Cmt,
                         od.Is842Cmt,
                         od.IsShohoComment,
                         od.IsShohoBiko,
                         od.IsDrug,
                         od.IsInjection,
                         od.IsDrugUsage,
                         od.IsStandardUsage,
                         od.IsSuppUsage,
                         od.IsInjectionUsage,
                         od.IsNormalComment,
                         od.IsComment,
                         od.IsUsage,
                         od.DisplayedQuantity,
                         od.EditingQuantity
                    )
                ).ToList()
            )).ToList();
    }

    private List<SetGroupOrderInfItem> ConvertSetGroupOrderInfToItem(List<SetGroupOrderInfModel> setGroupOrderInfModels)
    {
        return setGroupOrderInfModels.Select(s => new SetGroupOrderInfItem(
             s.GUID,
             s.KouiCode,
             s.GroupKouiCode.Value,
             s.GroupName,
             s.IsShowInOut,
             s.InOutKbn,
             s.InOutName,
             s.IsShowSikyu,
             s.SikyuKbn,
             s.TosekiKbn,
             s.SikyuName,
             s.IsShowSantei,
             s.SanteiKbn,
             s.SanteiName,
             s.SyohoSbt,
             s.IsDrug,
             s.IsKensa,
             ConvertSetOrderInfToItem(s.ListSetOrdInfModels)
         )).ToList();
    }

    private SuperSetDetailItem ConvertSuperSetDetailToItem(SuperSetDetailModel superSetDetailModel)
    {
        return new SuperSetDetailItem(ConvertSetByomeiToItem(superSetDetailModel.SetByomeiList), superSetDetailModel.SetKarteInf, ConvertSetGroupOrderInfToItem(superSetDetailModel.SetGroupOrderInfList));
    }
}
