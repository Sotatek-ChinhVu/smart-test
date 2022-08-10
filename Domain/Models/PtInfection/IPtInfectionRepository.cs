namespace Domain.Models.PtInfection
{
    public interface IPtInfectionRepository
    {
        List<PtInfectionModel> GetList(long ptId);
    }
}
