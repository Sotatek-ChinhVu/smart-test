namespace Interactor.Recalculation
{
    public interface IRecalculation
    {
        void CheckErrorInMonth(int seikyuYm, List<long> ptIds);
    }
}
