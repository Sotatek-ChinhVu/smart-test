namespace Domain.Models.OrdInfDetails
{
    public interface IOrdInfDetailRepository
    {
        void Create(OrdInfDetailMst ordInfDetail);

        OrdInfDetailMst Read(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        void Update(OrdInfDetailMst ordInfDetail);

        void Delete(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        IEnumerable<OrdInfDetailMst> GetAll();
        IEnumerable<OrdInfDetailMst> GetList(long ptId, long raiinNo, int sinDate);

        int MaxUserId();
    }
}
