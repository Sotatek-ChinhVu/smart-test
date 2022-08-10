namespace Domain.Models.PtAlrgyFood
{
    public interface IPtAlrgyFoodRepository
    {
        List<PtAlrgyFoodModel> GetList(long ptId);
    }
}
