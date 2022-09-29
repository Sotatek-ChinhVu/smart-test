using UseCase.Core.Sync.Core;

namespace UseCase.PostCodeMst.Search
{
    public class SearchPostCodeInputData : IInputData<SearchPostCodeOutputData>
    {
        public SearchPostCodeInputData(int hpId, string postCode1, string postCode2, string address)
        {
            HpId = hpId;
            PostCode1 = postCode1;
            PostCode2 = postCode2;
            Address = address;
        }

        public int HpId { get; private set; }
        public string PostCode1 { get; private set; }
        public string PostCode2 { get; private set; }
        public string Address { get; private set; }
    }
}
