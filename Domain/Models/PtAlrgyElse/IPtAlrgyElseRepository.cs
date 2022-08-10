namespace Domain.Models.PtAlrgyElse
{
    public interface IPtAlrgyElseRepository
    {
        List<PtAlrgyElseModel> GetList(long ptId);
    }
}
