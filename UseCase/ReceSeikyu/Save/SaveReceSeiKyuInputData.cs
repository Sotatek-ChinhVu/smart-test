using Domain.Models.ReceSeikyu;
using Helper.Messaging;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuInputData : IInputData<SaveReceSeiKyuOutputData>
    {
        public SaveReceSeiKyuInputData(List<ReceSeikyuModel> receSeiKyus, int sinYm, int hpId, int userId, IMessenger messenger)
        {
            ReceSeiKyus = receSeiKyus;
            SinYm = sinYm;
            HpId = hpId;
            UserId = userId;
            Messenger = messenger;
        }

        public List<ReceSeikyuModel> ReceSeiKyus { get; private set; }

        public int SinYm { get; private set; }

        public int HpId { get; set; }

        public int UserId { get; set; }

        public IMessenger Messenger { get; set; }
    }
}
