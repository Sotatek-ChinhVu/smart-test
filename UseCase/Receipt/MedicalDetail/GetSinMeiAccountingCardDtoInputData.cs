﻿namespace UseCase.Receipt.MedicalDetail
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

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinYm { get; private set; }
        public int HokenId { get; private set; }
    }
}
