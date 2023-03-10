namespace Domain.Models.MstItem
{
    public class JihiSbtMstModel
    {
        public JihiSbtMstModel(int hpId, int jihiSbt, int sortNo, string name, int isDeleted)
        {
            HpId = hpId;
            JihiSbt = jihiSbt;
            SortNo = sortNo;
            Name = name;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int JihiSbt { get; private set; }
        public int SortNo { get; private set; }
        public string Name { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
