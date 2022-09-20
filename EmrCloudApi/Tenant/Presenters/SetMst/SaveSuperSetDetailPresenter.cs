using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SuperSetDetail.SaveSuperSetDetail;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class SaveSuperSetDetailPresenter : ISaveSuperSetDetailOutputPort
{
    public Response<SaveSuperSetDetailResponse> Result { get; private set; } = new();

    public void Complete(SaveSuperSetDetailOutputData output)
    {
        Result.Data = new SaveSuperSetDetailResponse(output.Status == SaveSuperSetDetailStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveSuperSetDetailStatus status) => status switch
    {
        SaveSuperSetDetailStatus.Successed => ResponseMessage.Success,
        SaveSuperSetDetailStatus.Failed => ResponseMessage.Failed,
        SaveSuperSetDetailStatus.SaveSetByomeiFailed => ResponseMessage.SaveSetByomeiFailed,
        SaveSuperSetDetailStatus.SaveSetKarteInfFailed => ResponseMessage.SaveSetKarteInfFailed,
        SaveSuperSetDetailStatus.SaveSetOrderInfFailed => ResponseMessage.SaveSetOrderInfFailed,
        SaveSuperSetDetailStatus.InvalidSetByomeiId => ResponseMessage.InvalidSetByomeiId,
        SaveSuperSetDetailStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveSuperSetDetailStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        SaveSuperSetDetailStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveSuperSetDetailStatus.InvalidSikkanKbn => ResponseMessage.InvalidSikkanKbn,
        SaveSuperSetDetailStatus.InvalidNanByoCd => ResponseMessage.InvalidNanByoCd,
        SaveSuperSetDetailStatus.InvalidByomeiCdOrSyusyokuCd => ResponseMessage.InvalidByomeiCdOrSyusyokuCd,
        SaveSuperSetDetailStatus.FullByomeiMaxlength160 => ResponseMessage.FullByomeiMaxlength160,
        SaveSuperSetDetailStatus.ByomeiCmtMaxlength80 => ResponseMessage.ByomeiCmtMaxlength80,
        SaveSuperSetDetailStatus.SetCdNotExist => ResponseMessage.SetCdNotExist,
        SaveSuperSetDetailStatus.InvalidSetOrderInfId => ResponseMessage.InvalidSetOrderInfId,
        SaveSuperSetDetailStatus.InvalidSetOrderInfRpNo => ResponseMessage.InvalidSetOrderInfRpNo,
        SaveSuperSetDetailStatus.InvalidSetOrderInfRpEdaNo => ResponseMessage.InvalidSetOrderInfRpEdaNo,
        SaveSuperSetDetailStatus.InvalidSetOrderInfKouiKbn => ResponseMessage.InvalidSetOrderInfKouiKbn,
        SaveSuperSetDetailStatus.RpNameMaxLength240 => ResponseMessage.RpNameMaxLength240,
        SaveSuperSetDetailStatus.InvalidSetOrderInfInoutKbn => ResponseMessage.InvalidSetOrderInfInoutKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderInfSikyuKbn => ResponseMessage.InvalidSetOrderInfSikyuKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderInfSyohoSbt => ResponseMessage.InvalidSetOrderInfSyohoSbt,
        SaveSuperSetDetailStatus.InvalidSetOrderInfSanteiKbn => ResponseMessage.InvalidSetOrderInfSanteiKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderInfTosekiKbn => ResponseMessage.InvalidSetOrderInfTosekiKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderInfDaysCnt => ResponseMessage.InvalidSetOrderInfDaysCnt,
        SaveSuperSetDetailStatus.InvalidSetOrderInfSortNo => ResponseMessage.InvalidSetOrderInfSortNo,
        SaveSuperSetDetailStatus.InvalidSetOrderSinKouiKbn => ResponseMessage.InvalidSetOrderSinKouiKbn,
        SaveSuperSetDetailStatus.ItemCdMaxLength10 => ResponseMessage.ItemCdMaxLength10,
        SaveSuperSetDetailStatus.ItemNameMaxLength240 => ResponseMessage.ItemNameMaxLength240,
        SaveSuperSetDetailStatus.UnitNameMaxLength24 => ResponseMessage.UnitNameMaxLength24,
        SaveSuperSetDetailStatus.InvalidSetOrderSuryo => ResponseMessage.InvalidSetOrderSuryo,
        SaveSuperSetDetailStatus.InvalidSetOrderUnitSBT => ResponseMessage.InvalidSetOrderUnitSBT,
        SaveSuperSetDetailStatus.InvalidSetOrderTermVal => ResponseMessage.InvalidSetOrderTermVal,
        SaveSuperSetDetailStatus.InvalidSetOrderKohatuKbn => ResponseMessage.InvalidSetOrderKohatuKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderSyohoKbn => ResponseMessage.InvalidSetOrderSyohoKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderSyohoLimitKbn => ResponseMessage.InvalidSetOrderSyohoLimitKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderDrugKbn => ResponseMessage.InvalidSetOrderDrugKbn,
        SaveSuperSetDetailStatus.InvalidSetOrderYohoKbn => ResponseMessage.InvalidSetOrderYohoKbn,
        SaveSuperSetDetailStatus.Kokuji1MaxLength1 => ResponseMessage.Kokuji1MaxLength1,
        SaveSuperSetDetailStatus.Kokuji2MaxLength1 => ResponseMessage.Kokuji2MaxLength1,
        SaveSuperSetDetailStatus.InvalidSetOrderIsNodspRece => ResponseMessage.InvalidSetOrderIsNodspRece,
        SaveSuperSetDetailStatus.IpnCdMaxLength12 => ResponseMessage.IpnCdMaxLength12,
        SaveSuperSetDetailStatus.IpnNameMaxLength120 => ResponseMessage.IpnNameMaxLength120,
        SaveSuperSetDetailStatus.BunkatuMaxLength10 => ResponseMessage.BunkatuMaxLength10,
        SaveSuperSetDetailStatus.CmtNameMaxLength240 => ResponseMessage.CmtNameMaxLength240,
        SaveSuperSetDetailStatus.CmtOptMaxLength38 => ResponseMessage.CmtOptMaxLength38,
        SaveSuperSetDetailStatus.FontColorMaxLength8 => ResponseMessage.FontColorMaxLength8,
        SaveSuperSetDetailStatus.InvalidSetOrderCommentNewline => ResponseMessage.InvalidSetOrderCommentNewline,

        _ => string.Empty
    };
}
