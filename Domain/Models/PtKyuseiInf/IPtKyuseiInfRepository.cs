namespace Domain.Models.PtKyuseiInf
{
    public interface IPtKyuseiInfRepository
    {
        public List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);
    }
}
