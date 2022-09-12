namespace Domain.Models.MstItem
{
    public interface IMstItemRepository
    {
        List<DosageDrugModel> GetDosages(List<string> yjCds);
        List<SearchOTCModel> SearchOTCModels(string searchValue, int pageIndex, int pageSize);
    }
}
