using Domain.Common;

namespace Domain.Models.DrugDetail
{
    public interface IDrugDetailRepository : IRepositoryBase
    {
        public IEnumerable<DrugMenuItemModel> GetDrugMenu(int hpId, int sinDate, string itemCd);

        DrugDetailModel GetDataDrugSeletedTree(int selectedIndexOfMenuLevel, int level, string drugName, string itemCd, string yjCode);

        List<TenMstByomeiModel> GetZaiganIsoItems(int hpId, int seikyuYm);
    }
}
