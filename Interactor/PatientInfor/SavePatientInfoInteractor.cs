using Domain.Models.PatientInfor;
using Entity.Tenant;
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
                    msgValidation += string.IsNullOrEmpty(msgValidation) ? item : $",{item}";

                return new SavePatientInfoOutputData(msgValidation, SavePatientInfoStatus.Failed);
            }
            try
            {
                bool result = false;
                if (inputData.Patient.PtId == 0)
                    result = _patientInforRepository.CreatePatientInfo(inputData.Patient,inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.PtGrps);
                else
                    result = _patientInforRepository.UpdatePatientInfo(inputData.Patient, inputData.PtKyuseis, inputData.PtSanteis, inputData.Insurances, inputData.PtGrps);

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

        private IEnumerable<string> Validation(SavePatientInfoInputData model)
        {
            var resultMessages = new List<string>();

            #region Patient Info
            if (model.Patient.HpId <= 0)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HpId`"));

            if (string.IsNullOrEmpty(model.Patient.Name))
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.Name`"));

            if (model.Patient.Name.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Name`"));

            if (string.IsNullOrEmpty(model.Patient.KanaName))
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.KanaName`"));

            if (model.Patient.KanaName.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.KanaName`"));

            if (model.Patient.IsDead < 0 || model.Patient.IsDead > 1)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.IsDead`"));

            if (model.Patient.IsDead == 1 && model.Patient.DeathDate == 0)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), "`Patient.DeathDate`"));

            if (model.Patient.HomePost != null && model.Patient.HomePost.Length > 7)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomePost`"));

            if (model.Patient.HomeAddress1 != null && model.Patient.HomeAddress1.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress1`"));

            if (model.Patient.HomeAddress2 != null && model.Patient.HomeAddress2.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.HomeAddress2`"));

            if (model.Patient.Tel1 != null && model.Patient.Tel1.Length > 15)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel1`"));

            if (model.Patient.Tel2 != null && model.Patient.Tel2.Length > 15)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Tel2`"));

            if (model.Patient.Mail != null && model.Patient.Mail.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Mail`"));

            if (model.Patient.Setanusi != null && model.Patient.Setanusi.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Setanusi`"));

            if (model.Patient.Zokugara != null && model.Patient.Zokugara.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Zokugara`"));

            if (model.Patient.Job != null && model.Patient.Job.Length > 40)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.Job`"));

            if (model.Patient.RenrakuPost != null && model.Patient.RenrakuPost.Length > 7)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuPost`"));

            if (model.Patient.RenrakuAddress1 != null && model.Patient.RenrakuAddress1.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress1`"));

            if (model.Patient.RenrakuAddress2 != null && model.Patient.RenrakuAddress2.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuAddress2`"));

            if (model.Patient.RenrakuTel != null && model.Patient.RenrakuTel.Length > 15)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuTel`"));

            if (model.Patient.RenrakuMemo != null && model.Patient.RenrakuMemo.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.RenrakuMemo`"));

            if (model.Patient.OfficeName != null && model.Patient.OfficeName.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeName`"));

            if (model.Patient.OfficePost != null && model.Patient.OfficePost.Length > 7)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficePost`"));

            if (model.Patient.OfficeAddress1 != null && model.Patient.OfficeAddress1.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress1`"));

            if (model.Patient.OfficeAddress2 != null && model.Patient.OfficeAddress2.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeAddress2`"));

            if (model.Patient.OfficeTel != null && model.Patient.OfficeTel.Length > 15)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeTel`"));

            if (model.Patient.OfficeMemo != null && model.Patient.OfficeMemo.Length > 100)
                resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), "`Patient.OfficeMemo`"));
            #endregion

            #region PtKytsei
            for(int i = 0; i < model.PtKyuseis.Count; i++)
            {
                if (model.PtKyuseis[i].SeqNo < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].SeqNo`"));

                if(string.IsNullOrEmpty(model.PtKyuseis[i].KanaName))
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].KanaName`"));

                if (model.PtKyuseis[i].KanaName.Length > 100)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].KanaName`"));

                if (string.IsNullOrEmpty(model.PtKyuseis[i].Name))
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtKyuseis[{i}].Name`"));

                if (model.PtKyuseis[i].Name.Length > 100)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].Name`"));

                if (model.PtKyuseis[i].EndDate < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtKyuseis[{i}].EndDate`"));
            }
            #endregion

            #region PtSanteis
            for (int i = 0; i < model.PtSanteis.Count; i++)
            {
                if (model.PtSanteis[i].SeqNo < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].SeqNo`"));

                if (model.PtSanteis[i].EdaNo < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EdaNo`"));

                if (model.PtSanteis[i].KbnVal < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].KbnVal`"));

                if (model.PtSanteis[i].StartDate < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].StartDate`"));

                if (model.PtSanteis[i].EndDate < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtSanteis[{i}].EndDate`"));
            }
            #endregion

            #region PtGrps
            for (int i = 0; i < model.PtGrps.Count; i++)
            {
                if (model.PtGrps[i].GroupId < 0)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupId`"));

                if (string.IsNullOrEmpty(model.PtGrps[i].GroupCode))
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsRequired.GetDescription(), $"`PtGrps[{i}].GroupCode`"));

                if (model.PtGrps[i].GroupCode.Length > 4)
                    resultMessages.Add(string.Format(SavePatientInfoValidation.PropertyIsInvalid.GetDescription(), $"`PtGrps[{i}].GroupCode`"));
            }
            #endregion
            return resultMessages;
        }
    }
}