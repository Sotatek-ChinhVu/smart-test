namespace Domain.Models.SetKbnMst
{
    public class SetKbnMstModel
    {
        public SetKbnMstModel(int hpId, int setKbn, int setKbnEdaNo, string setKbnName, int kaCd, int docCd, int isDeleted, int generationId)
        {
            HpId = hpId;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            SetKbnName = setKbnName;
            KaCd = kaCd;
            DocCd = docCd;
            IsDeleted = isDeleted;
            GenerationId = generationId;
        }

        public int HpId { get; private set; }
        public int SetKbn { get; private set; }
        public int SetKbnEdaNo { get; private set; }
        public string SetKbnName { get; private set; }
        public int KaCd { get; private set; }
        public int DocCd { get; private set; }
        public int IsDeleted { get; private set; }
        public int GenerationId { get; private set; }
    }
}
