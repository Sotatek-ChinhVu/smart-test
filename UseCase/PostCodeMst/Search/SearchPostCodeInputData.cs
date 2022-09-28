using UseCase.Core.Sync.Core;

namespace UseCase.PostCodeMst.Search
{
    public class SearchPostCodeInputData : IInputData<SearchPostCodeOutputData>
    {
        public SearchPostCodeInputData(string postCode1, string postCode2, string address)
        {
            this.PostCode1 = postCode1;
            this.PostCode2 = postCode2;
            this.Address = address;
        }

        public string PostCode1 { get; private set; }
        public string PostCode2 { get; private set; }
        public string Address { get; private set; }
    }
}
