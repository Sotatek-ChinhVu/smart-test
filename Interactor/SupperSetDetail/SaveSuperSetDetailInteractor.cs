using Domain.Models.SuperSetDetail;
using UseCase.SupperSetDetail.SaveSuperSetDetail;
using UseCase.SupperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;

namespace Interactor.SupperSetDetail;

public class SaveSupperSetDetailInteractor : ISaveSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveSupperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public SaveSuperSetDetailOutputData Handle(SaveSuperSetDetailInputData inputData)
    {
        try
        {
            var result = _superSetDetailRepository.SaveSuperSetDetail(inputData.SetCd, inputData.UserId, ConvertToSuperSetDetailModel(inputData));
            return new SaveSuperSetDetailOutputData(result, SaveSuperSetDetailStatus.Successed);
        }
        catch
        {
            return new SaveSuperSetDetailOutputData(SaveSuperSetDetailStatus.Failed);
        }
    }

    private SuperSetDetailModel ConvertToSuperSetDetailModel(SaveSuperSetDetailInputData inputData)
    {
        return new SuperSetDetailModel(
                ConvertToListSetByomeiModel(inputData.SaveSupperSetDetailInput.SetByomeiModelInputs ?? new List<SaveSetByomeiInputItem>()),
                new SetKarteInfModel()
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
}
