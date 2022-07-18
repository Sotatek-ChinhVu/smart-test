namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfMst ord);

        OrdInfMst Read(int ordId);

        void Update(OrdInfMst ord);

        void Delete(int ordId);

        IEnumerable<OrdInfMst> GetAll();
        IEnumerable<OrdInfMst> GetList(long ptId, long raiinNo, int sinDate);
        int MaxUserId();
    }
}
