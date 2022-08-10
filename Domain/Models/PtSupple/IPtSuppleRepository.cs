namespace Domain.Models.PtSupple
{
    public interface IPtSuppleRepository
    {
        List<PtSuppleModel> GetList(long ptId);
    }
}
