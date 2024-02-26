using Domain.Common;

namespace Domain.Models.UketukeSbtDayInf;

public interface IUketukeSbtDayInfRepository : IRepositoryBase
{
    List<UketukeSbtDayInfModel> GetListBySinDate(int hpId, int sinDate);
    void Upsert(int hpId, int sinDate, int uketukeSbt, int seqNo, int userId);
}
