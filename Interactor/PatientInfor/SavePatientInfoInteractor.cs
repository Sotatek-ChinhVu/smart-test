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
                bool result = false;
                if (inputData.Patient.PtId == 0)
                    result = _patientInforRepository.CreatePatientInfo(inputData.Patient, inputData.PtSanteis, inputData.HokenPartterns, inputData.PtGrps);
                else
                    result = _patientInforRepository.UpdatePatientInfo(inputData.Patient, inputData.PtSanteis, inputData.HokenPartterns, inputData.PtGrps);

                if (result)
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Successful);
                else
                    return new SavePatientInfoOutputData(string.Empty, SavePatientInfoStatus.Failed);
            }
            catch
            {
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

            if (model.Patient.IsDead == 1 && model.Patient.DeathDate == 0)
                result.Add(SavePatientInfoValidation.InvalidRequiredDeathDate);

            if (model.Patient.HomePost != null && model.Patient.HomePost.Length > 7)
                result.Add(SavePatientInfoValidation.InvalidHomePost);

            if (model.Patient.HomeAddress1 != null && model.Patient.HomeAddress1.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidHomeAddress1);

            if (model.Patient.HomeAddress2 != null && model.Patient.HomeAddress2.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidHomeAddress2);

            if (model.Patient.Tel1 != null && model.Patient.Tel1.Length > 15)
                result.Add(SavePatientInfoValidation.InvalidTel1);

            if (model.Patient.Tel2 != null && model.Patient.Tel2.Length > 15)
                result.Add(SavePatientInfoValidation.InvalidTel2);

            if (model.Patient.Mail != null && model.Patient.Mail.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidEmail);

            if (model.Patient.Setanusi != null && model.Patient.Setanusi.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidSetanusi);

            if (model.Patient.Zokugara != null && model.Patient.Zokugara.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidZokugara);

            if (model.Patient.Job != null && model.Patient.Job.Length > 40)
                result.Add(SavePatientInfoValidation.InvalidJob);

            if (model.Patient.RenrakuPost != null && model.Patient.RenrakuPost.Length > 7)
                result.Add(SavePatientInfoValidation.InvalidRenrakuPost);

            if (model.Patient.RenrakuAddress1 != null && model.Patient.RenrakuAddress1.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidRenrakuAddress1);

            if (model.Patient.RenrakuAddress2 != null && model.Patient.RenrakuAddress2.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidRenrakuAddress2);

            if (model.Patient.RenrakuTel != null && model.Patient.RenrakuTel.Length > 15)
                result.Add(SavePatientInfoValidation.InvalidRenrakuTel);

            if (model.Patient.RenrakuMemo != null && model.Patient.RenrakuMemo.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidRenrakuMemo);

            if (model.Patient.OfficeName != null && model.Patient.OfficeName.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidOfficeName);

            if (model.Patient.OfficePost != null && model.Patient.OfficePost.Length > 7)
                result.Add(SavePatientInfoValidation.InvalidOfficePost);

            if (model.Patient.OfficeAddress1 != null && model.Patient.OfficeAddress1.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidOfficeAddress1);

            if (model.Patient.OfficeAddress2 != null && model.Patient.OfficeAddress2.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidOfficeAddress2);

            if (model.Patient.OfficeTel != null && model.Patient.OfficeTel.Length > 15)
                result.Add(SavePatientInfoValidation.InvalidOfficeOfficeTel);

            if (model.Patient.OfficeMemo != null && model.Patient.OfficeMemo.Length > 100)
                result.Add(SavePatientInfoValidation.InvalidOfficeOfficeMemo);

            return result;
        }
    }
}