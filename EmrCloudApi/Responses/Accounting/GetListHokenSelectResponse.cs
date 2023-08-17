using UseCase.Accounting.GetListHokenSelect;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetListHokenSelectResponse
    {
        public GetListHokenSelectResponse(List<ListHokenSelectDto> hokenSelects)
        {
            HokenSelects = hokenSelects;
        }

        public List<ListHokenSelectDto> HokenSelects { get; private set; }
    }
}
