using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfModel
{
    public class GetYousiki1InfModelInputData : IInputData<GetYousiki1InfModelOutputData>
    {
        public GetYousiki1InfModelInputData(int hpId, int sinYm, long ptNum, int dataType)
        {
            HpId = hpId;
            SinYm = sinYm;
            PtNum = ptNum;
            DataType = dataType;
        }

        public int HpId { get; private set; }

        public int SinYm { get; private set; }

        public long PtNum { get; private set; }

        public int DataType { get; private set; }
    }
}
