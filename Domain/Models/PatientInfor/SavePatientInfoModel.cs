﻿using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.InsuranceInfor;

namespace Domain.Models.PatientInfor
{
    public class SavePatientInfoModel
    {
        public SavePatientInfoModel(PatientInforSaveModel patient, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<GroupInfModel> ptGrpInfs)
        {
            Patient = patient;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            Insurances = insurances;
            PtGrpInfs = ptGrpInfs;
        }

        public PatientInforSaveModel Patient { get; private set; }
        public List<PtKyuseiModel> PtKyuseis { get; private set; }
        public List<CalculationInfModel> PtSanteis { get; private set; }
        public List<InsuranceModel> Insurances { get; private set; }
        public List<GroupInfModel> PtGrpInfs { get; private set; }

    }
}