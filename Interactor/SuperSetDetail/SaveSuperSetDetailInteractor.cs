using Domain.Models.MstItem;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;

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

            var result = _superSetDetailRepository.SaveSuperSetDetail(inputData.SetCd, inputData.UserId, inputData.HpId, ConvertToSuperSetDetailModel(inputData));
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

    private SuperSetDetailModel ConvertToSuperSetDetailModel(SaveSuperSetDetailInputData inputData)
    {
        return new SuperSetDetailModel(
                    ConvertToListSetByomeiModel(inputData.SetByomeiModelInputs ?? new List<SaveSetByomeiInputItem>()),
                    ConvertToSetKarteInfModel(inputData.SaveSetKarteInputItem ?? new SaveSetKarteInputItem()),
                    new()
                );
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
                return SaveSuperSetDetailStatus.Maxlength160;
            }
            else if (item.ByomeiCmt.Length > 80)
            {
                return SaveSuperSetDetailStatus.Maxlength80;
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


        return SaveSuperSetDetailStatus.ValidateSuccess;
    }
}
