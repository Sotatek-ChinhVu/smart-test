namespace Interactor.Recalculation
{
    public interface IRecalculation
    {
        void CheckErrorInMonth(int hpId, int seikyuYm, List<long> ptIds);
    }
}
