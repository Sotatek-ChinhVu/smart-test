using Domain.Common;

namespace Domain.Models.UketukeSbtDayInf;

public interface IUketukeSbtDayInfRepository : IRepositoryBase
{
    List<UketukeSbtDayInfModel> GetListBySinDate(int sinDate);
    void Upsert(int sinDate, int uketukeSbt, int seqNo, int userId);
}
