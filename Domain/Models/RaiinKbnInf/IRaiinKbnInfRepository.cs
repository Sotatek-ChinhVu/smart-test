namespace Domain.Models.RaiinKbnInf;

public interface IRaiinKbnInfRepository
{
    void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd);
}
