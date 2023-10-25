using Domain.Models.Insurance;
using Helper.Common;
using Helper.Extension;

namespace Domain.Models.RsvInf
{
    public class RsvInfToConfirmModel
    {
        public RsvInfToConfirmModel(string ptName, int hpId, int sinDate, long raiinNo, long ptId, long ptNum, int birthday, int tantoId, int kaId, List<HokenInfModel> hokenInfModels)
        {
            PtName = ptName;
            HpId = hpId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            PtId = ptId;
            PtNum = ptNum;
            Birthday = birthday;
            TantoId = tantoId;
            KaId = kaId;
            ListPtHokenInfModel = hokenInfModels;
        }

        public string PtName { get; private set; }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public int Birthday { get; private set; }

        public int TantoId { get; private set; }

        public int KaId { get; private set; }

        public List<HokenInfModel> ListPtHokenInfModel { get; private set; }

        public string PtNumDisplay
        {
            get => PtNum > 0 ? PtNum.AsString() : string.Empty;
        }

        public int Age
        {
            get
            {
                if (Birthday > 0)
                {
                    return CIUtil.SDateToAge(Birthday, CIUtil.DateTimeToInt(DateTime.Today));
                }
                return 0;
            }
        }
    }
}
