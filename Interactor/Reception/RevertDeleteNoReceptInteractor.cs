using Domain.Models.Reception;
using UseCase.Reception.RevertDeleteNoRecept;

namespace Interactor.Reception;

public class RevertDeleteNoReceptInteractor : IRevertDeleteNoReceptInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public RevertDeleteNoReceptInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public RevertDeleteNoReceptOutputData Handle(RevertDeleteNoReceptInputData inputData)
    {
        try
        {
            if (inputData.HpId < 0)
            {
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.InvalidHpId);
            }
            if (inputData.RaiinNo <= 0)
            {
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.InvalidRaiinNo);
            }

            var result = _receptionRepository.UpdateIsDeleted(inputData.HpId, inputData.RaiinNo);

            if (result)
            {
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.Success);
            }

            return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.Failed);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}