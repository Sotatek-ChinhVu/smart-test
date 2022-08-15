namespace Domain.Models.RsvFrameMst
{
    public interface IRsvFrameMstRepository
    {
        List<RsvFrameMstModel> GetList(int hpId);
    }
}
