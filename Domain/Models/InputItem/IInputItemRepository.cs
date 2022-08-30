namespace Domain.Models.InputItem
{
    public interface IInputItemRepository
    {
        public IEnumerable<InputItemModel> SearchDataInputItem(string keyword, int kouiKbn, int sinDate, int startIndex, int pageCount, int genericOrSameItem, string yjCd, int hpId, double pointFrom, double pointTo, bool isRosai, bool isMirai, bool isExpired);

        public bool UpdateAdoptedItemAndItemConfig(int valueAdopted, string itemCdInputItem, int startDateInputItem);

        public InputItemModel? GetTenMst(int hpId, int sinDate, string itemCd);
    }
}
