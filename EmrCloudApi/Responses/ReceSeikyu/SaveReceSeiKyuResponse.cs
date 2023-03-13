using UseCase.ReceSeikyu.Save;

namespace EmrCloudApi.Responses.ReceSeikyu
{
    public class SaveReceSeiKyuResponse
    {
        public SaveReceSeiKyuResponse(SaveReceSeiKyuStatus state)
        {
            State = state;
        }

        public SaveReceSeiKyuStatus State { get; private set; }
    }
}
