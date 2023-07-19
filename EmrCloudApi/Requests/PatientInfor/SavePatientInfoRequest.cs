﻿using Domain.Models.CalculationInf;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using EmrCloudApi.Requests.GroupInf;
using EmrCloudApi.Requests.Insurance;

namespace EmrCloudApi.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        public SavePatientInfoRequest(PatientInforDto patient, List<HokenPartternDto> insurances, List<HokenInfDto> hokenInfs, List<HokenKohiDto> hokenKohis, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<GroupInfDto> ptGrps, List<LimitListModel> maxMoneys, ReactSavePatientInfo reactSave, List<int> hokenIdList)
        {
            Patient = patient;
            Insurances = insurances;
            HokenInfs = hokenInfs;
            HokenKohis = hokenKohis;
            PtKyuseis = ptKyuseis;
            PtSanteis = ptSanteis;
            PtGrps = ptGrps;
            MaxMoneys = maxMoneys;
            ReactSave = reactSave;
            HokenIdList = hokenIdList;
        }

        public PatientInforDto Patient { get; private set; }

        public List<HokenPartternDto> Insurances { get; private set; }

        public List<HokenInfDto> HokenInfs { get; private set; }

        public List<HokenKohiDto> HokenKohis { get; private set; }

        public List<PtKyuseiModel> PtKyuseis { get; private set; }

        public List<CalculationInfModel> PtSanteis { get; private set; }

        public List<GroupInfDto> PtGrps { get; private set; }

        public List<LimitListModel> MaxMoneys { get; private set; }

        public ReactSavePatientInfo ReactSave { get; private set; }

        public List<int> HokenIdList { get; private set; }
    }
}