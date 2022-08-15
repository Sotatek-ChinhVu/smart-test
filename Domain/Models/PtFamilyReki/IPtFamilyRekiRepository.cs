namespace Domain.Models.PtFamilyReki
{
    public interface IPtFamilyRekiRepository
    {
        List<PtFamilyRekiModel> GetList(int hpId);
    }
}
