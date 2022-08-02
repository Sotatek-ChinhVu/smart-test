using Domain.Models.IsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.InsuranceMst.Get;

namespace Interactor.InsuranceMst
{
    public class GetInsuranceMstInteractor : IGetInsuranceMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetInsuranceMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetInsuranceMstOutputData Handle(GetInsuranceMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetInsuranceMstOutputData(null, GetInsuranceMstStatus.InvalidHpId);
            }

            if (inputData.PtId < 0)
            {
                return new GetInsuranceMstOutputData(null, GetInsuranceMstStatus.InvalidPtId);
            }
            
            if (inputData.SinDate < 0)
            {
                return new GetInsuranceMstOutputData(null, GetInsuranceMstStatus.InvalidSinDate);
            }

            if (inputData.HokenId < 0)
            {
                return new GetInsuranceMstOutputData(null, GetInsuranceMstStatus.InvalidHokenId);
            }

            var data = _insuranceMstReponsitory.GetDataInsuranceMst(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId);
            return new GetInsuranceMstOutputData(data, GetInsuranceMstStatus.Successed);
        }
    }
}
