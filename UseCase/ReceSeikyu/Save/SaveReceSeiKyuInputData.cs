using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuInputData : IInputData<SaveReceSeiKyuOutputData>
    {
        public SaveReceSeiKyuInputData(List<ReceSeikyuModel> receSeiKyus, int sinYm, int hpId, int userAct)
        {
            ReceSeiKyus = receSeiKyus;
            SinYm = sinYm;
            HpId = hpId;
            UserAct = userAct;
        }

        public List<ReceSeikyuModel> ReceSeiKyus { get; private set; }

        public int SinYm { get; private set; }

        public int HpId { get; set; }

        public int UserAct { get; set; }
    }
}
