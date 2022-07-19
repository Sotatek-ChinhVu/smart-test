namespace Domain.Models.OrdInfDetails
{
    public interface IOrdInfDetailRepository
    {
        void Create(OrdInfDetail ordInfDetail);

        OrdInfDetail Read(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        void Update(OrdInfDetail ordInfDetail);

        void Delete(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        IEnumerable<OrdInfDetail> GetAll();

        IEnumerable<OrdInfDetail> GetList(long ptId, long raiinNo, int sinDate);

        int MaxUserId();
    }
}
