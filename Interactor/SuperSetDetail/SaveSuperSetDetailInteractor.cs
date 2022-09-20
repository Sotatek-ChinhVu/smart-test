using Domain.Models.MstItem;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

namespace Interactor.SuperSetDetail;

public class SaveSuperSetDetailInteractor : ISaveSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ISetMstRepository _setMstRepository;
    private const string SUSPECTED_CD = "8002";
    private const string FREE_WORD = "0000999";

    public SaveSuperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository, ISetMstRepository setMstRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
        _setMstRepository = setMstRepository;
    }

    public SaveSuperSetDetailOutputData Handle(SaveSuperSetDetailInputData inputData)
    {
        try
        {
            var statusValidate = ValidateSuperSetDetail(inputData);
            if (statusValidate != SaveSuperSetDetailStatus.ValidateSuccess)
            {
                return new SaveSuperSetDetailOutputData(statusValidate);
            }

            var result = _superSetDetailRepository.SaveSuperSetDetail(
                                                                        inputData.SetCd,
                                                                        inputData.UserId,
                                                                        inputData.HpId,
                                                                        ConvertToListSetByomeiModel(inputData.SetByomeiModelInputs),
                                                                        ConvertToSetKarteInfModel(inputData.SaveSetKarteInputItem),
                                                                        ConvertToListSetOrderInfModel(inputData.SaveSetOrderInputItems)
                                                                    );
            switch (result)
            {
                case 1:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetByomeiFailed);
                case 2:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetKarteInfFailed);
                case 3:
                    return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.SaveSetOrderInfFailed);
                default:
                    return new SaveSuperSetDetailOutputData(result, SaveSuperSetDetailStatus.Successed);
            }
        }
        catch
        {
            return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.Failed);
        }
    }

    private List<SetByomeiModel> ConvertToListSetByomeiModel(List<SaveSetByomeiInputItem> inputItems)
    {
        List<SetByomeiModel> result = new();
        foreach (var item in inputItems)
        {
            result.Add(new SetByomeiModel(
                            item.Id,
                            item.IsSyobyoKbn,
                            item.SikkanKbn,
                            item.NanByoCd,
                            item.FullByomei,
                            item.IsSuspected,
                            item.IsDspRece,
                            item.IsDspKarte,
                            item.ByomeiCmt,
                            item.ByomeiCd,
                            item.PrefixSuffixList.Select(pre =>
                                    new PrefixSuffixModel(
                                            pre.Code,
                                            pre.Name
                                        )
                                ).ToList()
                        )
                );
        }
        return result;
    }

    private SetKarteInfModel ConvertToSetKarteInfModel(SaveSetKarteInputItem inputItem)
    {
        return new SetKarteInfModel(
                inputItem.HpId,
                inputItem.SetCd,
                inputItem.RichText
            );
    }

    private List<SetOrderInfModel> ConvertToListSetOrderInfModel(List<SaveSetOrderInfInputItem> inputItems)
    {
        List<SetOrderInfModel> listSetInfModels = new();
        foreach (var inputMst in inputItems)
        {
            var listSetOrderInfDetailModels = inputMst.OrdInfDetails.Select(detail =>
                    new SetOrderInfDetailModel(
                            detail.SinKouiKbn,
                            detail.ItemCd,
                            detail.ItemName,
                            detail.ItemName,
                            detail.Suryo,
                            detail.UnitName,
                            detail.UnitSBT,
                            detail.TermVal,
                            detail.KohatuKbn,
                            detail.SyohoKbn,
                            detail.SyohoLimitKbn,
                            detail.DrugKbn,
                            detail.YohoKbn,
                            detail.Kokuji1,
                            detail.Kokuji2,
                            detail.IsNodspRece,
                            detail.IpnCd,
                            detail.IpnName,
                            detail.Bunkatu,
                            detail.CmtName,
                            detail.CmtOpt,
                            detail.FontColor,
                            detail.CommentNewline
                        )
                ).ToList();
            var model = new SetOrderInfModel(
                    inputMst.Id,
                    inputMst.RpNo,
                    inputMst.RpEdaNo,
                    inputMst.OdrKouiKbn,
                    inputMst.RpName,
                    inputMst.InoutKbn,
                    inputMst.SikyuKbn,
                    inputMst.SyohoSbt,
                    inputMst.SanteiKbn,
                    inputMst.TosekiKbn,
                    inputMst.DaysCnt,
                    inputMst.SortNo,
                    inputMst.IsDeleted,
                    listSetOrderInfDetailModels
                );
            listSetInfModels.Add(model);
        }

        return listSetInfModels;
    }

    private SaveSuperSetDetailStatus ValidateSuperSetDetail(SaveSuperSetDetailInputData inputData)
    {
        if (inputData.HpId <= 0)
        {
            return SaveSuperSetDetailStatus.InvalidHpId;
        }
        else if (inputData.SetCd < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetCd;
        }
        else if (inputData.UserId <= 0)
        {
            return SaveSuperSetDetailStatus.InvalidUserId;
        }
        else if (!_setMstRepository.CheckExistSetMstBySetCd(inputData.SetCd))
        {
            return SaveSuperSetDetailStatus.SetCdNotExist;
        }

        // Validate SetByomeiModel
        List<string> listByomeiCode = new();
        foreach (var item in inputData.SetByomeiModelInputs)
        {
            listByomeiCode.AddRange(item.PrefixSuffixList.Where(item => item.Code != SUSPECTED_CD).Select(item => item.Code).ToList());
            if (item.ByomeiCd != FREE_WORD)
            {
                listByomeiCode.Add(item.ByomeiCd);
            }
            if (item.Id < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetByomeiId;
            }
            else if (item.SikkanKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSikkanKbn;
            }
            else if (item.NanByoCd < 0)
            {
                return SaveSuperSetDetailStatus.InvalidNanByoCd;
            }
            else if (item.FullByomei.Length > 160)
            {
                return SaveSuperSetDetailStatus.FullByomeiMaxlength160;
            }
            else if (item.ByomeiCmt.Length > 80)
            {
                return SaveSuperSetDetailStatus.ByomeiCmtMaxlength80;
            }
        }
        var listByomeiCd = _mstItemRepository.DiseaseSearch(listByomeiCode).Select(item => item.ByomeiCd).ToList();
        foreach (var item in inputData.SetByomeiModelInputs.Select(item => item.PrefixSuffixList))
        {
            foreach (var presufCode in item.Select(item => item.Code))
            {
                if (!(presufCode == SUSPECTED_CD || presufCode == FREE_WORD || listByomeiCd.Any(code => code == presufCode)))
                {
                    return SaveSuperSetDetailStatus.InvalidByomeiCdOrSyusyokuCd;
                }
            }
        }

        // validate Order
        foreach (var mst in inputData.SaveSetOrderInputItems)
        {
            if (mst.Id < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfId;
            }
            else if (mst.RpNo < 1)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfRpNo;
            }
            else if (mst.RpEdaNo < 1)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfRpEdaNo;
            }
            else if (mst.OdrKouiKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfKouiKbn;
            }
            else if (mst.RpName.Length <= 240)
            {
                return SaveSuperSetDetailStatus.RpNameMaxLength240;
            }
            else if (mst.InoutKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfInoutKbn;
            }
            else if (mst.SikyuKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfSikyuKbn;
            }
            else if (mst.SyohoSbt < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfSyohoSbt;
            }
            else if (mst.SanteiKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfSanteiKbn;
            }
            else if (mst.TosekiKbn < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfTosekiKbn;
            }
            else if (mst.DaysCnt < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfDaysCnt;
            }
            else if (mst.SortNo < 0)
            {
                return SaveSuperSetDetailStatus.InvalidSetOrderInfSortNo;
            }
            foreach (var detail in mst.OrdInfDetails)
            {
                if (detail.SinKouiKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderSinKouiKbn;
                }
                else if (detail.ItemCd.Length <= 10)
                {
                    return SaveSuperSetDetailStatus.ItemCdMaxLength10;
                }
                else if (detail.ItemName.Length <= 240)
                {
                    return SaveSuperSetDetailStatus.ItemNameMaxLength240;
                }
                else if (detail.UnitName.Length <= 24)
                {
                    return SaveSuperSetDetailStatus.UnitNameMaxLength24;
                }
                else if (detail.Suryo < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderSuryo;
                }
                else if (detail.UnitSBT < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderUnitSBT;
                }
                else if (detail.TermVal < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderTermVal;
                }
                else if (detail.KohatuKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderKohatuKbn;
                }
                else if (detail.SyohoKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderSyohoKbn;
                }
                else if (detail.SyohoLimitKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderSyohoLimitKbn;
                }
                else if (detail.DrugKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderDrugKbn;
                }
                else if (detail.YohoKbn < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderYohoKbn;
                }
                else if (detail.Kokuji1.Length <= 1)
                {
                    return SaveSuperSetDetailStatus.Kokuji1MaxLength1;
                }
                else if (detail.Kokuji2.Length <= 1)
                {
                    return SaveSuperSetDetailStatus.Kokuji2MaxLength1;
                }
                else if (detail.IsNodspRece < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderIsNodspRece;
                }
                else if (detail.IpnCd.Length <= 12)
                {
                    return SaveSuperSetDetailStatus.IpnCdMaxLength12;
                }
                else if (detail.IpnName.Length <= 120)
                {
                    return SaveSuperSetDetailStatus.IpnNameMaxLength120;
                }
                else if (detail.Bunkatu.Length <= 10)
                {
                    return SaveSuperSetDetailStatus.BunkatuMaxLength10;
                }
                else if (detail.CmtName.Length <= 240)
                {
                    return SaveSuperSetDetailStatus.CmtNameMaxLength240;
                }
                else if (detail.CmtOpt.Length <= 38)
                {
                    return SaveSuperSetDetailStatus.CmtOptMaxLength38;
                }
                else if (detail.FontColor.Length <= 8)
                {
                    return SaveSuperSetDetailStatus.FontColorMaxLength8;
                }
                else if (detail.CommentNewline < 0)
                {
                    return SaveSuperSetDetailStatus.InvalidSetOrderCommentNewline;
                }
            }
        }

        return SaveSuperSetDetailStatus.ValidateSuccess;
    }
}
