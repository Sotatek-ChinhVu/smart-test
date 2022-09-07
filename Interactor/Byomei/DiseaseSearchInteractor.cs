using Domain.Models.Byomei;
using UseCase.Byomei.DiseaseSearch;

namespace Interactor.Byomei;

public class DiseaseSearchInteractor : IDiseaseSearchInputPort
{
    private readonly IByomeiRepository _byomeiRepository;

    public DiseaseSearchInteractor(IByomeiRepository byomeiRepository)
    {
        _byomeiRepository = byomeiRepository;
    }

    public DiseaseSearchOutputData Handle(DiseaseSearchInputData inputData)
    {
        try
        {
            if (inputData.PageCount < 1)
            {
                return new DiseaseSearchOutputData(DiseaseSearchStatus.InvalidPageCount);
            }
            if (inputData.PageIndex < 1)
            {
                return new DiseaseSearchOutputData(DiseaseSearchStatus.InvalidPageIndex);
            }
            var listData = _byomeiRepository.DiseaseSearch(inputData.IsSyusyoku, inputData.Keyword, inputData.PageIndex, inputData.PageCount);
            return new DiseaseSearchOutputData(listData, DiseaseSearchStatus.Successed);
        }
        catch (Exception)
        {
            return new DiseaseSearchOutputData(DiseaseSearchStatus.Failed);
        }
    }
}
