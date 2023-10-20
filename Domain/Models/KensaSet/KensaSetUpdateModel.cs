namespace Domain.Models.KensaSet
{
    public class KensaSetUpdateModel
    {
        public KensaSetUpdateModel(int hpId, int setId, int sortNo, string setName, int isDeleted)
        {
            HpId = hpId;
            SetId = setId;
            SortNo = sortNo;
            SetName = setName;
            IsDeleted = isDeleted;
        }
        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public int SortNo { get; private set; }
        
        public string SetName { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
