using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using static Helper.Constants.DefHokenNoConst;

namespace Interactor.PatientInfor
{
    public class SaveInsuranceMasterLinkageInteractor : ISaveInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public SaveInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public SaveInsuranceMasterLinkageOutputData Handle(SaveInsuranceMasterLinkageInputData inputData)
        {
            try
            {
                if (inputData.DefHokenNoModels.Any())
                {
                    foreach (var item in inputData.DefHokenNoModels)
                    {
                        var validationStatus = item.Validation();
                        if (validationStatus != ValidationStatus.Valid)
                            return new SaveInsuranceMasterLinkageOutputData(validationStatus);
                    }
                }
                else
                    return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.InputDataNull);
                bool success = _patientInforRepository.SaveInsuranceMasterLinkage(inputData.DefHokenNoModels);
                var status = success ? ValidationStatus.Success : ValidationStatus.Failed;
                return new SaveInsuranceMasterLinkageOutputData(status);
            }
            catch (Exception)
            {
                return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.Failed);
            }
        }
    }
}
