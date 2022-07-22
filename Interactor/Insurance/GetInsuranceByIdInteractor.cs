using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.GetById;
using UseCase.Insurance.GetList;

namespace Interactor.Insurance
{
    public class GetInsuranceByIdInteractor : IGetInsuranceByIdInputPort
    {
        private readonly IInsuranceRepository _insuranceResponsitory;
        public GetInsuranceByIdInteractor(IInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public GetInsuranceByIdOutputData Handle(GetInsuranceByIdInputData inputData)
        {
            if (inputData.PtId < 0)
            {
                return new GetInsuranceByIdOutputData(null, GetInsuranceByIdStatus.InvalidPtId);
            }
            if (inputData.HpId < 0)
            {
                return new GetInsuranceByIdOutputData(null, GetInsuranceByIdStatus.InvalidHpId);
            }
            if (inputData.SinDate < 0)
            {
                return new GetInsuranceByIdOutputData(null, GetInsuranceByIdStatus.InvalidSinDate);
            }
            if (inputData.HokenPid < 0)
            {
                return new GetInsuranceByIdOutputData(null, GetInsuranceByIdStatus.InvalidHokenPid);
            }

            var data = _insuranceResponsitory.GetInsuranceById(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenPid);
            return new GetInsuranceByIdOutputData(data, GetInsuranceByIdStatus.Successed);
        }
    }
}
