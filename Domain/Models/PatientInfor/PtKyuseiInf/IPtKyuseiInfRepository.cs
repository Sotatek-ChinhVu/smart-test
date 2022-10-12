namespace Domain.Models.PatientInfor.PtKyuseiInf
{
    public interface IPtKyuseiInfRepository
    {
        List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);
    }
}
