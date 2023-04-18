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
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidUserId, new());
            }
            if (raiinNos.Count() < 1)
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new());
            }
            if (!_raiinInfRepository.CheckExistOfRaiinNos(raiinNos))
            {
                return new DeleteReceptionOutputData(DeleteReceptionStatus.InvalidRaiinNo, new());
            }

            var result = _raiinInfRepository.Delete(inputData.HpId, inputData.UserId, raiinNos);

            if (result.Count > 0)
            {
                //Item1: SinDate, Item2: RaiinNo, Item3: PtId
                return new DeleteReceptionOutputData(DeleteReceptionStatus.Successed, result.Select(r => new DeleteReceptionItem(r.Item1, r.Item2, r.Item3)).ToList());
            }

            return new DeleteReceptionOutputData(DeleteReceptionStatus.Failed, new());
        }
        finally
        {
            _raiinInfRepository.ReleaseResource();
        }
    }
}