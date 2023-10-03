namespace UseCase.MstItem.GetByomeiByCode
{
    public class ByomeiMstItem
    {
        public ByomeiMstItem(string name)
        {
            Name = name;
        }

        public ByomeiMstItem()
        {
            Name = string.Empty;
        }

        public string Name { get; private set; }
    }
}
