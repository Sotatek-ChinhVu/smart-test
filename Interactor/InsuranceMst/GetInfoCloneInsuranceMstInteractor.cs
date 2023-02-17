using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.GetInfoCloneInsuranceMst;

namespace Interactor.InsuranceMst
{
    public class GetInfoCloneInsuranceMstInteractor : IGetInfoCloneInsuranceMstInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetInfoCloneInsuranceMstInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetInfoCloneInsuranceMstOutputData Handle(GetInfoCloneInsuranceMstInputData inputData)
        {
            if (inputData.HpId < 0)
                return new GetInfoCloneInsuranceMstOutputData(0, 0 ,GetInfoCloneInsuranceMstStatus.InvalidHpId);

            if (inputData.HokenNo < 0)
                return new GetInfoCloneInsuranceMstOutputData(0, 0, GetInfoCloneInsuranceMstStatus.InvalidHokenNo);

            if (inputData.PrefNo < 0)
                return new GetInfoCloneInsuranceMstOutputData(0, 0, GetInfoCloneInsuranceMstStatus.InvalidPrefNo);

            try
            {
                var result = _insuranceMstReponsitory.GetInfoCloneInsuranceMst(inputData.HpId, inputData.HokenNo, inputData.PrefNo, inputData.StartDate);

                return new GetInfoCloneInsuranceMstOutputData(result.Item2 , result.Item1, GetInfoCloneInsuranceMstStatus.Successful);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }
        }
    }
}
