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
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidSetCd);
            }

            var result = _superSetDetailRepository.GetSuperSetDetailForTodayOrder(inputData.HpId, inputData.SetCd);
            if (result.Item1.Count == 0 && result.Item2.Count == 0 && result.Item3.Count == 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.NoData);
            }

            return new GetSuperSetDetailToDoTodayOrderOutputData(ConvertSetByomeiToItem(result.Item1), ConvertSetByomeiToItem(result.Item2), ConvertSetOrderInfToItem(result.Item3), GetSuperSetDetailToDoTodayOrderStatus.Successed);
        }
        catch
        {
            return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.Failed);
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
                s.PrefixSuffixList
            )).ToList();
    }

    private List<SetKarteInfItem> ConvertSetByomeiToItem(List<SetKarteInfModel> setKarteInfModels)
    {
        return setKarteInfModels.Select(k => new SetKarteInfItem(
                k.HpId,
                k.SetCd,
                k.RichText
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
                         od.InOutKbn,
                         od.CommentNewline
                    )
                ).ToList()
            )).ToList();
    }
}
