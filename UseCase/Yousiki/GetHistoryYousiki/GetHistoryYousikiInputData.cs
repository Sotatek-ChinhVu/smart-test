using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetHistoryYousiki
{
    public class GetHistoryYousikiInputData : IInputData<GetHistoryYousikiOutputData>
    {
        public GetHistoryYousikiInputData(int hpId, int sinYm, long ptId, int dataType)
        {
            HpId = hpId;
            SinYm = sinYm;
            PtId = ptId;
            DataType = dataType;
        }

        public int HpId { get; private set; }

        public int SinYm { get; private set; }

        public long PtId { get; private set; }

        public int DataType { get; private set; }
    }
}
