using UseCase.Reception.Delete;

namespace EmrCloudApi.Responses.Reception
{
    public class DeleteReceptionResponse
    {
        public DeleteReceptionResponse(List<DeleteReceptionItem> deleteReceptionItems)
        {
            DeleteReceptionItems = deleteReceptionItems;
        }

        public List<DeleteReceptionItem> DeleteReceptionItems { get; private set; }
    }
}
