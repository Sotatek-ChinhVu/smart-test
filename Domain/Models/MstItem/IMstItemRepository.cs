namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);
        SearchSupplementModel GetListSupplement(string searchValue, int pageIndex, int pageSize);
    }
}
