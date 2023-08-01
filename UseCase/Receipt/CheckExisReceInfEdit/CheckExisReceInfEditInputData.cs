using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExisReceInfEdit
{
    public class CheckExisReceInfEditInputData : IInputData<CheckExisReceInfEditOutputData>
    {
        public CheckExisReceInfEditInputData(int hpId, int seikyuYm, long ptId, int sinYm, int hokenId)
        {
            HpId = hpId;
            SeikyuYm = seikyuYm;
            PtId = ptId;
            SinYm = sinYm;
            HokenId = hokenId;
        }

        public int HpId { get; private set; }

        public int SeikyuYm { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int HokenId { get; private set; }
    }
}
