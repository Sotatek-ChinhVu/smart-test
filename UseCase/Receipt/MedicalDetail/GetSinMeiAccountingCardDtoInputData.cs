namespace UseCase.Receipt.MedicalDetail
{
    public class GetSinMeiAccountingCardDtoInputData
    {
        public GetSinMeiAccountingCardDtoInputData(int hpId, long ptId, int sinYm, int hokenId)
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinYm { get; set; }
        public int HokenId { get; set; }
    }
}
