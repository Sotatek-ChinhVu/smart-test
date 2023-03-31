using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.SaveOrdInsuranceMst;

namespace Interactor.InsuranceMst
{
    public class SaveOrdInsuranceMstInteractor : ISaveOrdInsuranceMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public SaveOrdInsuranceMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public SaveOrdInsuranceMstOutputData Handle(SaveOrdInsuranceMstInputData inputData)
        {
            if (inputData.HpId < 0)
                return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.InvalidHpId);

            if (inputData.UserId < 0)
                return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.InvalidUserId);
            try
            {
                bool result = _insuranceMstReponsitory.SaveOrdInsuranceMst(inputData.Insurances ,inputData.HpId, inputData.UserId);
                if (result)
                    return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.Successful);
                else
                    return new SaveOrdInsuranceMstOutputData(SaveOrdInsuranceMstStatus.Failed);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
