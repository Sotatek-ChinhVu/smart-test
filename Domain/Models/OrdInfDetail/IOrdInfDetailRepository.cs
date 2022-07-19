namespace Domain.Models.OrdInfDetails
{
    public interface IOrdInfDetailRepository
    {
        void Create(OrdInfDetailModel ordInfDetail);

        OrdInfDetailModel Read(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        void Update(OrdInfDetailModel ordInfDetail);

        void Delete(int ordId, long raiinNo, long rpNo, long rpEdaNo, int rowNo);

        IEnumerable<OrdInfDetailModel> GetAll();

        IEnumerable<OrdInfDetailModel> GetList(long ptId, long raiinNo, int sinDate);

        int MaxUserId();
    }
}
