using UseCase.Core.Sync.Core;

namespace UseCase.Set.GetList
{
    public class GetSetListInputData : IInputData<GetSetListOutputData>
    {
        public GetSetListInputData(int hpId, int setKbn, int setKbnEdaNo, string textSearch, int sinDate)
        {
            HpId = hpId;
            SetKbn = setKbn;
            SetKbnEdaNo = setKbnEdaNo;
            TextSearch = textSearch;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public int SetKbn { get; private set; }
        public int SetKbnEdaNo { get; private set; }
        public int SinDate { get; private set; }
        public string TextSearch { get; private set; }
    }
}