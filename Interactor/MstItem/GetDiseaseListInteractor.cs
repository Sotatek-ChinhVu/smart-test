using Domain.Models.MstItem;
using UseCase.MstItem.GetDiseaseList;

namespace Interactor.MstItem;

public class GetDiseaseListInteractor : IGetDiseaseListInputPort
{
    private readonly IMstItemRepository _inputItemRepository;

    public GetDiseaseListInteractor(IMstItemRepository inputItemRepository)
    {
        _inputItemRepository = inputItemRepository;
    }

    public GetDiseaseListOutputData Handle(GetDiseaseListInputData inputData)
    {
        try
        {
            var itemCdList = inputData.ItemCdList.Distinct().ToList();
            var result = _inputItemRepository.DiseaseSearch(inputData.HpId, itemCdList);
            return new GetDiseaseListOutputData(result, GetDiseaseListStatus.Successed);
        }
        finally
        {
            _inputItemRepository.ReleaseResource();
        }
    }
}
