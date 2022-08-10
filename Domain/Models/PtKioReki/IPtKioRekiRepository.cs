namespace Domain.Models.PtKioReki
{
    public interface IPtKioRekiRepository
    {
        List<PtKioRekiModel> GetList(long ptId);
    }
}
