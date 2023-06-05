using Domain.Models.NextOrder;
using UseCase.NextOrder.CheckNextOrdHaveOdr;
namespace Interactor.NextOrder
{
    public class CheckNextOrdHaveInteractor : ICheckNextOrdHaveOdrInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;

        public CheckNextOrdHaveInteractor(INextOrderRepository nextOrderRepository)
        {
            _nextOrderRepository = nextOrderRepository;
        }

        public CheckNextOrdHaveOdrOutputData Handle(CheckNextOrdHaveOdrInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckNextOrdHaveOdrOutputData(CheckNextOrdHaveOdrStatus.InvalidHpId, false);
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckNextOrdHaveOdrOutputData(CheckNextOrdHaveOdrStatus.InvalidPtId, false);
                }
                if (inputData.SinDate <= 0)
                {
                    return new CheckNextOrdHaveOdrOutputData(CheckNextOrdHaveOdrStatus.InvalidSinDate, false);
                }

                var result = _nextOrderRepository.CheckNextOrdHaveOdr(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new CheckNextOrdHaveOdrOutputData(CheckNextOrdHaveOdrStatus.Successed, result);
            }
            finally
            {
                _nextOrderRepository.ReleaseResource();
            }
        }
    }
}
