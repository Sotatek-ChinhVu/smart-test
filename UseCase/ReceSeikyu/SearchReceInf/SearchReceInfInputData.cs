using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.SearchReceInf
{
    public class SearchReceInfInputData : IInputData<SearchReceInfOutputData>
    {
        public SearchReceInfInputData(int hpId, long ptNum, int sinYm)
        {
            HpId = hpId;
            PtNum = ptNum;
            SinYm = sinYm;
        }

        public int HpId { get; private set; }

        public long PtNum { get; private set; }

        public int SinYm { get; private set; }
    }
}
