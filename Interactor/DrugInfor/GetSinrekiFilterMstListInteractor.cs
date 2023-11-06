using Domain.Models.DrugInfor;
using UseCase.DrugInfor.GetSinrekiFilterMstList;

namespace Interactor.DrugInfor;

public class GetSinrekiFilterMstListInteractor : IGetSinrekiFilterMstListInputPort
{
    private readonly IDrugInforRepository _drugInforRepository;

    public GetSinrekiFilterMstListInteractor(IDrugInforRepository drugInforRepository)
    {
        _drugInforRepository = drugInforRepository;
    }

    public GetSinrekiFilterMstListOutputData Handle(GetSinrekiFilterMstListInputData inputData)
    {
        try
        {
            var result = _drugInforRepository.GetSinrekiFilterMstList(inputData.HpId);
            return new GetSinrekiFilterMstListOutputData(result, GetSinrekiFilterMstListStatus.Successed);
        }
        finally
        {
            _drugInforRepository.ReleaseResource();
        }
    }
}
