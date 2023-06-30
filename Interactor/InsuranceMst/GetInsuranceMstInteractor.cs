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
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidHpId, 0);
            }

            if (inputData.PtId < 0)
            {
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidPtId, 0);
            }
            
            if (inputData.SinDate < 0)
            {
                return new GetInsuranceMstOutputData(new InsuranceMstModel(), GetInsuranceMstStatus.InvalidSinDate, 0);
            }

            try
            {
                var data = _insuranceMstReponsitory.GetDataInsuranceMst(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new GetInsuranceMstOutputData(data.insurance, GetInsuranceMstStatus.Successed, data.prefNo);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
