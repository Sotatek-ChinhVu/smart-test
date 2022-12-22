using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.KohiHokenMst.Get;

namespace Interactor.KohiHokenMst
{
    public class GetKohiHokenMstInteractor : IGetKohiHokenMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstResponsitory;
        public GetKohiHokenMstInteractor(IInsuranceMstRepository insuranceMstResponsitory)
        {
            _insuranceMstResponsitory = insuranceMstResponsitory;
        }

        public GetKohiHokenMstOutputData Handle(GetKohiHokenMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetKohiHokenMstOutputData(new HokenMstModel(), GetKohiHokenMstStatus.InvalidHpId);
            }

            if (inputData.SinDate < 0)
            {
                return new GetKohiHokenMstOutputData(new HokenMstModel(), GetKohiHokenMstStatus.InvalidSinDate);
            }

            if (String.IsNullOrEmpty(inputData.FutansyaNo))
            {
                return new GetKohiHokenMstOutputData(new HokenMstModel(), GetKohiHokenMstStatus.InvalidFutansyaNo);
            }

            try
            {
                var data = _insuranceMstResponsitory.GetHokenMstByFutansyaNo(inputData.HpId, inputData.SinDate, inputData.FutansyaNo);
                return new GetKohiHokenMstOutputData(data, GetKohiHokenMstStatus.Successed);
            }
            finally
            {
                _insuranceMstResponsitory.ReleaseResource();
            }
        }
    }
}
