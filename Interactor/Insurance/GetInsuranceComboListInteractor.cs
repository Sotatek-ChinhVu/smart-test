using Domain.Models.Insurance;
using UseCase.Insurance.GetComboList;

namespace Interactor.Insurance
{
    public class GetInsuranceComboListInteractor : IGetInsuranceComboListInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public GetInsuranceComboListInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public GetInsuranceComboListOutputData Handle(GetInsuranceComboListInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetInsuranceComboListOutputData(new(), GetInsuranceComboListStatus.InvalidHpId);
                }


                if (inputData.PtId < 0)
                {
                    return new GetInsuranceComboListOutputData(new(), GetInsuranceComboListStatus.InvalidPtId);
                }


                if (inputData.SinDate < 0)
                {
                    return new GetInsuranceComboListOutputData(new(), GetInsuranceComboListStatus.InvalidSinDate);
                }

                var data = _insuranceResponsitory.GetInsuranceList(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new GetInsuranceComboListOutputData(data.Select(x => new GetInsuranceComboItemOuputData(x.HokenPid, x.HokenName, x.IsExpirated)).ToList(), GetInsuranceComboListStatus.Successed);
            }
            catch
            {
                return new GetInsuranceComboListOutputData(new(), GetInsuranceComboListStatus.Successed);
            }
        }
    }
}