using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.GetYousiki1InfDetailsByCodeNo
{
    public class GetYousiki1InfDetailsByCodeNoInputData : IInputData<GetYousiki1InfDetailsByCodeNoOutputData>
    {
        public GetYousiki1InfDetailsByCodeNoInputData(int hpId, int sinYm, long ptId, int dataType, int seqNo, string codeNo)
        {
            HpId = hpId;
            SinYm = sinYm;
            PtId = ptId;
            DataType = dataType;
            SeqNo = seqNo;
            CodeNo = codeNo;
        }

        public int HpId { get; private set; }

        public int SinYm { get; private set; }

        public long PtId { get; private set; }

        public int DataType { get; private set; }

        public int SeqNo { get; private set; }

        public string CodeNo { get; private set; }
    }
}
