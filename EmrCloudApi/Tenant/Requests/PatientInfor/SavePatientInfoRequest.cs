﻿
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using EmrCloudApi.Tenant.Requests.Insurance;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public SavePatientInfoRequest(PatientInforDto patient, List<HokenPartternDto> insurances, List<HokenInfDto> hokenInfs, List<HokenKohiDto> hokenKohis, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<GroupInfModel> ptGrps)
        {
            Patient = patient;
            Insurances = insurances;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            PtGrps = ptGrps;
        }

        public PatientInforDto Patient { get; private set; }

        public List<HokenPartternDto> Insurances { get; private set; }

        public List<HokenInfDto> HokenInfs { get; private set; }

        public List<HokenKohiDto> HokenKohis { get; private set; }

        public List<PtKyuseiModel> PtKyuseis { get; private set; }

        public List<CalculationInfModel> PtSanteis { get; private set; }

        public List<GroupInfModel> PtGrps { get; private set; }
    }
}