﻿namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoValidationResult
    {
        public string Message { get; private set; }

        public SavePatientInforValidationCode Code { get; private set; }

        public SavePatientInfoValidationResult(string message, SavePatientInforValidationCode code)
        {
            Message = message;
            Code = code;
        }
    }
}
