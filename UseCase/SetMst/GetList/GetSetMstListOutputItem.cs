﻿namespace UseCase.SetMst.GetList
{
    public class GetSetMstListOutputItem
    {
        public GetSetMstListOutputItem(int hpId, int setCd, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, string setName, int weightKbn, int color, int isGroup, List<GetSetMstListOutputItem>? childrens)
        {
            HpId = hpId;
            SetCd = setCd;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            GenerationId = generationId;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            SetName = setName;
            WeightKbn = weightKbn;
            Color = color;
            IsGroup = isGroup;
            Childrens = childrens;
        }

        public int HpId { get; private set; }
        public int SetCd { get; private set; }
        public int SetKbn { get; private set; }
        public int SetKbnEdaNo { get; private set; }
        public int GenerationId { get; private set; }
        public int Level1 { get; private set; }
        public int Level2 { get; private set; }
        public int Level3 { get; private set; }
        public string SetName { get; private set; }
        public int WeightKbn { get; private set; }
        public int Color { get; private set; }
        public int IsGroup { get; private set; }
        public List<GetSetMstListOutputItem>? Childrens { get; private set; }
    }
}
