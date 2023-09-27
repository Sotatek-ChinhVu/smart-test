namespace Domain.Models.KensaSet
{
    public class KensaSetModel
    {
        public KensaSetModel(int hpId, int setId, string setName, int sortNo, int isDeleted, DateTime createDate, int createId, string? createMachine, DateTime updateDate, int updateId, string? updateMachine)
        {
            HpId = hpId;
            SetId = setId;
            SetName = setName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public string SetName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int CreateId { get; private set; }

        public string? CreateMachine { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public int UpdateId { get; private set; }

        public string? UpdateMachine { get; private set; }
    }
}
