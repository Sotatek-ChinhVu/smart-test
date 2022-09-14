using Domain.Models.SuperSetDetail;
using UseCase.SupperSetDetail.SaveSupperSetDetail;
using UseCase.SupperSetDetail.SaveSupperSetDetail.SaveSetByomeiInput;

namespace Interactor.SupperSetDetail;

public class SaveSupperSetDetailInteractor : ISaveSupperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public SaveSupperSetDetailInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public SaveSupperSetDetailOutputData Handle(SaveSupperSetDetailInputData inputData)
    {
        try
        {
            var result = _superSetDetailRepository.SaveSuperSetDetail(inputData.SetCd, inputData.UserId, ConvertToSuperSetDetailModel(inputData));
            return new SaveSupperSetDetailOutputData(result, SaveSupperSetDetailStatus.Successed);
        }
        catch
        {
            return new SaveSupperSetDetailOutputData(SaveSupperSetDetailStatus.Failed);
        }
    }

    private SuperSetDetailModel ConvertToSuperSetDetailModel(SaveSupperSetDetailInputData inputData)
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
