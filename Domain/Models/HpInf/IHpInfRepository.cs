namespace Domain.Models.HpInf
{
    public interface IHpInfRepository
    {
        bool CheckHpId(int hpId);

        HpInfModel GetHpInf(int hpId);
    }
}
