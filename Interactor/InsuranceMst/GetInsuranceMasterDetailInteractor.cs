using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.GetMasterDetails;

namespace Interactor.InsuranceMst
{
    public class GetInsuranceMasterDetailInteractor : IGetInsuranceMasterDetailInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetInsuranceMasterDetailInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetInsuranceMasterDetailOutputData Handle(GetInsuranceMasterDetailInputData inputData)
        {
            var result = new List<InsuranceMasterDetailModel>();
            if (inputData.HpId < 0)
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.InvalidHpId);

            if(inputData.FHokenNo < 0)
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.InvalidFHokenNo);

            if (inputData.FHokenSbtKbn < 0)
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.InvalidFHokenSbtKbn);

            try
            {
                result = _insuranceMstReponsitory.GetInsuranceMasterDetails(inputData.HpId, inputData.FHokenNo, inputData.FHokenSbtKbn, inputData.IsJitan, inputData.IsTaken);
            }
            finally
            {
                _insuranceMstReponsitory.ReleaseResource();
            }

            if(result.Any())
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.Successful);
            else
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.DataNotFound);
        }
    }
}