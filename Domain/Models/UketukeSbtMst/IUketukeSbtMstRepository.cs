namespace Domain.Models.UketukeSbtMst;

public interface IUketukeSbtMstRepository
{
    UketukeSbtMstModel? GetByKbnId(int kbnId);
    List<UketukeSbtMstModel> GetList();
}
