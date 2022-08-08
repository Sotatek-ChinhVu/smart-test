namespace Domain.Models.UketukeSbtDayInf;

public interface IUketukeSbtDayInfRepository
{
    List<UketukeSbtDayInfModel> GetListBySinDate(int sinDate);
    void Upsert(int sinDate, int uketukeSbt, int seqNo);
}
