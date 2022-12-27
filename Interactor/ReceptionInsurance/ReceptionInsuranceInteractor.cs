using Domain.Models.ReceptionInsurance;
using UseCase.ReceptionInsurance.Get;

namespace Interactor.ReceptionInsurance
{
    public class ReceptionInsuranceInteractor : IGetReceptionInsuranceInputPort
    {
        private readonly IReceptionInsuranceRepository _receptionInsuranceRepository;
        public ReceptionInsuranceInteractor(IReceptionInsuranceRepository receptionInsuranceRepository)
        {
            _receptionInsuranceRepository = receptionInsuranceRepository;
        }

        public GetReceptionInsuranceOutputData Handle(GetReceptionInsuranceInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetReceptionInsuranceOutputData(new List<ReceptionInsuranceModel>(), GetReceptionInsuranceStatus.InvalidHpId);
                }

                if (inputData.PtId <= 0)
                {
                    return new GetReceptionInsuranceOutputData(new List<ReceptionInsuranceModel>(), GetReceptionInsuranceStatus.InvalidHpId);
                }

                if (inputData.SinDate <= 0)
                {
                    return new GetReceptionInsuranceOutputData(new List<ReceptionInsuranceModel>(), GetReceptionInsuranceStatus.InvalidHpId);
                }

                var data = _receptionInsuranceRepository.GetReceptionInsurance(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IsShowExpiredReception);

                return new GetReceptionInsuranceOutputData(data.ToList(), GetReceptionInsuranceStatus.Successed);
            }
            finally
            {
                _receptionInsuranceRepository.ReleaseResource();
            }
        }
    }
}
