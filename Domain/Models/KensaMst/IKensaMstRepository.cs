namespace Domain.Models.KensaMst
{
    public interface IKensaMstRepository
    {
        List<KensaMstModel> GetList(int hpId);
    }
}
