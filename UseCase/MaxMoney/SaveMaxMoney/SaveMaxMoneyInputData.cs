using Domain.Models.MaxMoney;
using UseCase.Core.Sync.Core;

namespace UseCase.MaxMoney.SaveMaxMoney
{
    public class SaveMaxMoneyInputData : IInputData<SaveMaxMoneyOutputData>
    {
        public SaveMaxMoneyInputData(List<LimitListModel> listLimits, int hpId,long ptId, int kohiId, int sinYM)
        {
            ListLimits = listLimits;
            HpId = hpId;
            PtId = ptId;
            KohiId = kohiId;
            SinYM = sinYM;
        }
        public List<LimitListModel> ListLimits { get; private set; }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int KohiId { get; private set; }
        public int SinYM { get; private set; }
    }
}
