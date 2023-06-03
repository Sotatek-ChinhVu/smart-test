using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.GetHokenMasterReadOnly;

namespace Interactor.InsuranceMst
{
    public class GetHokenMasterReadOnlyInteractor : IGetHokenMasterReadOnlyInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetHokenMasterReadOnlyInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetHokenMasterReadOnlyOutputData Handle(GetHokenMasterReadOnlyInputData inputData)
        {
            if (inputData.HpId < 0)
                return new GetHokenMasterReadOnlyOutputData(GetHokenMasterReadOnlyStatus.InvalidHpId, new HokenMstModel());

            try
            {
                var data = _insuranceMstReponsitory.GetHokenMasterReadOnly(inputData.HpId, inputData.HokenNo, inputData.HokenEdaNo, inputData.PrefNo, inputData.SinDate);
                return new GetHokenMasterReadOnlyOutputData(GetHokenMasterReadOnlyStatus.Successful, data);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
