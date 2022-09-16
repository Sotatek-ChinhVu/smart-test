using Domain.Models.SuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;

namespace Interactor.SuperSetDetail;

public class SaveSuperSetDetailInteractor : ISaveSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveSuperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public SaveSuperSetDetailOutputData Handle(SaveSuperSetDetailInputData inputData)
    {
        try
        {
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
                ConvertToSetKarteInfModel(inputData.SaveSetKarteInputItem ?? new SaveSetKarteInputItem())
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

}
