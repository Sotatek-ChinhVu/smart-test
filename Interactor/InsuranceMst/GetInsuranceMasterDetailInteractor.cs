using Domain.Models.InsuranceMst;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.GetMasterDetails;

namespace Interactor.InsuranceMst
{
    internal class GetInsuranceMasterDetailInteractor : IGetInsuranceMasterDetailInputPort
    {
        private readonly IInsuranceMstRepository _insuranceMstReponsitory;

        public GetInsuranceMasterDetailInteractor(IInsuranceMstRepository insuranceMstReponsitory)
        {
            _insuranceMstReponsitory = insuranceMstReponsitory;
        }

        public GetInsuranceMstOutputData Handle(GetInsuranceMstInputData inputData)
        {
            

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

        public GetInsuranceMasterDetailOutputData Handle(GetInsuranceMasterDetailInputData inputData)
        {
            var result = new List<InsuranceMasterDetailModel>();
            if (inputData.HpId < 0)
                return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.InvalidHpId);



            return new GetInsuranceMasterDetailOutputData(result, GetInsuranceMasterDetailStatus.Successful);
        }
    }
}