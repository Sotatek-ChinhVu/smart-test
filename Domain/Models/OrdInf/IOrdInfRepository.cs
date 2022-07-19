namespace Domain.Models.OrdInfs
{
    public interface IOrdInfRepository
    {
        void Create(OrdInfModel ord);

        OrdInfModel Read(int ordId);

        void Update(OrdInfModel ord);

        void Delete(int ordId);

        IEnumerable<OrdInfModel> GetAll();

        IEnumerable<OrdInfModel> GetList(long ptId, long raiinNo, int sinDate);

        int MaxUserId();
    }
}
