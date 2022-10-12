namespace Domain.Models.PtKyuseiInf
{
    public interface IPtKyuseiInfRepository
    {
        List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);
    }
}
