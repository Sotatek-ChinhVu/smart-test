using Domain.Models.Reception;
using UseCase.Reception.Delete;

namespace Interactor.Reception;

public class DeleteReceptionInteractor : IDeleteReceptionInputPort
{
    private readonly IReceptionRepository _raiinInfRepository;

    public DeleteReceptionInteractor(IReceptionRepository raiinInfRepository)
    {
        _raiinInfRepository = raiinInfRepository;
    }

    public DeleteReceptionOutputData Handle(DeleteReceptionInputData inputData)
    {
        try
        {
            var raiinNos = inputData.RaiinNos.Distinct().ToList();
            if (inputData.HpId < 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidUserId);
            }
            if (raiinNos.Count() < 1)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo);
            }
            if (!_raiinInfRepository.CheckExistOfRaiinNos(raiinNos))
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo);
            }

            var result = _raiinInfRepository.Delete(inputData.HpId, inputData.UserId, raiinNos);

            if (result)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.Successed);
            }

            return new DeleteReceptionOutputData(DeleteReceptionStatus.Failed);
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }
}