using Domain.Models.InsuranceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InsuranceList.GetInsuranceListById;

namespace Interactor.InsuranceList
{
    public class GetInsuranceListByIdInteractor: IGetInsuranceListByIdInputPort
    {
        private readonly IInsuranceListResponsitory _insuranceListResponsitory;
        public GetInsuranceListByIdInteractor(IInsuranceListResponsitory insuranceListResponsitory)
        {
            _insuranceListResponsitory = insuranceListResponsitory;
        }

        public GetInsuranceListByIdOutputData Handle(GetInsuranceListByIdInputData inputData)
        {
            var data = _insuranceListResponsitory.GetInsuranceListById(inputData.PtId);
            return new GetInsuranceListByIdOutputData(data.ToList());
        }
    }
}
