namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInf ord);

        OrdInf Read(int ordId);

        void Update(OrdInf ord);

        void Delete(int ordId);

        IEnumerable<OrdInf> GetAll();

        IEnumerable<OrdInf> GetList(long ptId, long raiinNo, int sinDate);

        int MaxUserId();
    }
}
