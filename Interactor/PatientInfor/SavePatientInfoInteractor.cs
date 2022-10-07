using Domain.Models.PatientInfor;
using Helper;
using UseCase.PatientInfor.Save;

namespace Interactor.PatientInfor
{
    public class SavePatientInfoInteractor : ISavePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public SavePatientInfoInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public SavePatientInfoOutputData Handle(SavePatientInfoInputData inputData)
        {
            var validations = Validation(inputData);
            if (validations.Any())
            {
                string msgValidation = string.Empty;
                foreach (var item in validations)
                    msgValidation += string.IsNullOrEmpty(msgValidation) ? item.GetDescription() : $",{item.GetDescription()}";

                return new SavePatientInfoOutputData(msgValidation, SavePatientInfoStatus.Failed);
            }
            try
            {
                bool result = _patientInforRepository.SavePatientInfor(inputData.Patient,
                    inputData.PtSanteis,inputData.HokenPartterns,inputData.PtGrps);

                if (result)
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Successful);
                else
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Failed);
            }
            catch (Exception ex)
            {
                var a = ex;
                return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Failed);
            }
        }

        private IEnumerable<SavePatientInfoValidation> Validation(SavePatientInfoInputData model)
        {
            var result = new List<SavePatientInfoValidation>();

            if (model.Patient.HpId <= 0)
                result.Add(SavePatientInfoValidation.InvalidHpId);

            if (string.IsNullOrEmpty(model.Patient.Name) || model.Patient.Name.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidName);

            if (string.IsNullOrEmpty(model.Patient.KanaName) || model.Patient.KanaName.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidKanaName);

            return result;
        }
    }
}
