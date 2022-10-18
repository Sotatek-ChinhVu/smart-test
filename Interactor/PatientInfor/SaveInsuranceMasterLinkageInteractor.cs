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
                            return new SaveInsuranceMasterLinkageOutputData(ConvertStatus(validationStatus));
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

        private ValidationStatus ConvertStatus(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidHpId)
                return ValidationStatus.InvalidHpId;

            if (status == ValidationStatus.InvalidDigit1)
                return ValidationStatus.InvalidDigit1;

            if (status == ValidationStatus.InvalidDigit2)
                return ValidationStatus.InvalidDigit2;

            if (status == ValidationStatus.InvalidDigit3)
                return ValidationStatus.InvalidDigit3;

            if (status == ValidationStatus.InvalidDigit4)
                return ValidationStatus.InvalidDigit4;

            if (status == ValidationStatus.InvalidDigit5)
                return ValidationStatus.InvalidDigit5;

            if (status == ValidationStatus.InvalidDigit6)
                return ValidationStatus.InvalidDigit6;

            if (status == ValidationStatus.InvalidDigit7)
                return ValidationStatus.InvalidDigit7;

            if (status == ValidationStatus.InvalidDigit8)
                return ValidationStatus.InvalidDigit8;

            if (status == ValidationStatus.InvalidHokenNo)
                return ValidationStatus.InvalidHokenNo;
            return ValidationStatus.Success;
        }
    }
}
