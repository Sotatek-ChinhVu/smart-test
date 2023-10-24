using Domain.Models.Diseases;

namespace UseCase.MstItem.GetTreeByomeiSet
{
    public class ByomeiSetMstItem
    {
        public ByomeiSetMstItem(ByomeiSetMstModel model)
        {
            GenerationId = model.GenerationId;
            SeqNo = model.SeqNo;
            Level1 = model.Level1;
            Level2 = model.Level2;
            Level3 = model.Level3;
            Level4 = model.Level4;
            Level5 = model.Level5;
            ByomeiCd = model.ByomeiCd;
            SetName = model.SetName;
            IsTitle = model.IsTitle;
            SelectType = model.SelectType;
        }
        public ByomeiSetMstItem(string setName)
        {
            SetName = setName;
        }
        public int GenerationId { get; private set; }

        public int SeqNo { get; private set; }

        public int Level1 { get; private set; }

        public int Level2 { get; private set; }

        public int Level3 { get; private set; }

        public int Level4 { get; private set; }

        public int Level5 { get; private set; }

        public string ByomeiCd { get; private set; }

        public string SetName { get; private set; }

        public int IsTitle { get; private set; }

        public int SelectType { get; private set; }
        public int Level
        {
            get
            {
                if (Level1 == 0) return 0;
                if (Level1 > 0 && Level2 == 0) return 1;
                if (Level2 > 0 && Level3 == 0) return 2;
                if (Level3 > 0 && Level4 == 0) return 3;
                if (Level4 > 0 && Level5 == 0) return 4;
                return 5;
            }
        }
        public List<ByomeiSetMstItem> Childrens { get; set; } = new List<ByomeiSetMstItem>();
    }
}
