using UseCase.MstItem.GetByomeiByCode;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetByomeiByCodeResponse
    {
        public GetByomeiByCodeResponse(ByomeiMstItem data)
        {
            Data = data;
        }
        public ByomeiMstItem Data { get; private set; }
    }
}
