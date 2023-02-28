namespace Domain.Models.Diseases
{
    public class ByomeiSetMstModel
    {
        public ByomeiSetMstModel(int generationId, int seqNo, int level1, int level2, int level3, int level4, int level5, string byomeiCd, string byomeiName, string icd101, string icd102, string icd1012013, string icd1022013, string setName, int isTitle, int selectType)
        {
            GenerationId = generationId;
            SeqNo = seqNo;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            Level5 = level5;
            ByomeiCd = byomeiCd;
            ByomeiName = byomeiName;
            Icd101 = icd101;
            Icd102 = icd102;
            Icd1012013 = icd1012013;
            Icd1022013 = icd1022013;
            SetName = setName;
            IsTitle = isTitle;
            SelectType = selectType;
        }

        public int GenerationId { get; private set; }

        public int SeqNo { get; private set; }

        public int Level1 { get; private set; }

        public int Level2 { get; private set; }

        public int Level3 { get; private set; }

        public int Level4 { get; private set; }

        public int Level5 { get; private set; }

        public string ByomeiCd { get; private set; }

        public string ByomeiName { get; private set; }

        public string Icd101 { get; private set; }

        public string Icd102 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public string SetName { get; private set; }

        public int IsTitle { get; private set; }

        public int SelectType { get; private set; }

        public ByomeiType ByomeiKbn
        {
            get
            {
                //byomei
                if (ByomeiCd == null || ByomeiCd.Length != 4)
                {
                    return ByomeiType.Byomei;
                }
                //suffix
                else if (ByomeiCd.StartsWith("8"))
                {
                    return ByomeiType.Suffix;
                }
                //prefix
                else if (!ByomeiCd.StartsWith("9"))
                {
                    return ByomeiType.Prefix;

                }
                return ByomeiType.Byomei;
            }
        }

        public int Level
        {
            get
            {
                if (Level1 > 0 && Level2 == 0) return 1;
                if (Level2 > 0 && Level3 == 0) return 2;
                if (Level3 > 0 && Level4 == 0) return 3;
                if (Level4 > 0 && Level5 == 0) return 4;
                return 5;
            }
        }

        public List<ByomeiSetMstModel> Childrens { get; set; } = new List<ByomeiSetMstModel>();
    }

    public enum ByomeiType
    {
        Byomei,
        Suffix,
        Prefix
    }
}
