using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;

namespace Interactor.PatientInfor
{
    public class GetInsuranceMasterLinkageInteractor : IGetInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public GetInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetInsuranceMasterLinkageOutputData Handle(GetInsuranceMasterLinkageInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.InvalidHpId);

                if ((inputData.FutansyaNo.Length != 8))
                    return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.InvalidFutansyaNo);

                var listInsuranceMstLinkage = _patientInforRepository.GetDefHokenNoModels(inputData.HpId, inputData.FutansyaNo);

                if (!listInsuranceMstLinkage.Any())
                    return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.NoData);

                return new GetInsuranceMasterLinkageOutputData(listInsuranceMstLinkage, GetInsuranceMasterLinkageStatus.Success);
            }
            catch (Exception)
            {
                return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.Failed);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
