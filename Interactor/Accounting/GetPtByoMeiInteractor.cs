using Domain.Models.Accounting;
using Domain.Models.Diseases;
using UseCase.Accounting.GetPtByoMei;

namespace Interactor.Accounting
{
    public class GetPtByoMeiInteractor : IGetPtByoMeiInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetPtByoMeiInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetPtByoMeiOutputData Handle(GetPtByoMeiInputData inputData)
        {
            try
            {
                //Get GetPtByoMeiList
                var ptByoMei = _accountingRepository.GetPtByoMeiList(inputData.HpId, inputData.PtId, inputData.SinDate);

                if (!ptByoMei.Any())
                {
                    return new GetPtByoMeiOutputData(new List<PtDiseaseModel>(), GetPtByoMeiStatus.Failed);
                }
                return new GetPtByoMeiOutputData(ptByoMei, GetPtByoMeiStatus.Successed);

            }
            catch (Exception)
            {

                return new GetPtByoMeiOutputData(new List<PtDiseaseModel>(), GetPtByoMeiStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
