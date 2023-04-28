using Domain.Models.OrdInfs;
using UseCase.OrdInfs.CheckOrdInfInDrug;

namespace Interactor.OrdInfs
{
    public class CheckOrdInfInDrugInteractor : ICheckOrdInfInDrugInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;

        public CheckOrdInfInDrugInteractor(IOrdInfRepository ordInfRepository)
        {
            _ordInfRepository = ordInfRepository;
        }

        public CheckOrdInfInDrugOutputData Handle(CheckOrdInfInDrugInputData inputData)
        {
            try
            {
                if (inputData.RaiinNo <= 0)
                {
                    return new CheckOrdInfInDrugOutputData(false, CheckOrdInfInDrugStatus.InvalidRaiinNo);
                }
                if (inputData.HpId <= 0)
                {
                    return new CheckOrdInfInDrugOutputData(false, CheckOrdInfInDrugStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckOrdInfInDrugOutputData(false, CheckOrdInfInDrugStatus.InvalidPtId);
                }

                var result = _ordInfRepository.CheckOrdInfInDrug(inputData.HpId, inputData.PtId, inputData.RaiinNo);

                return new CheckOrdInfInDrugOutputData(result, CheckOrdInfInDrugStatus.Successed);
            }
            finally
            {
                _ordInfRepository.ReleaseResource();
            }
        }
    }
}
