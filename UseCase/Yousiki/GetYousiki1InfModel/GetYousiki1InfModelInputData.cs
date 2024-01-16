using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfModel
{
    public class GetYousiki1InfModelInputData : IInputData<GetYousiki1InfModelOutputData>
    {
        public GetYousiki1InfModelInputData(int hpId, int sinYm, long ptNum, int dataTypes)
        {
            HpId = hpId;
            SinYm = sinYm;
            PtNum = ptNum;
            DataTypes = dataTypes;
        }

        public int HpId { get; private set; }

        public int SinYm { get; private set; }

        public long PtNum { get; private set; }

        public int DataTypes { get; private set; }
    }
}
