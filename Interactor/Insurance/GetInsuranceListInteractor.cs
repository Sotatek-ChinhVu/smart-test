using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.GetList;

namespace Interactor.Insurance
{
    public class GetInsuranceListInteractor : IGetInsuranceListByIdInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public GetInsuranceListInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public GetInsuranceListByIdOutputData Handle(GetInsuranceListInputData inputData)
        {
            if (inputData.PtId < 0)
            {
                return new GetInsuranceListByIdOutputData(new List<InsuranceModel>(), GetInsuranceListStatus.InvalidPtId);
            }
            if (inputData.HpId < 0)
            {
                return new GetInsuranceListByIdOutputData(new List<InsuranceModel>(), GetInsuranceListStatus.InvalidHpId);
            }

            var data = _insuranceResponsitory.GetInsuranceListById(inputData.HpId, inputData.PtId, inputData.SinDate);
            return new GetInsuranceListByIdOutputData(data.ToList(), GetInsuranceListStatus.Successed);
        }
    }
}