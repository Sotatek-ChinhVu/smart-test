using Domain.Models.MstItem;
using Helper.Common;
using UseCase.MstItem.DiseaseSearch;

namespace Interactor.Byomei;

public class DiseaseSearchInteractor : IDiseaseSearchInputPort
{
    private readonly IMstItemRepository _inputItemRepository;

    public DiseaseSearchInteractor(IMstItemRepository inputItemRepository)
    {
        _inputItemRepository = inputItemRepository;
    }

    public DiseaseSearchOutputData Handle(DiseaseSearchInputData inputData)
    {
        try
        {
            if (inputData.PageSize < 1)
            {
                return new DiseaseSearchOutputData(DiseaseSearchStatus.InvalidPageCount);
            }
            if (inputData.PageIndex < 1)
            {
                return new DiseaseSearchOutputData(DiseaseSearchStatus.InvalidPageIndex);
            }

            var listData = _inputItemRepository.DiseaseSearch(inputData.IsPrefix, inputData.IsByomei, inputData.IsSuffix, inputData.IsMisaiyou, inputData.Keyword, inputData.Sindate, inputData.PageIndex, inputData.PageSize);
            return new DiseaseSearchOutputData(listData, DiseaseSearchStatus.Successed);
        }
        catch (Exception)
        {
            return new DiseaseSearchOutputData(DiseaseSearchStatus.Failed);
        }
    }
}