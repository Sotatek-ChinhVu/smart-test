using Domain.Models.InsuranceMst;
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
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidHpId);
            }

            if (inputData.PtId < 0)
            {
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidPtId);
            }
            
            if (inputData.SinDate < 0)
            {
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidSinDate);
            }

            try
            {
                var data = _insuranceMstReponsitory.GetDataInsuranceMst(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new GetInsuranceMstOutputData(data, GetInsuranceMstStatus.Successed);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
