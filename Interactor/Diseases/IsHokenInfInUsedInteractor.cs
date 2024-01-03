using Domain.Models.Diseases;
using UseCase.Diseases.IsHokenInfInUsed;

namespace Interactor.Diseases;

public class IsHokenInfInUsedInteractor : IIsHokenInfInUsedInputPort
{
    private readonly IPtDiseaseRepository _ptDiseaseRepository;

    public IsHokenInfInUsedInteractor(IPtDiseaseRepository ptDiseaseRepository)
    {
        _ptDiseaseRepository = ptDiseaseRepository;
    }

    public IsHokenInfInUsedOutputData Handle(IsHokenInfInUsedInputData inputData)
    {
        try
        {
            return new IsHokenInfInUsedOutputData(_ptDiseaseRepository.IsHokenInfInUsed(inputData.HpId, inputData.PtId, inputData.HokenId), IsHokenInfInUsedStatus.Successed);
        }
        finally
        {
            _ptDiseaseRepository.ReleaseResource();
        }
    }
}
