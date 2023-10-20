using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Reception;
using Entity.Tenant;
using Microsoft.Extensions.Options;
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
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.InvalidHpId, new());
            }
            if (inputData.RaiinNo <= 0)
            {
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.InvalidRaiinNo, new());
            }

            var result = _receptionRepository.UpdateIsDeleted(inputData.HpId, inputData.RaiinNo);
            if (result)
            {
                var reception = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId);
                return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.Success, reception);
            }
            else
            {
                Thread.Sleep(2000);
                result = _receptionRepository.UpdateIsDeleted(inputData.HpId, inputData.RaiinNo);
                if (result)
                {
                    var reception = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, inputData.RaiinNo, inputData.PtId);
                    return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.Success, reception);
                }
            }

            return new RevertDeleteNoReceptOutputData(RevertDeleteNoReceptStatus.Success, new());
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}