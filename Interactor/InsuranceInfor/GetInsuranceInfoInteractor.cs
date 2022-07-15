using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InsuranceInfor.Get;
using UseCase.InsuranceList.GetInsuranceListById;

namespace Interactor.InsuranceList
{
    public class GetInsuranceInfoInteractor : IGetInsuranceInforInputPort
    {
        private readonly IInsuranceInforResponsitory _insuranceInforResponsitory;
        public GetInsuranceInfoInteractor(IInsuranceInforResponsitory insuranceInforResponsitory)
        {
            _insuranceInforResponsitory = insuranceInforResponsitory;
        }

        public GetInsuranceInforOutputData Handle(GetInsuranceInforInputData inputData)
        {
            if(inputData.PtId.Value < 0)
            {
                return new GetInsuranceInforOutputData(null, GetInsuranceInforStatus.PtIdInvalid);
            }
            
            if(inputData.HokenId < 0)
            {
                return new GetInsuranceInforOutputData(null, GetInsuranceInforStatus.HokenIdInvalid);
            }

            var data = _insuranceInforResponsitory.GetInsuranceInfor(inputData.PtId, inputData.HokenId);
            if(data == null)
                return new GetInsuranceInforOutputData(null, GetInsuranceInforStatus.DataNotExist);

            return new GetInsuranceInforOutputData(data, GetInsuranceInforStatus.Successed);
        }
    }
}
