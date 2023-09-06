using Domain.Models.ReceSeikyu;
using Helper.Messaging;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuInputData : IInputData<SaveReceSeiKyuOutputData>
    {
        public SaveReceSeiKyuInputData(List<ReceSeikyuModel> receSeiKyus, int sinYm, int hpId, int userAct, IMessenger messenger)
        {
            ReceSeiKyus = receSeiKyus;
            SinYm = sinYm;
            HpId = hpId;
            UserAct = userAct;
            Messenger = messenger;
        }

        public List<ReceSeikyuModel> ReceSeiKyus { get; private set; }

        public int SinYm { get; private set; }

        public int HpId { get; set; }

        public int UserAct { get; set; }

        public IMessenger Messenger { get; set; }
    }
}
