namespace Domain.Models.SpecialNote.PatientInfo
{
    public interface IPatientInfoRepository
    {
        List<PhysicalInfoModel> GetPhysicalList(int hpId, long ptId);

        List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId);

        List<SeikaturekiInfModel> GetSeikaturekiInfList(long ptId, int hpId);
    }
}
