﻿using Domain.Common;

namespace Domain.Models.SpecialNote.PatientInfo
{
    public interface IPatientInfoRepository : IRepositoryBase
    {
        List<PhysicalInfoModel> GetPhysicalList(int hpId, long ptId);

        List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId);

        List<SeikaturekiInfModel> GetSeikaturekiInfList(long ptId, int hpId);

        List<KensaInfDetailModel> GetListKensaInfModel(int hpId, long ptId, int sinDate);

        List<KensaInfDetailModel> GetListKensaInfDetailModel(int hpId, long ptId, int sinDate);

        List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId, int sinDate);

        bool SaveKensaInfWeightedConfirmation(int hpId, long ptId, long raiinNo, double weight, int sinDate, int userId);
    }
}
