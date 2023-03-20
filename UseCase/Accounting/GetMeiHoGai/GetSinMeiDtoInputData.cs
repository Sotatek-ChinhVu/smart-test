namespace UseCase.Accounting.GetMeiHoGai
{
    public class GetSinMeiDtoInputData
    {
        public GetSinMeiDtoInputData(List<long> raiinNoList, long ptId, int sinDate, int hpId, int sinMeiMode)
        {
            RaiinNoList = raiinNoList;
            PtId = ptId;
            SinDate = sinDate;
            HpId = hpId;
            SinMeiMode = sinMeiMode;
        }

        public GetSinMeiDtoInputData(long ptId, int hpId, int sinYm, int hokenId, int sinMeiMode)
        {
            PtId = ptId;
            HpId = hpId;
            SinYm = sinYm;
            HokenId = hokenId;
            SinMeiMode = sinMeiMode;
            RaiinNoList = new();
        }

        public GetSinMeiDtoInputData(int hpId, long ptId, int sinYm, int seikyuYm, int hokenId, int sinMeiMode)
        {
            PtId = ptId;
            HpId = hpId;
            SinYm = sinYm;
            SeikyuYm = seikyuYm;
            HokenId = hokenId;
            SinMeiMode = sinMeiMode;
            RaiinNoList = new();
        }

        public List<long> RaiinNoList { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int HpId { get; private set; }
        public int SinYm { get; private set; }
        public int SeikyuYm { get; set; }
        public int HokenId { get; private set; }
        public int SinMeiMode { get; private set; }
    }
}
