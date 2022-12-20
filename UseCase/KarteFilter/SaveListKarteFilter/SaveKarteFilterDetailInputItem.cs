namespace UseCase.KarteFilter.SaveListKarteFilter
{
    public class SaveKarteFilterDetailInputItem
    {
        public SaveKarteFilterDetailInputItem(bool bookMarkChecked, List<int> listHokenId, List<int> listKaId, List<int> listUserId)
        {
            BookMarkChecked = bookMarkChecked;
            ListHokenId = listHokenId;
            ListKaId = listKaId;
            ListUserId = listUserId;
        }

        public bool BookMarkChecked { get; private set; }

        public List<int> ListHokenId { get; private set; }

        public List<int> ListKaId { get; private set; }

        public List<int> ListUserId { get; private set; }
    }
}
