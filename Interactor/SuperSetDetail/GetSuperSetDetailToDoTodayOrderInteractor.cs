using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace Interactor.SuperSetDetail;

public class GetSuperSetDetailToDoTodayOrderInteractor : IGetSuperSetDetailToDoTodayOrderInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetSuperSetDetailToDoTodayOrderInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetSuperSetDetailToDoTodayOrderOutputData Handle(GetSuperSetDetailToDoTodayOrderInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidUserId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidSetCd);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidSinDate);
            }

            var result = _superSetDetailRepository.GetSuperSetDetailForTodayOrder(inputData.HpId, inputData.UserId, inputData.SetCd, inputData.SinDate);
            if (result.Item1.Count == 0 && result.Item2.Count == 0 && result.Item3.Count == 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.NoData);
            }

            return new GetSuperSetDetailToDoTodayOrderOutputData(ConvertSetByomeiToItem(result.Item1), result.Item2, ConvertSetOrderInfToItem(result.Item3), GetSuperSetDetailToDoTodayOrderStatus.Successed);
        }
        catch
        {
            return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.Failed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
        }
    }

    private List<SetByomeiItem> ConvertSetByomeiToItem(List<SetByomeiModel> setByomeiModels)
    {
        return setByomeiModels.Select(s => new SetByomeiItem(
                s.IsSyobyoKbn,
                s.SikkanKbn,
                s.NanByoCd,
                s.FullByomei,
                s.IsSuspected,
                s.IsDspRece,
                s.IsDspKarte,
                s.ByomeiCmt,
                s.ByomeiCd,
                s.Icd10,
                s.Icd102013,
                s.Icd1012013,
                s.Icd1022013,
                s.PrefixSuffixList
            )).ToList();
    }

    private List<SetOrderInfItem> ConvertSetOrderInfToItem(List<SetOrderInfModel> setOrderInfModels)
    {
        return setOrderInfModels.Select(o => new SetOrderInfItem(
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
                o.GroupKoui.Value,
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
                         od.CenterItemCd1,
                         od.CenterItemCd2,
                         od.Kasan1,
                         od.Kasan2,
                         od.YohoSets
                    )
                ).ToList()
            )).ToList();
    }
}
