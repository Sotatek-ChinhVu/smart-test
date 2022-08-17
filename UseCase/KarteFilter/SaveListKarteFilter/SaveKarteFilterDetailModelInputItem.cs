namespace UseCase.KarteFilter.SaveListKarteFilter
{
    public class SaveKarteFilterDetailModelInputItem
    {
        public SaveKarteFilterDetailModelInputItem(int hpId, int userId, long filterId, bool bookMarkChecked, List<int> listHokenId, List<int> listKaId, List<int> listUserId)
        {
            HpId = hpId;
            UserId = userId;
            FilterId = filterId;
            BookMarkChecked = bookMarkChecked;
            ListHokenId = listHokenId;
            ListKaId = listKaId;
            ListUserId = listUserId;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public long FilterId { get; private set; }

        public bool BookMarkChecked { get; private set; }

        public List<int> ListHokenId { get; private set; }

        public List<int> ListKaId { get; private set; }

        public List<int> ListUserId { get; private set; }
    }
}
